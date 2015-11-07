using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapCommonType
{
    public abstract class Geometry
    {
        public List<PointLatLng> ToPointLatLngs()
        {
            if (this.Points == null)
            {
                return null;
            }
            List<PointLatLng> list = new List<PointLatLng>();
            foreach (Point2D pointd in this.Points)
            {
                list.Add(new PointLatLng(pointd.Y, pointd.X));
            }
            return list;
        }

        // Properties
        public Point2D Center
        {
            get
            {
                if ((this.Points == null) || (this.Points.Count == 0))
                {
                    return new Point2D();
                }
                double num = 0.0;
                double num2 = 0.0;
                int count = this.Points.Count;
                foreach (Point2D pointd in this.Points)
                {
                    num += pointd.X;
                    num2 += pointd.Y;
                }
                return new Point2D(num / ((double)count), num2 / ((double)count));
            }
        }

        public BoundingBox Envelope
        {
            get
            {
                BoundingBox box = new BoundingBox();
                if (this.Points != null)
                {
                    box.LowerCorner = this.Points[0];
                    box.UpperCorner = this.Points[0];
                    foreach (Point2D pointd in this.Points)
                    {
                        if (box.Left > pointd.X)
                        {
                            box.Left = pointd.X;
                        }
                        if (box.Right < pointd.X)
                        {
                            box.Right = pointd.X;
                        }
                        if (box.Bottom > pointd.Y)
                        {
                            box.Bottom = pointd.Y;
                        }
                        if (box.Top < pointd.Y)
                        {
                            box.Top = pointd.Y;
                        }
                    }
                }
                return box;
            }
        }

        public abstract GeometryType GeoType { get; }

        public abstract List<Point2D> Points { get; }

    }
}
