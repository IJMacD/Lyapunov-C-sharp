using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Lyapunov
{
    public partial class SingleTileForm : Form
    {
        System.Collections.ObjectModel.Collection<LyapunovGenerator> Lyaps = new System.Collections.ObjectModel.Collection<LyapunovGenerator>();
        System.Collections.ObjectModel.Collection<Configuration> Confs = new System.Collections.ObjectModel.Collection<Configuration>();
        //int fileError = 0;
        //BackgroundWorker _ConfAdder;
        Image _Image;
        int refreshrate = 20;
        int waitrefresh = 0;
        int colsDone;

        //int limit;

        const double XMIN = 2;
        const double XMAX = 4;
        const double YMIN = 2;
        const double YMAX = 4;
        const double InitX = 0.5;
        const int PicWidth = 256;
        const int PicHeight = 256;
        readonly char[] Pattern;

        private delegate void UICallerDelegate();

        public SingleTileForm()
        {
            Pattern = new char[]{ 'a', 'b' };
            InitializeComponent();
            for (int i = 0; i < System.Environment.ProcessorCount; i++)
            {
                Lyaps.Add(new LyapunovGenerator());
                InitLyap(Lyaps[Lyaps.Count - 1]);
            }
        }

        private void InitLyap(LyapunovGenerator lyap)
        {
            lyap.Progressed += Lyap_ColumnCompleted;
            lyap.Completed += Lyap_PicCompleted;
            lyap.LayerCompleted += lyap_LayerCompleted;
        }

        //private void InitWorker()
        //{
        //    if (_ConfAdder != null)
        //    {
        //        _ConfAdder.DoWork -= new DoWorkEventHandler(_ConfAdder_DoWork);
        //        _ConfAdder.ProgressChanged -= new ProgressChangedEventHandler(_ConfAdder_ProgressChanged);
        //        _ConfAdder.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_ConfAdder_RunWorkerCompleted);
        //        _ConfAdder.Dispose();
        //    }

        //    _ConfAdder = new BackgroundWorker();
        //    _ConfAdder.DoWork += new DoWorkEventHandler(_ConfAdder_DoWork);
        //    _ConfAdder.ProgressChanged += new ProgressChangedEventHandler(_ConfAdder_ProgressChanged);
        //    _ConfAdder.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_ConfAdder_RunWorkerCompleted);
        //    _ConfAdder.WorkerReportsProgress = true;
        //    _ConfAdder.WorkerSupportsCancellation = true;
        //}

        //void _ConfAdder_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    Job job = (Job)e.Argument;
        //    int limit = (int)Math.Pow(2, job.Zoom);

        //    double each_tile_x = (XMAX - XMIN) / limit; // 2 = 
        //    double each_tile_y = (YMAX - YMIN) / limit; // 2 = 

        //    for (int i = 0; i < limit; i++)
        //    {
        //        for (int j = 0; j < limit; j++)
        //        {
        //            if (!System.IO.File.Exists(job.Path + "\\" + job.Zoom + "\\" + i + "_" + j + ".png"))
        //            {
        //                double XMin = (i * each_tile_x) + XMIN; // 2 = 
        //                double XMax = XMin + each_tile_x;
        //                double YMin = ((limit - j - 1) * each_tile_y) + YMIN; // 2 = 
        //                double YMax = YMin + each_tile_y;

        //                Confs.Add(new Configuration(XMin, XMax, YMin, YMax, Pattern, job.Iterations, InitX, PicWidth, PicHeight, i, j, job.Zoom, job.Path));
        //            }
        //            _ConfAdder.ReportProgress(i * limit + j);
        //            if (_ConfAdder.CancellationPending) { i = limit; j = limit; }
        //        }
        //    }
        //}

        //void _ConfAdder_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    prog_jobs.Value = e.ProgressPercentage;
        //    lbl_count.Text = Confs.Count.ToString() + " pictures to generate";
        //}

        //void _ConfAdder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    prog_jobs.Value = 0;
        //    lbl_count.Text = Confs.Count.ToString() + " pictures to generate";

        //    btn_save.Text = "Add";

        //    if (btn_start.Text == "Stop")
        //    {
        //        for (int k = 0; k < Lyaps.Count; k++)
        //        {
        //            if (Confs.Count > 0)
        //            {
        //                NextConf(Lyaps[k]);
        //            }
        //            else break;
        //        }
        //    }
        //}

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
                int zoom = int.Parse(txt_zoom.Text);
                int x = int.Parse(txt_X.Text);
                int y = int.Parse(txt_Y.Text);
                int iterations = int.Parse(txt_iterations.Text);
                addConfs(zoom, x, y, iterations);
                lbl_count.Text = Confs.Count.ToString() + " pictures to generate";

                for (int k = 0; k < Lyaps.Count; k++)
                {
                    Lyaps[k].map = rad_norm.Checked ? LyapunovGenerator.Pallet.Yellow : LyapunovGenerator.Pallet.Red;
                    if (Confs.Count > 0) NextConf(Lyaps[k]);
                    else break;
                }
            }
            else
            {
                foreach (LyapunovGenerator lyap in Lyaps)
                {
                    lyap.Stop();
                }
                //Confs.Clear();
                prog_pri.Value = 0;
                btn_start.Text = "Start";
            }
        }

        private void addConfs(int zoom, int x, int y, int iterations)
        {
            if (_output_pb.BackgroundImage != null) _output_pb.BackgroundImage.Dispose();

            int limit = (int)Math.Pow(2, zoom);

            double each_tile_x = (XMAX - XMIN) / limit; // 2 = 
            double each_tile_y = (YMAX - YMIN) / limit; // 2 = 

            double XMin = (x * each_tile_x) + XMIN; // 2 = 
            double XMax = XMin + each_tile_x;
            double YMin = ((limit - y - 1) * each_tile_y) + YMIN; // 2 = 
            double YMax = YMin + each_tile_y;

            colsDone = 0;
            prog_pri.Maximum = PicWidth;
            prog_pri.Value = 0;

            _Image = new Bitmap(PicWidth, PicHeight);
            _output_pb.BackgroundImage = _Image;
            _output_pb.Refresh();

            for (int j = 0; j < Lyaps.Count; j++)
            {
                double deltaX = (XMax - XMin) / Lyaps.Count;
                double xmin = XMin + (j * deltaX);
                double xmax = XMin + ((j + 1) * deltaX);
                int width = (int)(PicWidth / Lyaps.Count);
                int startX = (int)(PicWidth / Lyaps.Count * j);
                Confs.Add(new Configuration(xmin, xmax, YMin, YMax, Pattern, iterations, InitX, width, PicHeight, startX));
            }

        }

        //private void addConfs(int zoom, int iterations, string path)
        //{
        //    prog_pri.Maximum = 257;
        //    prog_jobs.Maximum = (int)Math.Pow(2, 2 * zoom);
        //    prog_jobs.Value = 0;

        //    Job j = new Job(zoom, iterations, path);
        //    _ConfAdder.RunWorkerAsync(j);
        //}

        void lyap_LayerCompleted(object src, LyapunovGenerator.LayerCompletedEventArgs e)
        {
            //LyapunovGenerator sender = (LyapunovGenerator)src;
            //try
            //{
            //    if (!System.IO.Directory.Exists(sender.Conf._path + "\\" + sender.Conf._z)) System.IO.Directory.CreateDirectory(sender.Conf._path + "\\" + sender.Conf._z);
            if (e.Z == -1)
            {
                MessageBox.Show("Recons all blue");
            }
            //}
            //catch
            //{
            //    //MessageBox.Show("File Error");
            //    if (fileError < 5)
            //    {
            //        fileError++;
            //        Confs.Add(sender.Conf);
            //    }
            //    else
            //    {
            //        Confs.Clear();
            //        if (fileError == 5) MessageBox.Show("Could not save");
            //        fileError = 0;
            //        btn_start.Text = "Start";
            //    }
            //}
        }

        void Lyap_PicCompleted(object src, EventArgs e)
        {
                //completed = true;
                prog_pri.Value = 0;
                btn_start.Text = "Start";
                _output_pb.BackgroundImage = _Image;
                 UICallerDelegate dlg = new UICallerDelegate(UpdatePicBox);
                BeginInvoke(dlg, null);

            bool alldone = true;

            foreach (LyapunovGenerator lyap in Lyaps) if (!lyap.IsComplete) alldone = false;

            if (alldone)
            {
                colsDone = 0;
                UICallerDelegate delg = new UICallerDelegate(UpdateProgBar);
                BeginInvoke(delg, null);
            }
        }

        private void UpdatePicBox()
        {
            _output_pb.Refresh();
        }

        private void UpdateProgBar()
        {
            if (colsDone < prog_pri.Maximum)
            {
                prog_pri.Value = colsDone;
            }
        }

        private void AddColumn(int X, Bitmap image)
        {
            try
            {
                Graphics g = Graphics.FromImage(_Image);
                g.DrawImage(image, X, 0);
                g.Dispose();
            }
            catch
            {
                AddColumn(X, image);
            }
        }

        private void NextConf(LyapunovGenerator lyap)
        {
            if (Confs.Count > 0)
            {
                if (!lyap.Working)
                {
                    lyap.Initialise(Confs[0]);
                    lyap.Generate();
                    Confs.RemoveAt(0);
                    lbl_count.Text = Confs.Count.ToString() + " pictures to generate";
                }
            }
        }

        void Lyap_ColumnCompleted(object src, Generator.ProgressedEventArgs e)
        {
            if (_Image == null) return;
            colsDone++;
            AddColumn(e.X, e.Image);
            //ImageUpdateEventArgs args = new ImageUpdateEventArgs();
            //args.X = e.X;
            //args.Column = e.Column;
            //pictureBox1.Image = e.Column;
            if (waitrefresh >= refreshrate)
            {
                waitrefresh = 0;
                _output_pb.BackgroundImage = _Image;
                UICallerDelegate dlg = new UICallerDelegate(UpdatePicBox);
                BeginInvoke(dlg, null);
            }
            else
            {
                waitrefresh++;
            }
            try
            {
                UICallerDelegate delg = new UICallerDelegate(UpdateProgBar);
                BeginInvoke(delg, null);
            }
            catch { }
        }

        //private void btn_add_Click(object sender, EventArgs e)
        //{
        //    if (btn_save.Text == "Add")
        //    {
        //        btn_save.Text = "Stop";
        //        //listView1.Items.Add(new Job(int.Parse(txt_zoom.Text), int.Parse(txt_iterations.Text), txt_folder.Text));
        //        ListViewItem.ListViewSubItem[] subs = new ListViewItem.ListViewSubItem[3];
        //        subs[0] = new ListViewItem.ListViewSubItem();
        //        subs[0].Tag = "Zoom";
        //        subs[0].Text = txt_zoom.Text;
        //        subs[1] = new ListViewItem.ListViewSubItem();
        //        subs[1].Tag = "Iterations";
        //        subs[1].Text = txt_iterations.Text;
        //        subs[2] = new ListViewItem.ListViewSubItem();
        //        subs[2].Tag = "Path";
        //        subs[2].Text = txt_folder.Text;
        //        listView1.Items.Add(new ListViewItem(subs, 0));

        //        addConfs(int.Parse(txt_zoom.Text), int.Parse(txt_iterations.Text), txt_folder.Text);
        //    }
        //    else
        //    {
        //        btn_save.Text = "Add";
        //        _ConfAdder.CancelAsync();
        //    }
        //}

        //private void btn_remove_Click(object sender, EventArgs e)
        //{
        //    if (listView1.SelectedIndices.Count == 1)
        //    {
        //        listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
        //    }
        //}

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                int zoom = int.Parse(txt_zoom.Text);
                int x = int.Parse(txt_X.Text);
                int y = int.Parse(txt_Y.Text);
                if (!System.IO.Directory.Exists(txt_folder.Text + "\\" + zoom)) System.IO.Directory.CreateDirectory(txt_folder.Text + "\\" + zoom);
                if (1 != -1) // was e.Z
                {
                    _Image.Save(txt_folder.Text + "\\" + zoom + "\\" + x + "_" + y + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch
            {
                MessageBox.Show("Could not save");
            }
        }
    }
}
