using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMapCommonType
{
    public class Point2D : Geometry
    {
        // Fields
        public static readonly Point2D Empty = new Point2D();
        public double X;
        public double Y;

        public Point2D()
        {
        }

        public Point2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public void SwapXY()
        {
            double x = this.X;
            this.X = this.Y;
            this.Y = x;
        }

        public override string ToString()
        {
            return string.Format("{{X:{0},Y:{1}}}", this.X, this.Y);
        }

        // Properties
        public override GeometryType GeoType
        {
            get
            {
                return GeometryType.Point;
            }
        }

        public override List<Point2D> Points
        {
            get
            {
                return new List<Point2D> { new Point2D(this.X, this.Y) };
            }
        }
    }


}
