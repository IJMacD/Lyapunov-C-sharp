using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;


namespace Lyapunov
{
    public partial class MainForm : Form
    {
        int selStartX, selStartY, selFinX, selFinY;
        bool mouseDown;

        double XMin, XMax, YMin, YMax;
        char[] Pattern;
        int Iterations;
        double InitX = 0.5; // <----- Initial X
        int PicWidth;
        int PicHeight;

        Collection<LyapunovGenerator> Lyaps = new Collection<LyapunovGenerator>();
        //LyapunovGenerator[] Lyaps = new LyapunovGenerator[2];
        Image _Image;
        //{
        //    get
        //    {
        //        Bitmap img = new Bitmap(PicWidth, PicHeight);
        //        Graphics g = Graphics.FromImage(img);
        //        foreach (LyapunovGenerator Lyap in Lyaps)
        //        {
        //            lock (LyapunovGenerator.padlock)
        //            {
        //                Bitmap chunk = Lyap.Image;
        //                if (chunk != null) g.DrawImage(chunk, Lyap.StartCol, 0);
        //            }
        //        }
        //        g.Dispose();
        //        return img;
        //    }

        //}
        int refreshrate = 20;
        int waitrefresh = 0;
        int colsDone;
        Socket _mainSocket;
        const int port = 2000;
        ArrayList _workerSocketList = ArrayList.Synchronized(new ArrayList());

        public MainForm()
        {
            InitializeComponent();
            InitialiseNetwork();
            for (int i = 0; i < System.Environment.ProcessorCount; i++)
            {
                Lyaps.Add(null);
                Lyaps[Lyaps.Count - 1] = new LyapunovGenerator();
                InitLyap(Lyaps[Lyaps.Count - 1]);
            }
        }

        void InitialiseNetwork()
        {
            try
            {
                _mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, port);
                _mainSocket.Bind(ipLocal);
                _mainSocket.Listen(4);
                _mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                Lyaps.Add(new LyapunovGenerator(_mainSocket.EndAccept(asyn)));
                _mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
                Lyaps[Lyaps.Count - 1].SetRemote(LyapunovGenerator.TypeofRemote.Sender);
                InitLyap(Lyaps[Lyaps.Count - 1]);
                UICallerDelegate dlg = new UICallerDelegate(UpdateStatusStripNC);
                BeginInvoke(dlg, null);
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }
        private void UpdateStatusStripNC()
        {
                statusStrip1.Text = "Client Connected";
        }

        private void InitLyap(LyapunovGenerator lyap)
        {
            lyap.ColumnCompleted += new LyapunovGenerator.ColumnCompletedHandler(Lyap_ColumnCompleted);
            lyap.PicCompleted += new LyapunovGenerator.PicCompletedHandler(Lyap_PicCompleted);
            lyap.Died += new LyapunovGenerator.DiedHandler(Lyap_Died);
        }

        private bool parseArgs()
        {
            try
            {
                XMin = double.Parse(_x_min_txt.Text);
                XMax = double.Parse(_x_max_txt.Text);
                YMin = double.Parse(_y_min_txt.Text);
                YMax = double.Parse(_y_max_txt.Text);
                Iterations = int.Parse(_iterations_txt.Text);
                Pattern = _pattern_txt.Text.ToCharArray();
            }
            catch
            {
                MessageBox.Show("Please review your settings there seems to be an error.");
                return false;
            }
            return true;
        }

        void Generate()
        {

            if (_output_pb.BackgroundImage != null) _output_pb.BackgroundImage.Dispose();

            //CreateForegroundBitmap();

            PicWidth = _output_pb.Width;
            PicHeight = _output_pb.Height;

            colsDone = 0;
            toolStripStatusLabel1.Text = "Generating...";
            toolStripProgressBar1.Maximum = PicWidth;
            toolStripProgressBar1.Value = 0;

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
                if (Lyaps[j] == null)
                {
                    Lyaps[j] = new LyapunovGenerator();
                    InitLyap(Lyaps[j]);
                }
                Lyaps[j].Initialise(xmin, xmax, YMin, YMax, Pattern, Iterations, InitX, width, PicHeight, startX);
                Lyaps[j].Generate();
            }
        }

        void Lyap_Died(object src)
        {
            Lyaps.Remove((LyapunovGenerator)src);
        }

        void Lyap_PicCompleted(object src)
        {
            _output_pb.BackgroundImage = _Image;
            UICallerDelegate dlg = new UICallerDelegate(UpdatePicBox);
            BeginInvoke(dlg, null);
            Collection<LyapunovGenerator> complete = new Collection<LyapunovGenerator>();
            Collection<LyapunovGenerator> incomplete = new Collection<LyapunovGenerator>();
            foreach (LyapunovGenerator Lyap in Lyaps)
            {
                if (Lyap.Completed)
                {
                //    complete.Add(Lyap);
                //    if (incomplete.Count > 0)
                //    {
                //        if (incomplete[0].EndCol - incomplete[0].LastCol > 10)
                //        {
                //            double oldendx = incomplete[0].EndX;
                //            int oldendcol = incomplete[0].EndCol;
                //            int newendcol = (incomplete[0].EndCol + incomplete[0].LastCol) / 2;
                //            incomplete[0].EndCol = newendcol;
                //            Lyap.Initialise(Lyap.EndX, oldendx, YMin, YMax, Pattern, Iterations, InitX, Lyap.EndCol - Lyap.StartCol, PicHeight, Lyap.EndCol);
                //            Lyap.Generate();
                //            incomplete.RemoveAt(0);
                //        }
                //    }
                }
                else
                {
                    incomplete.Add(Lyap);
                    //if (complete.Count > 0)
                    //{
                    //    if (Lyap.EndCol - Lyap.LastCol > 10)
                    //    {

                    //        double oldendx = Lyap.EndX;
                    //        int oldendcol = Lyap.EndCol;
                    //        int newendcol = (Lyap.EndCol + Lyap.LastCol) / 2;
                    //        Lyap.EndCol = newendcol;
                    //        complete[0].Initialise(Lyap.EndX, oldendx, YMin, YMax, Pattern, Iterations, InitX, Lyap.EndCol - Lyap.StartCol, PicHeight, Lyap.EndCol);
                    //        complete[0].Generate();
                    //        complete.RemoveAt(0);
                    //    }
                    //}
                }
            }
            if (incomplete.Count == 0)
            {
                string msgText = "Completed in: " + CalcDuration();
                toolStripStatusLabel1.Text = msgText;
                MessageBox.Show(msgText);
                colsDone = 0;
                UICallerDelegate delg = new UICallerDelegate(UpdateProgBar);
                BeginInvoke(delg, null);
            }
        }

        void Lyap_ColumnCompleted(object src, LyapunovGenerator.ColumnCompletedEventArgs e)
        {
            if (_Image == null) return;
            colsDone++;
            AddColumn(e.X, e.Column);
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

        private delegate void UICallerDelegate();

        private void UpdateProgBar()
        {
            if (colsDone < toolStripProgressBar1.Maximum)
            {
                toolStripProgressBar1.Value = colsDone;
            }
        }

        private void UpdatePicBox()
        {
            _output_pb.Refresh();
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
                result = Lyaps[0].Duration.ToString();
            }
            return result;
        }

        #region Zoom Code

        //private void _output_pb_MouseDown(object sender, MouseEventArgs e) { }
        //private void _output_pb_MouseMove(object sender, MouseEventArgs e) { }
        //private void _output_pb_MouseUp(object sender, MouseEventArgs e) { }

        //private void _output_pb_MouseDown(object sender, MouseEventArgs e)
        //{
        //    selStartX = e.X;
        //    selStartY = e.Y;
        //    mouseDown = true;
        //}

        //private void _output_pb_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (mouseDown)
        //    {
        //        if (_output_pb.Image == null)return;
        //        //
        //        _output_pb.Image = new Bitmap(_output_pb.Width, _output_pb.Height);
        //        //Graphics g = Graphics.FromImage(_output_pb.Image);

        //        Graphics g = Graphics.FromImage(_output_pb.Image);

        //        //ClearSelection();
        //        Pen pen = new Pen(Color.Black, 2);
        //        pen.DashPattern = new Single[] { 1, 1, 2, 1 };

        //        int smallerX = selStartX < selFinX ? selStartX : e.X;
        //        int biggerX = selStartX > selFinX ? selStartX : e.X;
        //        int smallerY = selStartY < selFinY ? selStartY : e.Y;
        //        int biggerY = selStartY > selFinY ? selStartY : e.Y;

        //        selStartX = smallerX;
        //        selFinX = biggerX;
        //        selStartY = smallerY;
        //        selFinY = biggerY;

        //        g.DrawRectangle(pen, smallerX, smallerY, biggerX - smallerX, biggerY - smallerY);
        //        _output_pb.Refresh();
        //        g.Dispose();
        //    }
        //}

        //private void _output_pb_MouseUp(object sender, MouseEventArgs e)
        //{
        //    Lyap.Stop();

        //    if ((e.X - selStartX > 0) && (e.Y - selStartY > 0))
        //    {
        //    double xmin = XMin + ((XMax - XMin) / _output_pb.Width) * selStartX;
        //    double xmax = XMin + ((XMax - XMin) / _output_pb.Width) * e.X;
        //    double ymin = YMin + ((YMax - YMin) / _output_pb.Height) * selStartY;
        //    double ymax = YMin + ((YMax - YMin) / _output_pb.Height) * e.Y;

        //    _x_min_txt.Text = xmin.ToString();
        //    _x_max_txt.Text = xmax.ToString();
        //    _y_min_txt.Text = ymin.ToString();
        //    _y_max_txt.Text = ymax.ToString();

        //    parseArgs();
        //    }
        //}

        //private void ClearSelection()
        //{
        //    Graphics g = Graphics.FromImage(_output_pb.Image);
        //    g.Clear(Color.Transparent);
        //    _output_pb.Refresh();
        //}

        Point _LastPoint;
        bool _MouseDown;

        private void _output_pb_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) //Handles _PictureBox.MouseDown
        {
            _MouseDown = true;
            _LastPoint = new Point(e.X, e.Y);
        }

        private void _output_pb_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_MouseDown)
            {
                int X1, Y1, X2, Y2;
                X1 = Math.Min(e.X, _LastPoint.X);
                X2 = Math.Max(e.X, _LastPoint.X) - X1;
                Y1 = Math.Min(e.Y, _LastPoint.Y);
                Y2 = Math.Max(e.Y, _LastPoint.Y) - Y1;
                Rectangle NewSelRect = new Rectangle(X1, Y1, X2, Y2);
                DrawSelection(NewSelRect);
            }
        }

        private void _output_pb_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (_MouseDown)
            {
                int X1, Y1, X2, Y2;
                //User may have selected from right to left
                X1 = Math.Min(e.X, _LastPoint.X);
                X2 = Math.Max(e.X, _LastPoint.X);
                Y1 = Math.Min(e.Y, _LastPoint.Y);
                Y2 = Math.Max(e.Y, _LastPoint.Y);
                ClearSelection();

                _MouseDown = false;

                if ((XMin == 0 && XMax == 0) || (YMin == 0 && YMax == 0)) parseArgs();

                double xmin = XMin + ((XMax - XMin) / _output_pb.Width) * X1;
                double xmax = XMin + ((XMax - XMin) / _output_pb.Width) * X2;
                double ymin = YMax - ((YMax - YMin) / _output_pb.Height) * Y1;
                double ymax = YMax - ((YMax - YMin) / _output_pb.Height) * Y2;

                _x_min_txt.Text = xmin.ToString();
                _x_max_txt.Text = xmax.ToString();
                _y_min_txt.Text = ymin.ToString();
                _y_max_txt.Text = ymax.ToString();

                //parseArgs();
            }
        }

        private void DrawSelection(Rectangle ZoomRect)
        {
            if (_output_pb.Image == null) _output_pb.Image = new Bitmap(_output_pb.Width, _output_pb.Height);
            Graphics g = Graphics.FromImage(_output_pb.Image);

            ClearSelection();
            Pen pen = new Pen(Color.LightBlue, 2);
            pen.DashPattern = new Single[] { 3, 3 };
            g.DrawRectangle(pen, ZoomRect);
            _output_pb.Refresh();
            g.Dispose();
        }

        private void ClearSelection()
        {
            if (_output_pb.Image == null) return;
            Graphics g = Graphics.FromImage(_output_pb.Image);
            g.Clear(Color.Transparent);
            _output_pb.Refresh();
        }

        private void CreateForegroundBitmap()
        {
            _output_pb.Image = new Bitmap(_output_pb.ClientRectangle.Width, _output_pb.ClientRectangle.Height);
            Graphics g = Graphics.FromImage(_output_pb.Image);
            g.Clear(Color.Transparent);
            g.Dispose();
        }

        #endregion

        #region Events

        private void strt_btn_Click(object sender, EventArgs e)
        {
            foreach (LyapunovGenerator Lyap in Lyaps)
            {
                if (Lyap != null) Lyap.Stop();
            }
            if(!parseArgs()) return;
            Generate();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            foreach (LyapunovGenerator Lyap in Lyaps)
            {
                if (Lyap != null) Lyap.Stop();
            }
            toolStripProgressBar1.Value = 0;
            toolStripStatusLabel1.Text = "";
            //byte xmax = Convert.ToByte(XMax);
            //MessageBox.Show(xmax.ToString());
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            _output_pb.BackgroundImage.Save(saveFileDialog1.FileName);
        }

        private void wizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WizardForm form = new WizardForm();
            form.Show();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _x_min_txt.Text = "2";
            _x_max_txt.Text = "4";
            _y_min_txt.Text = "2";
            _y_max_txt.Text = "4";
        }

        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridForm gf = new GridForm();
            gf.Show();
        }

        private void aboutLyapunovSpaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutLyapunov al = new AboutLyapunov();
            al.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.Show();
        }

        private void _output_pb_SizeChanged(object sender, EventArgs e)
        {
            trackBar1.Maximum = _output_pb.Width;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (LyapunovGenerator Lyap in Lyaps)
            {
                if (Lyap != null)
                {
                    Lyap.Stop();
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            refreshrate = trackBar1.Value;
        }
        #endregion

        private void tilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TilesForm frm = new TilesForm();
            frm.ShowDialog();
        }

        private void singleTileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SingleTileForm frm = new SingleTileForm();
            frm.ShowDialog();
        }

        private void benchmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BenchmarkForm frm = new BenchmarkForm();
            frm.ShowDialog();
        }
    }
}
