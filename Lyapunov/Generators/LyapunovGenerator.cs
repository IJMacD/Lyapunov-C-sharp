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
    class LyapunovGenerator : Generator
    {
        public enum Pallet { Yellow, Red, Green, Blue };

        BackgroundWorker _Worker;
        public static object padlock = new object();

        
        Bitmap[] _Columns;
        //Bitmap[] _Layers;
        Bitmap[] _Images;
        bool _connected, _initialised = false;
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
        public bool Connected
        {
            get
            {
                return _connected;
            }
        }

        public int LastCol { get { return currX + _StartCol; } }
        public int LastLayer { get { return currZ; } }
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

        public override void Initialise(Configuration conf)
        {
            base.Initialise(conf);


            _Columns = new Bitmap[_PicWidth];
            _Images = new Bitmap[_PicDepth];
        }


        public override void Generate()
        {
            _startTime = DateTime.Now;
            if (!_Worker.IsBusy)
            {
                _Worker.RunWorkerAsync();
            }
            else
            {
                Stop();
                Generate();
            }
            
        }
        
        public override void Stop()
        {
            _Worker.CancelAsync();
        }



        private void GenerationComplete()
        {
            _complete = true;
            onCompleted(this);
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
                _complete = false;
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
                _complete = true;
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
                onLayerCompleted(this, args);
            }
            else if (e.ProgressPercentage == -1)
            {
                LayerCompletedEventArgs args = new LayerCompletedEventArgs();
                int Z = (int)e.UserState;
                args.Layer = _Images[Z];
                args.Z = Z;
                onLayerCompleted(this, args);
            }
            else
            {
                int X = e.ProgressPercentage + _StartCol;
                //UpdatePic(X);
                    ProgressedEventArgs args = new ProgressedEventArgs();
                    args.Image = _Columns[e.ProgressPercentage];
                    args.Progress = (int)((currX / _PicWidth)*100);
                    args.X = X;
                    args.Y = 0;
                    args.Z = currZ;
                    onProgressed(this, args);
            }
        }

        private void _Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Working = false;
            if (_complete)
            {
                _finishTime = DateTime.Now;
                onCompleted(this);
            }
        }
    }
}
