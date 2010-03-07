using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Lyapunov
{
    class LyapunovGenerator
    {
        public class ColumnCompletedEventArgs : EventArgs
        {
            public int X;
            public Bitmap Column;
        }
        public delegate void ColumnCompletedHandler(object src, ColumnCompletedEventArgs e);
        public event ColumnCompletedHandler ColumnCompleted;
        public class LayerCompletedEventArgs : EventArgs
        {
            public int Z;
            public Bitmap Layer;
        }
        public delegate void LayerCompletedHandler(object src, LayerCompletedEventArgs e);
        public event LayerCompletedHandler LayerCompleted;
        public delegate void PicCompletedHandler(object src);
        public event PicCompletedHandler PicCompleted;
        public delegate void DiedHandler(object src);
        public event DiedHandler Died;
        public enum TypeofRemote { Local, Sender, Reciever, Unknown };
        public enum Pallet { Yellow, Red, Green, Blue };

        BackgroundWorker _Worker;
        public static object padlock = new object();

        double _XMin, _XMax, _YMin, _YMax, _ZMin, _ZMax, _InitX;
        char[] _Pattern;
        int _Iterations, _PicHeight, _PicWidth, _PicDepth, _StartCol;

        Bitmap[] _Columns;
        //Bitmap[] _Layers;
        Bitmap[] _Images;
        DateTime _startTime, _finishTime;
        bool _Completed, _connected, _initialised = false;
        TypeofRemote _remotetype = TypeofRemote.Local;
        AsyncCallback pfnWorkerCallBack;
        Socket _uplink;
        int currX, currZ;
        public Configuration Conf;
        public bool Working;
        public Pallet map = Pallet.Yellow;

        public Bitmap Image
        {
            get
            {
                //Bitmap img = new Bitmap(_PicWidth, _PicHeight);
                //Graphics g = Graphics.FromImage(img);
                //for (int i = 0; i < _Columns.Length; i++)
                //{
                //    if (_Columns[i] == null) return img;
                //    try
                //    {
                //        g.DrawImage(_Columns[i], i, 0);
                //    }
                //    catch { }
                //}
                //g.Dispose();
                //return img;
                lock (padlock)
                {
                    if (_Images != null)
                    {
                        if (_Images[0] != null) return _Images[0];
                        else return null;
                    }
                    else return null;
                }
            }
        }
        public TimeSpan Duration
        {
            get
            {
                if (_startTime != null)
                {
                    if (_finishTime != null)
                    {
                        return _finishTime - _startTime;
                    }
                    else
                    {
                        return DateTime.Now - _startTime;
                    }
                }
                else
                {
                    return new TimeSpan();
                }
            }
        }
        public bool Completed
        {
            get
            {
                return _Completed;
            }
        }
        public TypeofRemote RemoteType
        {
            get
            {
                return _remotetype;
            }
        }
        public double StartX
        {
            get
            {
                return _XMin;
            }
        }
        public double EndX
        {
            get
            {
                return _XMax;
            }
        }
        public int StartCol
        {
            get
            {
                return _StartCol;
            }
        }
        public int EndCol
        {
            get
            {
                return _StartCol + _PicWidth;
            }
            set
            {
                int diff = (_StartCol + _PicWidth) - value;
                _XMax += _XMin - (diff * _XMax);
                _PicWidth -= diff;
                if (_remotetype == TypeofRemote.Sender)
                {
                    byte[] picwidth = BitConverter.GetBytes(_PicWidth);
                    byte[] msg = new byte[10];
                    msg[0] = 0x09;
                    msg[1] = 0x01;
                    BitConverter.GetBytes(_XMax).CopyTo(msg, 2);
                    SendMessage(msg);
                    msg = new byte[6];
                    msg[0] = 0x09;
                    msg[1] = 0x08;
                    BitConverter.GetBytes(_PicWidth).CopyTo(msg, 2);
                    SendMessage(msg);
                }
            }
        }
        public int EndLayer
        {
            get
            {
                return _PicDepth;
            }
        }
        public bool Connected
        {
            get
            {
                return _connected;
            }
        }
        public int LastCol
        {
            get
            {
                return currX + _StartCol;
            }
        }
        public int LastLayer
        {
            get
            {
                return currZ;
            }
        }
        public double InitX
        {
            get
            {
                return _InitX;
            }
            set
            {
                _InitX = value;
            }
        }
        public bool Initialised
        {
            get
            {
                return _initialised;
            }
        }

        public LyapunovGenerator()
        {
            InitWorker();
        }

        public LyapunovGenerator(Socket socket)
        {
            _remotetype = TypeofRemote.Unknown;
            _uplink = socket;
            WaitForData(_uplink);

            InitWorker();
        }

        public void Initialise(double XMin, double XMax, double YMin, double YMax, char[] Pattern, int Iterations, double InitX, int PicWidth, int PicHeight, int startCol)
        {
            _XMin = XMin;
            _XMax = XMax;
            _YMin = YMin;
            _YMax = YMax;
            _ZMin = 0;
            _ZMax = 0;
            _Pattern = Pattern;
            _Iterations = Iterations;
            _InitX = InitX;
            _PicWidth = PicWidth;
            _PicHeight = PicHeight;
            _PicDepth = 1;
            _StartCol = startCol;

            _Columns = new Bitmap[_PicWidth];
            _Images = new Bitmap[_PicDepth];

            if (_remotetype == TypeofRemote.Sender)
            {
                SendWork();
            }
            _initialised = true;
        }

        public void Initialise(Configuration conf)
        {
            Conf = conf;
            _XMin = conf.XMin;
            _XMax = conf.XMax;
            _YMin = conf.YMin;
            _YMax = conf.YMax;
            _ZMin = conf.ZMin;
            _ZMax = conf.ZMax;
            _Pattern = conf.Pattern;
            _Iterations = conf.Iterations;
            _InitX = conf.InitX;
            _PicWidth = conf.PicWidth;
            _PicHeight = conf.PicHeight;
            _PicDepth = conf.PicDepth;
            _StartCol = conf._startX;

            _Columns = new Bitmap[_PicWidth];
            _Images = new Bitmap[_PicDepth];
            if (_remotetype == TypeofRemote.Sender)
            {
                SendWork();
            }
            _initialised = true;
        }

        public void Generate()
        {
            _startTime = DateTime.Now;
                switch (_remotetype)
                {
                    case TypeofRemote.Local:
                    case TypeofRemote.Reciever:
                        if (!_Worker.IsBusy)
                        {
                            _Worker.RunWorkerAsync();
                        }
                        else
                        {
                            Stop();
                            Generate();
                        }
                        break;
                    case TypeofRemote.Sender:
                        SendMessage(new byte[] { 0x04 });
                        break;
                }
            
        }

        public bool SetRemote(TypeofRemote remotetype)
        {
            _remotetype = remotetype;
            //
            //try socket code etc
            //
            switch (_remotetype)
            {
                case TypeofRemote.Local:
                    break;
                case TypeofRemote.Sender:
                    SendMessage(new byte[] { 0x01, 0x03 });
                    break;
                case TypeofRemote.Reciever:
                    SendMessage(new byte[] { 0x01, 0x04 });
                    break;
            }
            return true;
        }

        public void Stop()
        {
            _Worker.CancelAsync();
        }

        private void SendWork()
        {
            byte[] msg = new byte[59];
            msg[0] = 0x02;
            BitConverter.GetBytes(_XMin).CopyTo(msg, 1);
            BitConverter.GetBytes(_XMax).CopyTo(msg, 9);
            BitConverter.GetBytes(_YMin).CopyTo(msg, 17);
            BitConverter.GetBytes(_YMax).CopyTo(msg, 25);
            new byte[2].CopyTo(msg, 33);
            BitConverter.GetBytes(_Iterations).CopyTo(msg, 35);
            BitConverter.GetBytes(_InitX).CopyTo(msg, 39);
            BitConverter.GetBytes(_PicWidth).CopyTo(msg, 47);
            BitConverter.GetBytes(_PicHeight).CopyTo(msg, 51);
            BitConverter.GetBytes(_StartCol).CopyTo(msg, 55);
            SendMessage(msg);
            System.Diagnostics.Debugger.Log(1, "Work Sent", "Work Sent: " + _XMin.ToString() + " " + _XMax.ToString() + " " + _YMin.ToString() + " " + _YMax.ToString() + " " + _Iterations.ToString() + " " + _InitX.ToString() + " " + _PicWidth.ToString() + " " + _PicHeight.ToString() + " " + _StartCol.ToString() + "\n");
        }

        private void SendResults(int colIndex)
        {
            byte[] bmpbytes = BmpToBytes(_Columns[colIndex - _StartCol]);
            byte[] colbytes = BitConverter.GetBytes(colIndex);
            byte[] bmplength = BitConverter.GetBytes(bmpbytes.Length);
            byte[] msg = new byte[bmpbytes.Length + 5];
            msg[0] = 0x05;
            colbytes.CopyTo(msg, 1);
            bmplength.CopyTo(msg, 3);
            bmpbytes.CopyTo(msg, 5);
            SendMessage(msg);
            //System.Diagnostics.Debugger.Log(1, "Resilts Sent", "Results Sent: " + colIndex.ToString() + " " + _Columns[colIndex - _StartCol].Width + "x" + _Columns[colIndex - _StartCol].Height);
        }

        private void SendMessage(byte[] msg)
        {
            try
            {
                _uplink.Send(msg);
            }
            catch (SocketException sex)
            {
                Stop();
                MessageBox.Show(sex.Message);
            }
        }

        private void WaitForData(Socket socket)
        {
            try
            {
                if (pfnWorkerCallBack == null)
                {
                    pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
                }
                SocketPacket theSocPkt = new SocketPacket(socket);

                socket.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnWorkerCallBack, theSocPkt);
            }
            catch (SocketException sex)
            {
                MessageBox.Show(sex.Message);
            }
        }

        private void OnDataReceived(IAsyncResult asyn)
        {
            SocketPacket socketData = (SocketPacket)asyn.AsyncState;
            try
            {
                int iRx = socketData.Socket.EndReceive(asyn);
                byte[] buff = socketData.dataBuffer;
                byte[] data = new byte[iRx - 1];
                Array.Copy(buff, 1, data, 0, iRx - 1);

                switch (buff[0])
                {
                    case 0x01:
                        Negotiate(data);
                        break;
                    case 0x02:
                        RecieveWork(data);
                        break;
                    case 0x03:
                        //Data Sent and Recieved OK
                        break;
                    case 0x04:
                        Generate();
                        break;
                    case 0x05:
                        RecieveResults(data);
                        break;
                    case 0x06:
                        GenerationComplete();
                        break;
                    case 0x07:
                        ValueRequested(data);
                        break;
                    case 0x08:

                        break;
                    case 0x09:
                        SetValue(data);
                        break;
                }

                WaitForData(socketData.Socket);
            }
            catch (ObjectDisposedException)
            {
                //System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException sex)
            {
                if (sex.ErrorCode == 10054)
                {
                    //string msg = "Client " + socketData.m_clientNumber + " Disconnected" + "\n";
                    //AppendToRichEditControl(msg);
                    //MessageBox.Show("Disconnected");
                    if (Died != null) Died(this);

                    //_workerSocketList[socketData.m_clientNumber - 1] = null;
                    //UpdateClientListControl();
                }
                else
                {
                    //MessageBox.Show(sex.Message);
                }
            }
        }


        private void Negotiate(byte[] data)
        {
            switch (data[0])
            {
                case 0x01:
                    _connected = true;
                    SendMessage(new byte[] { 0x01, 0x02 });
                    break;
                case 0x02:
                    _connected = true;
                    //MessageBox.Show("Connected");
                    break;
                case 0x03:
                    _remotetype = TypeofRemote.Reciever;
                    SendMessage(new byte[] { 0x01, 0x05 });
                    break;
                case 0x04:
                    _remotetype = TypeofRemote.Sender;
                    SendMessage(new byte[] { 0x01, 0x05 });
                    break;
                case 0x05:
                    //MessageBox.Show("Established");
                    break;
            }
        }

        private void RecieveWork(byte[] data)
        {
            Initialise(BitConverter.ToDouble(data, 0),
                BitConverter.ToDouble(data, 8),
                BitConverter.ToDouble(data, 16),
                BitConverter.ToDouble(data, 24),
                new char[] { 'a', 'b' },
                BitConverter.ToInt32(data, 34),
                BitConverter.ToDouble(data, 38),
                BitConverter.ToInt32(data, 46),
                BitConverter.ToInt32(data, 50),
                BitConverter.ToInt32(data, 54));
            SendMessage(new byte[] { 0x03 });
        }

        private void RecieveResults(byte[] data)
        {
            byte[] bmpbytes = new byte[data.Length - 2];
            Array.Copy(data, 2, bmpbytes, 0, data.Length - 2);
            Bitmap column = (Bitmap)BytesToBmp(bmpbytes);
            int x = BitConverter.ToInt16(data, 0);
            int bmplength = BitConverter.ToInt16(data, 2);
            //Bitmap column = new Bitmap(1, 400);
            //Graphics g = Graphics.FromImage(column);
            //g.DrawLine(Pens.Blue, 0, 0, 0, 400);
            if (bmpbytes.Length == bmplength)
            {
                _Columns[x - _StartCol] = column;
                UpdatePic(x);
                ColumnCompletedEventArgs args = new ColumnCompletedEventArgs();
                args.Column = column;
                args.X = x;
                if (ColumnCompleted != null) ColumnCompleted(this, args);
                SendMessage(new byte[] { 0x03 });
                System.Diagnostics.Debugger.Log(1, "Results Recieved", "Results Recieved: " + x.ToString() + " Picture: " + column.Width.ToString() + "x" + column.Height.ToString() + "\n");
            }
            //g.Dispose();
        }

        private void ValueRequested(byte[] data)
        {
            //throw new NotImplementedException();
        }

        private void GenerationComplete()
        {
            _Completed = true;
            PicCompleted(null);
        }

        private void SetValue(byte[] data)
        {
            switch (data[0])
            {
                case 0x01:
                    _XMin = BitConverter.ToDouble(data, 1);
                    break;
                case 0x08:
                    _PicWidth = BitConverter.ToInt16(data, 1);
                    break;
            }
        }

        private byte[] BmpToBytes(Bitmap bmp)
        {
            MemoryStream ms = new MemoryStream();
            // Save to memory using the Jpeg format
            bmp.Save(ms, ImageFormat.Jpeg);

            // read to end
            byte[] bmpBytes = ms.GetBuffer();
            bmp.Dispose();
            ms.Close();

            return bmpBytes;
        }
        private Image BytesToBmp(byte[] bmpBytes)
        {
            Image img;
            MemoryStream ms = new MemoryStream(bmpBytes);
            try
            {
                img = System.Drawing.Image.FromStream(ms);
            }
            catch { img = null; }
            // Do NOT close the stream!

            return img;
        }

        private void UpdatePic(int X)
        {
            //lock (padlock)
            //{
            //    Graphics g = Graphics.FromImage(_Image);
            //    g.DrawImage(_Columns[X - _StartCol], X, 0);
            //    g.Dispose();
            //}
        }

        private void InitWorker()
        {
            if (_Worker != null)
            {
                _Worker.DoWork -= new DoWorkEventHandler(_Worker_DoWork);
                _Worker.ProgressChanged -= new ProgressChangedEventHandler(_Worker_ProgressChanged);
                _Worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_Worker_RunWorkerCompleted);
                _Worker.Dispose();
            }

            _Worker = new BackgroundWorker();
            _Worker.DoWork += new DoWorkEventHandler(_Worker_DoWork);
            _Worker.ProgressChanged += new ProgressChangedEventHandler(_Worker_ProgressChanged);
            _Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_Worker_RunWorkerCompleted);
            _Worker.WorkerReportsProgress = true;
            _Worker.WorkerSupportsCancellation = true;

        }

        private void _Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _Completed = false;
                Working = true;

                for(currZ = 0; currZ < _PicDepth; currZ++)
                {
                    _Images[currZ] = new Bitmap(_PicWidth, _PicHeight);
                    Graphics g = Graphics.FromImage(_Images[currZ]);
                    bool allblue = true;
                    for (currX = 0; currX < _PicWidth; currX++)
                    {
                        _Columns[currX] = new Bitmap(1, _PicHeight);
                        for (int currY = 0; currY < _PicHeight; currY++)
                        {
                    //currX = 0;
                    //int currY = 0;

                            double a = ((_YMax - _YMin) / _PicHeight) * (currY + _InitX) + _YMin;
                            double b = ((_XMax - _XMin) / _PicWidth) * (currX + _InitX) + _XMin;
                            double c = ((_ZMax - _ZMin) / _PicDepth) * (currZ + _InitX) + _ZMin;
                            double x = _InitX;
                            //Debugger.Log(0, "", "currY: " + currY.ToString() + " a: " + a.ToString() + " b: " + b.ToString() + "\n");

                            double r=0;
                            for (int i = 0; i < _Pattern.Length; i++)
                            {
                                //r = _Pattern[i] ? a : b;
                                switch (_Pattern[i])
                                {
                                    case 'a':
                                        r = a;
                                        break;
                                    case 'b':
                                        r = b;
                                        break;
                                    case 'c':
                                        r = c;
                                        break;
                                }
                                x *= r * (1 - x);

                            }

                            double sum_of_log_of_derived = 0;
                            for (int n = 0; n < _Iterations; n++)
                            {
                                double derived = 1;
                                for (int m = 0; m < _Pattern.Length; m++)
                                {
                                    //r = _Pattern[m] ? a : b;
                                    switch (_Pattern[m])
                                    {
                                        case 'a':
                                            r = a;
                                            break;
                                        case 'b':
                                            r = b;
                                            break;
                                        case 'c':
                                            r = c;
                                            break;
                                    }
                                    x *= r * (1 - x);
                                    derived *= r * (1 - 2 * x);
                                    //if (derived < 0) Debugger.Log(0, "", "< 0");
                                }
                                double log_of_derived = Math.Log(Math.Abs(derived));
                                sum_of_log_of_derived += log_of_derived;
                              if (double.IsInfinity(derived)) break;
                                //|| log_of_derived > 5.541263545158425) break;
                                //if (n >= 50 && log_of_derived * n == sum_of_log_of_derived) break;
                            }
                                

                            double value = sum_of_log_of_derived / (_Iterations + _Pattern.Length);

                            Color pix;

                            if (value > 0)
                            {
                                // CHAOS!!!
                                //Debugger.Log(0, "", "CHAOS\n");
                                switch (map)
                                {
                                    case Pallet.Yellow:
                                        int colorIntensity = (int)(Math.Exp(-value) * 255);
                                        if (colorIntensity > 0) allblue = false;
                                        pix = Color.FromArgb(255, 0, 0, 255 - colorIntensity);
                                        break;
                                    case Pallet.Red:
                                    case Pallet.Green:
                                    case Pallet.Blue:
                                        pix = Color.FromArgb(255, 0, 0, 0);
                                        break;
                                    default:
                                        pix = Color.White;
                                        break;
                                }
                            }

                            // STABILITY
                            else if (Double.IsNegativeInfinity(value))
                            {
                                //Debugger.Log(0, "", "-INF\n");
                                pix = Color.White;
                            }
                            else if (Double.IsNaN(value))
                            {
                                //Debugger.Log(0, "", "NaN\n");
                                pix = Color.White;
                            }
                            else
                            {
                                //Debugger.Log(0, "", "OK\n");
                                allblue = false;
                                int colorIntensity = (int)(Math.Exp(value) * 255);
                                switch (map)
                                {
                                    case Pallet.Yellow:
                                        pix = Color.FromArgb(255, colorIntensity, (int)(colorIntensity * .85), 0);
                                        break;
                                    case Pallet.Red:
                                        pix = Color.FromArgb(255, 255 - colorIntensity, 0, 0);
                                        break;
                                    case Pallet.Green:
                                        pix = Color.FromArgb(255,0 , 255 - colorIntensity, 0);
                                        break;
                                    case Pallet.Blue:
                                        pix = Color.FromArgb(255, 0, 0, 255 - colorIntensity);
                                        break;
                                    default:
                                        pix = Color.White;
                                        break;
                                }
                            }
                            _Columns[currX].SetPixel(0, (_PicHeight - currY) - 1, pix);
                        }
                        if (_Worker.CancellationPending) return;
                        g.DrawImage(_Columns[currX], currX, 0);
                        _Worker.ReportProgress(currX);
                    }
                    g.Dispose();
                    if (allblue) _Worker.ReportProgress(-2, currZ);
                    else _Worker.ReportProgress(-1, currZ);
                }
                _Completed = true;
            }
            catch (Exception ex)
            {
                Stop();
                MessageBox.Show(ex.Message);
            }
        }

        private void _Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == -2)
            {
                LayerCompletedEventArgs args = new LayerCompletedEventArgs();
                args.Layer = null;
                args.Z = -1;
                if (LayerCompleted != null) LayerCompleted(this, args);
            }
            else if (e.ProgressPercentage == -1)
            {
                LayerCompletedEventArgs args = new LayerCompletedEventArgs();
                int Z = (int)e.UserState;
                args.Layer = _Images[Z];
                args.Z = Z;
                if (LayerCompleted != null) LayerCompleted(this, args);
            }
            else
            {
                int X = e.ProgressPercentage + _StartCol;
                //UpdatePic(X);
                if (_remotetype == TypeofRemote.Local)
                {
                    ColumnCompletedEventArgs args = new ColumnCompletedEventArgs();
                    args.Column = _Columns[e.ProgressPercentage];
                    args.X = X;
                    if (ColumnCompleted != null) ColumnCompleted(this, args);
                }
                else if (_remotetype == TypeofRemote.Reciever)
                {
                    SendResults(X);
                }
            }
        }

        private void _Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Working = false;
            if (_Completed)
            {
                _finishTime = DateTime.Now;
                if (_remotetype == TypeofRemote.Local)
                {
                    if (PicCompleted != null) PicCompleted(this);
                }
                else if (_remotetype == TypeofRemote.Reciever)
                {
                    SendMessage(new byte[] { 0x06 });
                }
            }
        }
    }
}
