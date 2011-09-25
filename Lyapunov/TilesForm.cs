using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lyapunov
{
    public partial class TilesForm : Form
    {
        System.Collections.ObjectModel.Collection<LyapunovGenerator> Lyaps = new System.Collections.ObjectModel.Collection<LyapunovGenerator>();
        System.Collections.ObjectModel.Collection<Configuration> Confs = new System.Collections.ObjectModel.Collection<Configuration>();
        int fileError = 0;
        BackgroundWorker _ConfAdder;

        //int limit;

        const double XMIN = -10;
        const double XMAX = 10;
        const double YMIN = -10;
        const double YMAX = 10;
        const double InitX = 0.5;
        const int PicWidth = 256;
        const int PicHeight = 256;
        readonly char[] Pattern;

        public TilesForm()
        {
            Pattern = new char[]{ 'a', 'b' };
            InitializeComponent();
            for (int i = 0; i < System.Environment.ProcessorCount; i++)
            {
                Lyaps.Add(new LyapunovGenerator());
                InitLyap(Lyaps[Lyaps.Count - 1]);
            }
            InitWorker();
        }

        private void InitLyap(LyapunovGenerator lyap)
        {
            lyap.Progressed += Lyap_ColumnCompleted;
            lyap.Completed += Lyap_PicCompleted;
            lyap.LayerCompleted += lyap_LayerCompleted;
        }

        private void InitWorker()
        {
            if (_ConfAdder != null)
            {
                _ConfAdder.DoWork -= new DoWorkEventHandler(_ConfAdder_DoWork);
                _ConfAdder.ProgressChanged -= new ProgressChangedEventHandler(_ConfAdder_ProgressChanged);
                _ConfAdder.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_ConfAdder_RunWorkerCompleted);
                _ConfAdder.Dispose();
            }

            _ConfAdder = new BackgroundWorker();
            _ConfAdder.DoWork += new DoWorkEventHandler(_ConfAdder_DoWork);
            _ConfAdder.ProgressChanged += new ProgressChangedEventHandler(_ConfAdder_ProgressChanged);
            _ConfAdder.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_ConfAdder_RunWorkerCompleted);
            _ConfAdder.WorkerReportsProgress = true;
            _ConfAdder.WorkerSupportsCancellation = true;
        }

        void _ConfAdder_DoWork(object sender, DoWorkEventArgs e)
        {
            Job job = (Job)e.Argument;
            int limit = (int)Math.Pow(2, job.Zoom);

            double each_tile_x = (XMAX - XMIN) / limit; // 2 = 
            double each_tile_y = (YMAX - YMIN) / limit; // 2 = 

            for (int i = 0; i < limit; i++)
            {
                for (int j = 0; j < limit; j++)
                {
                    if (!System.IO.File.Exists(job.Path + "\\" + job.Zoom + "\\" + i + "_" + j + ".png"))
                    {
                        double XMin = (i * each_tile_x) + XMIN; // 2 = 
                        double XMax = XMin + each_tile_x;
                        double YMin = ((limit - j - 1) * each_tile_y) + YMIN; // 2 = 
                        double YMax = YMin + each_tile_y;

                        Confs.Add(new Configuration(XMin, XMax, YMin, YMax, Pattern, job.Iterations, InitX, PicWidth, PicHeight, i, j, job.Zoom, job.Path));
                    }
                    _ConfAdder.ReportProgress(i * limit + j);
                    if (_ConfAdder.CancellationPending) { i = limit; j = limit; }
                }
            }
        }

        void _ConfAdder_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            prog_jobs.Value = e.ProgressPercentage;
            prog_tiles.Maximum = Confs.Count;
            lbl_count.Text = Confs.Count.ToString() + " pictures to generate";
        }

        void _ConfAdder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            prog_jobs.Value = 0;
            lbl_count.Text = Confs.Count.ToString() + " pictures to generate";

            btn_add.Text = "Add";

            if (btn_start.Text == "Stop")
            {
                for (int k = 0; k < Lyaps.Count; k++)
                {
                    if (Confs.Count > 0)
                    {
                        NextConf(Lyaps[k]);
                    }
                    else break;
                }
            }
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            txt_folder.Text = folderBrowserDialog1.SelectedPath;
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (btn_start.Text == "Start")
            {
                btn_start.Text = "Stop";
                if (listView1.Items.Count == 0)
                {
                    int zoom = int.Parse(txt_zoom.Text);
                    int iterations = int.Parse(txt_iterations.Text);
                    addConfs(zoom, iterations, folderBrowserDialog1.SelectedPath);
                    lbl_count.Text = Confs.Count.ToString() + " pictures to generate";
                }

                for (int k = 0; k < Lyaps.Count; k++)
                {
                    if (Confs.Count > 0) NextConf(Lyaps[k]);
                    else break;
                }
                prog_tiles.Maximum = Confs.Count;
            }
            else
            {
                foreach (LyapunovGenerator lyap in Lyaps)
                {
                    lyap.Stop();
                }
                //Confs.Clear();
                prog_pri.Value = 0;
                prog_tiles.Value = 0;
                btn_start.Text = "Start";
            }
        }

        private void addConfs(int zoom, int iterations, string path)
        {
            prog_pri.Maximum = 257;
            prog_jobs.Maximum = (int)Math.Pow(2, 2 * zoom);
            prog_jobs.Value = 0;

            Job j = new Job(zoom, iterations, path);
            _ConfAdder.RunWorkerAsync(j);
        }

        void lyap_LayerCompleted(object src, LyapunovGenerator.LayerCompletedEventArgs e)
        {
            LyapunovGenerator sender = (LyapunovGenerator)src;
            try
            {
                if (!System.IO.Directory.Exists(sender.Conf._path + "\\" + sender.Conf._z)) System.IO.Directory.CreateDirectory(sender.Conf._path + "\\" + sender.Conf._z);
                if (e.Z != -1)
                {
                    //System.IO.File.Delete(sender.Conf._path + "\\" + sender.Conf._z + "\\" + sender.Conf._x + "_" + sender.Conf._y + ".png");
                    e.Layer.Save(sender.Conf._path + "\\" + sender.Conf._z + "\\" + sender.Conf._x + "_" + sender.Conf._y + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch
            {
                Confs.Clear();
                //MessageBox.Show("Problem with: " + sender.Conf._z + "x (" + sender.Conf._x + ", " + sender.Conf._y + ") please sort out!!!");
            
                //MessageBox.Show("File Error");
                if (fileError < 5)
                {
                    fileError++;
                    Confs.Add(sender.Conf);
                }
                else
                {
                    //Confs.Clear();
                    foreach (LyapunovGenerator lyap in Lyaps) lyap.Stop();
                    if (fileError == 5) MessageBox.Show("Could not save");
                    fileError = 0;
                    btn_start.Text = "Start";
                }
            }
        }

        void Lyap_PicCompleted(object src, EventArgs e)
        {
            if (prog_tiles.Value < prog_tiles.Maximum)
            {
                prog_tiles.Value++;
            }
                LyapunovGenerator lyap = (LyapunovGenerator)src;
                //Lyaps[0].Image.Save(textBox1.Text + lyap.InitX + ".jpg");
                if (Confs.Count > 0)
                {
                    NextConf(lyap);
                }
                else
                {
                    //completed = true;
                    prog_pri.Value = 0;
                    prog_tiles.Value = 0;
                    btn_start.Text = "Start";
                }
        }

        private void NextConf(LyapunovGenerator lyap)
        {
            if (Confs.Count > 0)
            {
                if (!lyap.Working)
                {
                    if (System.IO.File.Exists(Confs[0]._path + "\\" + Confs[0]._z + "\\" + Confs[0]._x + "_" + Confs[0]._y + ".png"))
                    {
                        Confs.RemoveAt(0);
                        lbl_count.Text = Confs.Count.ToString() + " pictures to generate";
                        NextConf(lyap);
                    }
                    else
                    {
                        try
                        {
                            if (!System.IO.Directory.Exists(Confs[0]._path + "\\" + Confs[0]._z)) System.IO.Directory.CreateDirectory(Confs[0]._path + "\\" + Confs[0]._z);
                            System.IO.Stream str = System.IO.File.Create(Confs[0]._path + "\\" + Confs[0]._z + "\\" + Confs[0]._x + "_" + Confs[0]._y + ".png");
                            str.Dispose();
                        }
                        catch
                        {
                            Confs.RemoveAt(0);
                            lbl_count.Text = Confs.Count.ToString() + " pictures to generate";
                            NextConf(lyap);
                        }
                        lyap.Initialise(Confs[0]);
                        lyap.Generate();
                        Confs.RemoveAt(0);
                        lbl_count.Text = Confs.Count.ToString() + " pictures to generate";
                    }
                }
            }
        }

        void Lyap_ColumnCompleted(object src, Generator.ProgressedEventArgs e)
        {
            if (src == Lyaps[0] && e.X < prog_pri.Maximum)
            {
                prog_pri.Value = e.X + 1;
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (btn_add.Text == "Add")
            {
                btn_add.Text = "Stop";
                //listView1.Items.Add(new Job(int.Parse(txt_zoom.Text), int.Parse(txt_iterations.Text), txt_folder.Text));
                ListViewItem.ListViewSubItem[] subs = new ListViewItem.ListViewSubItem[3];
                subs[0] = new ListViewItem.ListViewSubItem();
                subs[0].Tag = "Zoom";
                subs[0].Text = txt_zoom.Text;
                subs[1] = new ListViewItem.ListViewSubItem();
                subs[1].Tag = "Iterations";
                subs[1].Text = txt_iterations.Text;
                subs[2] = new ListViewItem.ListViewSubItem();
                subs[2].Tag = "Path";
                subs[2].Text = txt_folder.Text;
                listView1.Items.Add(new ListViewItem(subs, 0));

                addConfs(int.Parse(txt_zoom.Text), int.Parse(txt_iterations.Text), txt_folder.Text);
            }
            else
            {
                btn_add.Text = "Add";
                _ConfAdder.CancelAsync();
            }
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count == 1)
            {
                listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
            }
            if (btn_start.Text == "Start" && listView1.Items.Count == 0) Confs.Clear();
        }
    }

    struct Job
    {
        private int _zoom;
        private int _iterations;
        private string _path;

        public int Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }
        public int Iterations
        {
            get { return _iterations; }
            set { _iterations = value; }
        }
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public Job(int zoom, int iterations, string path)
        {
            _zoom = zoom;
            _iterations = iterations;
            _path = path;
        }
    }
}
