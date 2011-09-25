using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Lyapunov
{
    abstract class Generator
    {
        protected double _XMin, _XMax, _YMin, _YMax, _ZMin, _ZMax, _InitX;
        protected char[] _Pattern;
        protected int _Iterations, _PicHeight, _PicWidth, _PicDepth, _StartCol;
        protected DateTime _startTime, _finishTime;
        protected bool _complete = false;
        protected Configuration _conf;

        public Configuration Conf { get { return _conf; } }
        public double MinX { get { return _XMin; } }
        public double MaxX { get { return _XMax; } }
        public double MinY { get { return _YMin; } }
        public double MaxY { get { return _YMax; } }
        public double MinZ { get { return _ZMin; } }
        public double MaxZ { get { return _ZMax; } }
        public int PicHeight { get { return _PicHeight; } }
        public int PicWidth { get { return _PicWidth; } }
        public int PicDepth { get { return _PicDepth; } }
        public int StartCol { get { return _StartCol; } }
        public int EndCol {
            get { return _StartCol + _PicWidth; }
            set
            {
                int diff = (_StartCol + _PicWidth) - value;
                _XMax += _XMin - (diff * _XMax);
                _PicWidth -= diff;
            }
        }
        public int EndLayer { get { return _PicDepth; } }
        public double InitX
        {
            get { return _InitX; }
            set { _InitX = value; }
        }
        public int Iterations { get { return _Iterations; } }
        public char[] Pattern { get { return _Pattern; } }

        public bool IsComplete
        {
            get { return _complete; }
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

        public class ProgressedEventArgs : EventArgs
        {
            public int X;
            public int Y;
            public int Z;
            public Bitmap Image;
            public int Progress;
        }
        public class LayerCompletedEventArgs : EventArgs
        {
            public int Z;
            public Bitmap Layer;
        }
        public event EventHandler<ProgressedEventArgs> Progressed;
        public event EventHandler<LayerCompletedEventArgs> LayerCompleted;
        public event EventHandler Completed;
        public event EventHandler Died;

        protected virtual void onProgressed(object sender, ProgressedEventArgs e)
        {
            EventHandler<ProgressedEventArgs> handler = Progressed;
            if (handler != null)
                handler(sender, e);
        }
        protected virtual void onLayerCompleted(object sender, LayerCompletedEventArgs e)
        {
            EventHandler<LayerCompletedEventArgs> handler = LayerCompleted;
            if (handler != null)
                handler(sender, e);
        }
        protected virtual void onCompleted(object sender)
        {
            EventHandler handler = Completed;
            if (handler != null)
                handler(sender, null);
        }
        protected virtual void onDeath(object sender)
        {
            EventHandler handler = Died;
            if (handler != null)
                handler(sender, null);
        }



        public virtual void Initialise(Configuration conf)
        {
            _conf = conf;
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
            _StartCol = conf.StartCol;
        }

        public abstract void Generate();
        public abstract void Stop();
    }
}
