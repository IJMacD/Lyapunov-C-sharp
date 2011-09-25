using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lyapunov
{
    public partial class WizardForm : Form
    {
        System.Collections.ObjectModel.Collection<LyapunovGenerator> Lyaps = new System.Collections.ObjectModel.Collection<LyapunovGenerator>();
        System.Collections.ObjectModel.Collection<Configuration> Confs = new System.Collections.ObjectModel.Collection<Configuration>();
        //Timer endtime = new Timer();
        DateTime StartTime;
        TimeSpan[] colsdur = new TimeSpan[5];
        DateTime LastCol;
        TimeSpan[] picsdur = new TimeSpan[5];
        DateTime LastPic;

        public WizardForm()
        {
            InitializeComponent();
            
            for (int i = 0; i < System.Environment.ProcessorCount; i++)
            {
                Lyaps.Add(new LyapunovGenerator());
                InitLyap(Lyaps[Lyaps.Count - 1]);
            }
            //endtime.Interval = 500;
            //endtime.Tick += new EventHandler(endtime_Tick);
        }
        
        private void InitLyap(LyapunovGenerator lyap)
        {
            lyap.Progressed += Lyap_ColumnCompleted;
            lyap.Completed += Lyap_PicCompleted;
            lyap.LayerCompleted += lyap_LayerCompleted;
            //lyap.Died += new LyapunovGenerator.DiedHandler(Lyap_Died);
        }


        //void endtime_Tick(object sender, EventArgs e)
        //{
        //    if (lasttimecheck != DateTime.MinValue)
        //    {
        //        int progress = Lyaps[0].LastCol - lastcolcheck;
        //        if (progress == 0) return;
        //        TimeSpan since = DateTime.Now - lasttimecheck;
        //        //System.Diagnostics.Debugger.Log(1, "", since.ToString() + " " + Lyaps[0].LastCol.ToString() + " " + Lyaps[0].EndCol.ToString() + "\n");
        //        TimeSpan togo = TimeSpan.FromMilliseconds(since.TotalMilliseconds / progress  * (Lyaps[0].EndCol - Lyaps[0].LastCol));
        //        DateTime estimated = DateTime.Now + togo;
        //        label14.Text = "Estimated Time Left: " + togo.ToString();
        //        label15.Text = "Estimated Finish Time: " + estimated.ToString();
        //    }
        //    lasttimecheck = DateTime.Now;
        //    lastcolcheck = Lyaps[0].LastCol;
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double XMin = double.Parse(_x_min_txt.Text);
                double XMax = double.Parse(_x_max_txt.Text);
                double YMin = double.Parse(_y_min_txt.Text);
                double YMax = double.Parse(_y_max_txt.Text);
                double ZMin = double.Parse(_z_min_txt.Text);
                double ZMax = double.Parse(_z_max_txt.Text);
                int Iterations = int.Parse(_iterations_txt.Text);
                int PicHeight = int.Parse(textBox2.Text);
                int PicWidth = int.Parse(textBox3.Text);
                int PicDepth = int.Parse(textBox7.Text);
                if (!checkBox1.Checked) PicDepth = 1;
                double startInitX = double.Parse(textBox4.Text);
                double endInitX = double.Parse(textBox5.Text);
                double stepInitX = double.Parse(textBox6.Text);
                char[] Pattern = _pattern_txt.Text.ToCharArray();

                progressBar1.Maximum = PicWidth;

                progressBar2.Maximum = PicDepth;

                if (stepInitX <= 0) stepInitX = 0.1;

                for (double j = startInitX; j <= endInitX; j += stepInitX)
                {
                    Confs.Add(new Configuration(XMin, XMax, YMin, YMax, ZMin, ZMax, Pattern, Iterations, j, PicWidth, PicHeight, PicDepth));
                }
                if (Confs.Count > 1)
                {
                    progressBar3.Maximum = Confs.Count;
                    progressBar3.Visible = true;
                }
                else progressBar3.Visible = false;
                StartTime = DateTime.Now;
                for (int k = 0; k < Lyaps.Count; k++)
                {
                    if (Confs.Count > 0)
                    {
                        Lyaps[k].Initialise(Confs[0]);
                        Lyaps[k].Generate();
                        LastCol = DateTime.Now;
                        LastPic = DateTime.Now;
                        Confs.RemoveAt(0);
                    }
                    else break;
                    label16.Text = "Generating...";
                }
            }
            catch
            {
                MessageBox.Show("Please review your settings there seems to be an error.");
            }
        }

        string CalcDuration()
        {
            string result;
            if (Lyaps.Count > 1)
            {
                TimeSpan total = new TimeSpan(), max = new TimeSpan();
                foreach (LyapunovGenerator Lyap in Lyaps)
                {
                    if (Lyap.Duration > max) max = Lyap.Duration;
                    total += Lyap.Duration;
                }
                result = "Real time: " + max.ToString() + " Calculation Time: " + total.ToString();
            }
            else
            {
                result = (DateTime.Now - StartTime).ToString();
            }
            return result;
        }

        void lyap_LayerCompleted(object src, LyapunovGenerator.LayerCompletedEventArgs e)
        {
            e.Layer.Save(folderBrowserDialog1.SelectedPath + "\\" + GetFilename((LyapunovGenerator)src) + ".jpg");
            if (progressBar2.Value < progressBar2.Maximum)
            {
                progressBar2.Value = e.Z + 1;
                for (int i = 0; i < picsdur.Length - 1; i++)
                {
                    picsdur[i] = picsdur[i + 1];
                }
                picsdur[picsdur.Length - 1] = DateTime.Now - LastPic;
                LastPic = DateTime.Now;
                if (e.Z < picsdur.Length) return;
                TimeSpan sum = TimeSpan.FromSeconds(0);
                foreach (TimeSpan picdur in picsdur)
                {
                    sum += picdur;
                }
                double ave = (sum.TotalSeconds / picsdur.Length);
                //System.Diagnostics.Debugger.Log(1, "", ave.ToString() + "\n");
                TimeSpan togo = TimeSpan.FromSeconds(ave * (Lyaps[0].EndLayer - Lyaps[0].LastLayer));
                DateTime finish = DateTime.Now + togo;
                label17.Text = "Estimated Time Left: " + togo.ToString();
                label8.Text = "Estimated Finish Time: " + finish.ToString();
            }
        }

        private string GetFilename(Generator Lyap)
        {
            string p1 = Lyap.MinX.ToString() + "-" +
                Lyap.MaxX.ToString() + "-" +
                Lyap.PicWidth.ToString() + "-" +
                Lyap.MinY.ToString() + "-" +
                Lyap.MaxY.ToString() + "-" +
                Lyap.PicHeight.ToString() + "-";
            string p2 = Lyap.PicDepth > 1 ? Lyap.MinZ.ToString() + "-" +
                Lyap.MaxZ.ToString() + "-" +
                Lyap.PicDepth.ToString() + "-" : "";
            string p3 = new string(Lyap.Pattern) + "-" +
                Lyap.Iterations.ToString() + "-" +
                Lyap.InitX;
            return p1 + p2 + p3 + ".jpg";
        }

        void Lyap_PicCompleted(object src, EventArgs e)
        {
            if (progressBar3.Value + 1 < progressBar3.Maximum)
            {
                progressBar3.Value++;
                LyapunovGenerator lyap = (LyapunovGenerator)src;
                //Lyaps[0].Image.Save(textBox1.Text + lyap.InitX + ".jpg");
                if (Confs.Count > 0)
                {
                    lyap.Initialise(Confs[0]);
                    lyap.Generate();
                    Confs.RemoveAt(0);
                }
                else
                {
                    //completed = true;
                    label16.Text = "";
                    progressBar1.Value = 0;
                    progressBar2.Value = 0;
                    progressBar3.Value = 0;
                    string msgText = "Completed in: " + CalcDuration();
                    MessageBox.Show(msgText);
                    //if (textBox1.Text == "") saveFileDialog1.ShowDialog();
                    //MessageBox.Show("Completed!");
                }
            }
        }

        void Lyap_ColumnCompleted(object src, Generator.ProgressedEventArgs e)
        {
            if (e.X < progressBar1.Maximum)
            {
                progressBar1.Value = e.X + 1;
                for (int i = 0; i < colsdur.Length - 1; i++)
                {
                    colsdur[i] = colsdur[i + 1];
                }
                colsdur[colsdur.Length - 1] = DateTime.Now - LastCol;
                LastCol = DateTime.Now;
                if (e.X < colsdur.Length) return;
                TimeSpan sum = TimeSpan.FromSeconds(0);
                foreach (TimeSpan coldur in colsdur)
                {
                    sum += coldur;
                }
                double ave = (sum.TotalSeconds / colsdur.Length);
                //System.Diagnostics.Debugger.Log(1, "", ave.ToString() + "\n");
                TimeSpan togo = TimeSpan.FromSeconds(ave * (Lyaps[0].EndCol - Lyaps[0].LastCol));
                DateTime finish = DateTime.Now + togo;
                label14.Text = "Estimated Time Left: " + togo.ToString();
                label15.Text = "Estimated Finish Time: " + finish.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox1.Text = folderBrowserDialog1.SelectedPath;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Enabled = checkBox1.Checked;
            progressBar2.Visible = checkBox1.Checked;
            label17.Visible = checkBox1.Checked;
            label8.Visible = checkBox1.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            foreach (LyapunovGenerator Lyap in Lyaps)
            {
                Lyap.Stop();
            }
            progressBar1.Value = 0;
            progressBar2.Value = 0;
            progressBar3.Value = 0;
            label14.Text = "Estimated Time Left: ";
            label15.Text = "Estimated Finish Time: ";
            label17.Text = "Estimated Time Left: ";
            label8.Text = "Estimated Finish Time: ";

        }

        private void WizardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }
    }
}
