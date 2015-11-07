using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapCommonType
{
    public class BoundingBox : Geometry
    {
        public BoundingBox()
        {
        }

        public BoundingBox(double left, double bottom, double right, double top, string crs=null)
        {
            this.Left = left;
            this.Bottom = bottom;
            this.Right = right;
            this.Top = top;
            this.CRS = crs;
        }

        public bool Contain(BoundingBox bbox)
        {
            return (((this.LowerCorner.X <= bbox.LowerCorner.X) && (this.LowerCorner.Y <= bbox.LowerCorner.Y)) && ((this.UpperCorner.X >= bbox.UpperCorner.X) && (this.UpperCorner.Y >= bbox.UpperCorner.Y)));
        }

        public static BoundingBox FromRectLngLat(RectLatLng rect)
        {
            return new BoundingBox(rect.Left, rect.Bottom, rect.Right, rect.Top, null);
        }

        public BoundingBox Interset(BoundingBox bbox)
        {
            double left = Math.Max(this.Left, bbox.Left);
            double top = Math.Min(this.Top, bbox.Top);
            double right = Math.Min(this.Right, bbox.Right);
            double bottom = Math.Max(this.Bottom, bbox.Bottom);
            if ((left <= right) && (top >= bottom))
            {
                return new BoundingBox(left, bottom, right, top, null);
            }
            return null;
        }

        public string ToXML(string nodeName = "ows:BoundingBox ")
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("<{0} crs=\"{1}\">", nodeName, this.CRS));
            builder.Append(string.Format("<ows:LowerCorner>{0} {1}</ows:LowerCorner>", this.LowerCorner.X, this.LowerCorner.Y));
            builder.Append(string.Format("<ows:UpperCorner>{0} {1}</ows:UpperCorner>", this.UpperCorner.X, this.UpperCorner.Y));
            builder.Append(string.Format("</{0}>", nodeName, this.CRS));
            return builder.ToString();
        }

        public void Zoom(double value)
        {
            Point2D center = this.Center;
            double num = value * (this.Right - this.Left);
            double num2 = value * (this.Top - this.Bottom);
            this.Left = center.X - (num / 2.0);
            this.Top = center.Y + (num2 / 2.0);
            this.Right = center.X + (num / 2.0);
            this.Bottom = center.Y - (num2 / 2.0);
        }

        // Properties
        public double Bottom { get; set; }

        public Point2D Center
        {
            get
            {
                double x = (this.UpperCorner.X + this.LowerCorner.X) / 2.0;
                return new Point2D(x, (this.UpperCorner.Y + this.LowerCorner.Y) / 2.0);
            }
        }

        public string CRS { get; set; }

        public override GeometryType GeoType
        {
            get
            {
                return GeometryType.BBox;
            }
        }

        public double Left { get; set; }

        public Point2D LowerCorner
        {
            get
            {
                return new Point2D(this.Left, this.Bottom);
            }
            set
            {
                this.Left = value.X;
                this.Bottom = value.Y;
            }
        }

        public override List<Point2D> Points
        {
            get
            {
                return new List<Point2D> { new Point2D(this.Left, this.Bottom), new Point2D(this.Left, this.Top), new Point2D(this.Right, this.Top), new Point2D(this.Right, this.Bottom) };
            }
        }

        public double Right { get; set; }

        public double Top { get; set; }

        public Point2D UpperCorner
        {
            get
            {
                return new Point2D(this.Right, this.Top);
            }
            set
            {
                this.Right = value.X;
                this.Top = value.Y;
            }
        }
    }
}
