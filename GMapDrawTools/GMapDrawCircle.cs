using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapDrawTools
{
    public class GMapDrawCircle : GMapMarker
    {
        public Pen Stroke = new Pen(Color.Blue, 2);

        public Brush Fill = null;

        public PointLatLng CenterPoint { get; set; }

        private PointLatLng edgePoint;

        public PointLatLng EdgePoint
        {
            get
            {
                return edgePoint;
            }
            set
            {
                edgePoint = value;
            }
        }

        public GMapDrawCircle(PointLatLng centerPoint, PointLatLng edgePoint)
            : base(centerPoint)
        {
            IsHitTestVisible = true;
            this.CenterPoint = centerPoint;
            this.EdgePoint = edgePoint;
        }

        public GMapDrawCircle(PointLatLng centerPoint, PointLatLng edgePoint, Pen stroke, Brush fill)
            : base(centerPoint)
        {
            if (stroke != null)
            {
                Stroke = (Pen)stroke.Clone();
            }
            
            if (fill != null)
            {
                Fill = (Brush)fill.Clone();
            }

            IsHitTestVisible = true;
            this.CenterPoint = centerPoint;
            this.EdgePoint = edgePoint;
        }

        public override void OnRender(Graphics g)
        {
            GPoint cp = this.Overlay.Control.FromLatLngToLocal(CenterPoint);
            GPoint ep = this.Overlay.Control.FromLatLngToLocal(EdgePoint);

            double dis = (ep.X - cp.X) * (ep.X - cp.X) + (ep.Y - cp.Y) * (ep.Y - cp.Y);
            int R = (int)Math.Sqrt(dis)*2;
            if (R == 0) return;
            this.Size = new Size(R,R);
            this.Offset = new Point(-R/2, -R/2);

            Rectangle rect = new Rectangle(LocalPosition.X, LocalPosition.Y, R, R);

            if (Stroke != null)
            {
                g.DrawEllipse(Stroke, rect);
            }

            if (Fill != null)
            {
                g.FillEllipse(Fill, rect);
            }
        }

        public bool IsInside(PointLatLng p)
        {
            double radius = (edgePoint.Lat - CenterPoint.Lat) * (edgePoint.Lat - CenterPoint.Lat) +
                            (edgePoint.Lng - CenterPoint.Lng) * (edgePoint.Lng - CenterPoint.Lng);

            double dis = (p.Lat - CenterPoint.Lat) * (p.Lat - CenterPoint.Lat) +
                            (p.Lng - CenterPoint.Lng) * (p.Lng - CenterPoint.Lng);

            if (dis <= radius)
                return true;

            return false;
        }

        public override void Dispose()
        {
            if (Stroke != null)
            {
                Stroke.Dispose();
                Stroke = null;
            }

            if (Fill != null)
            {
                Fill.Dispose();
                Fill = null;
            }

            base.Dispose();
        }
    }
}
