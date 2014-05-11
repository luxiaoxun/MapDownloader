using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapTools
{
    public class GMapDrawingCircle : GMapMarker
    {
        public Pen Stroke = new Pen(Color.Blue, 2);

        public Brush Fill = new SolidBrush(Color.FromArgb(50, Color.Red));

        public PointLatLng CenterPoint { get; set; }

        public PointLatLng EdgePoint { get; set; }

        public GMapDrawingCircle(PointLatLng centerPoint, PointLatLng edgePoint) : base(centerPoint)
        {
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

            this.Size = new Size(R, R);
            this.Offset = new Point(-R / 2, -R / 2);
            g.DrawEllipse(Stroke, new System.Drawing.Rectangle(LocalPosition.X, LocalPosition.Y, R, R));
            g.FillEllipse(Fill, new System.Drawing.Rectangle(LocalPosition.X, LocalPosition.Y, R, R));
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
