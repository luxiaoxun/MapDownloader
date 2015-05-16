using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapPolygonLib
{
    public class GMapRectangle : GMapPolygon, IDisposable
    {
        // Fields
        private double maxX;
        private double maxY;
        private double minX;
        private double minY;

        // Methods
        public GMapRectangle(PointLatLng pt1, PointLatLng pt2)
            : base(new List<PointLatLng>(), "GMapRectangle")
        {
            this.minX = (pt1.Lng < pt2.Lng) ? pt1.Lng : pt2.Lng;
            this.maxX = (pt1.Lng > pt2.Lng) ? pt1.Lng : pt2.Lng;
            this.minY = (pt1.Lat < pt2.Lat) ? pt1.Lat : pt2.Lat;
            this.maxY = (pt1.Lat > pt2.Lat) ? pt1.Lat : pt2.Lat;
            this.UpdatePoints();
        }

        public GMapRectangle(double minX, double minY, double maxX, double maxY)
            : base(new List<PointLatLng>(), "GMapRectangle")
        {
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
            this.UpdatePoints();
        }

        public static GMapRectangle FromRectlatLng(RectLatLng rectLatLng)
        {
            return new GMapRectangle(rectLatLng.Left, rectLatLng.Bottom, rectLatLng.Right, rectLatLng.Top);
        }

        public RectLatLng ToRectLatLng()
        {
            return RectLatLng.FromLTRB(this.minX, this.maxY, this.maxX, this.minY);
        }

        private void UpdatePoints()
        {
            base.Points.Clear();
            base.Points.Add(this.LeftTop);
            base.Points.Add(this.RightTop);
            base.Points.Add(this.RightBottom);
            base.Points.Add(this.LeftBottom);
        }

        // Properties
        public double Height
        {
            get
            {
                return (this.maxY - this.minY);
            }
        }

        public PointLatLng LeftBottom
        {
            get
            {
                return new PointLatLng(this.minY, this.minX);
            }
        }

        public PointLatLng LeftTop
        {
            get
            {
                return new PointLatLng(this.maxY, this.minX);
            }
        }

        public PointLatLng RightBottom
        {
            get
            {
                return new PointLatLng(this.minY, this.maxX);
            }
        }

        public PointLatLng RightTop
        {
            get
            {
                return new PointLatLng(this.maxY, this.maxX);
            }
        }

        public double Width
        {
            get
            {
                return (this.maxX - this.minX);
            }
        }
    }
}
