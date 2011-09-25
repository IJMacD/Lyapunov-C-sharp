using System;
using System.Collections.Generic;
using System.Text;

namespace Lyapunov
{
    class Configuration
    {
        double _XMin, _XMax, _YMin, _YMax, _ZMin, _ZMax, _InitX, _InitXfinish, _InitXstep;
        char[] _Pattern;
        int _Iterations, _PicHeight, _PicWidth, _PicDepth;
        public int _x, _y, _z, _startX;
        public string _path;

        int _Divisions;

        public double XMin
        {
            get { return _XMin; }
            set { _XMin = value; }
        }
        public double XMax
        {
            get { return _XMax; }
            set { _XMax = value; }
        }
        public double YMin
        {
            get { return _YMin; }
            set { _YMin = value; }
        }
        public double YMax
        {
            get { return _YMax; }
            set { _YMax = value; }
        }
        public double ZMin
        {
            get { return _ZMin; }
            set { _ZMin = value; }
        }
        public double ZMax
        {
            get { return _ZMax; }
            set { _ZMax = value; }
        }
        public double InitX
        {
            get { return _InitX; }
            set { _InitX = value; }
        }
        public char[] Pattern
        {
            get { return _Pattern; }
            set { _Pattern = value; }
        }
        public int Iterations
        {
            get { return _Iterations; }
            set { _Iterations = value; }
        }
        public int PicHeight
        {
            get { return _PicHeight; }
            set { _PicHeight = value; }
        }
        public int PicWidth
        {
            get { return _PicWidth; }
            set { _PicWidth = value; }
        }
        public int PicDepth
        {
            get { return _PicDepth; }
            set { _PicDepth = value; }
        }
        public int StartCol
        {
            get { return _startX; }
        }

        public int Divisions
        {
            get { return _Divisions; }
            set { _Divisions = value; }
        }

        public Configuration(double xmin, double xmax, double ymin, double ymax, char[] pattern, int iterations, double initx, int picwidth, int picheight, int x, int y, int z, string path)
        {
            _XMin = xmin;
            _XMax = xmax;
            _YMin = ymin;
            _YMax = ymax;
            _Pattern = pattern;
            _Iterations = iterations;
            _InitX = initx;
            _PicWidth = picwidth;
            _PicHeight = picheight;
            _PicDepth = 1;
            _x = x;
            _y = y;
            _z = z;
            _path = path;
        }

        public Configuration(double xmin, double xmax, double ymin, double ymax, char[] pattern, int iterations, double initx, int picwidth, int picheight, int startX)
        {
            _XMin = xmin;
            _XMax = xmax;
            _YMin = ymin;
            _YMax = ymax;
            _Pattern = pattern;
            _Iterations = iterations;
            _InitX = initx;
            _PicWidth = picwidth;
            _PicHeight = picheight;
            _PicDepth = 1;
            _startX = startX;
        }

        public Configuration(double xmin, double xmax, double ymin, double ymax, char[] pattern, int iterations, double initx, int picwidth, int picheight)
        {
            _XMin = xmin;
            _XMax = xmax;
            _YMin = ymin;
            _YMax = ymax;
            _Pattern = pattern;
            _Iterations = iterations;
            _InitX = initx;
            _PicWidth = picwidth;
            _PicHeight = picheight;
            _PicDepth = 1;
        }
        public Configuration(double xmin, double xmax, double ymin, double ymax, double zmin, double zmax, char[] pattern, int iterations, double initx, int picwidth, int picheight, int picdepth)
        {
            _XMin = xmin;
            _XMax = xmax;
            _YMin = ymin;
            _YMax = ymax;
            _ZMin = zmin;
            _ZMax = zmax;
            _Pattern = pattern;
            _Iterations = iterations;
            _InitX = initx;
            _PicWidth = picwidth;
            _PicHeight = picheight;
            _PicDepth = picdepth;
        }

        public void SetInitX(double finish, double step)
        {
            _InitXfinish = finish;
            _InitXstep = step;
        }
    }
}
