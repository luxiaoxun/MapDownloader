using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapMarkerLib
{
    public class GMapCircleMarker2 : GMapMarker
    {
        public Pen Stroke = new Pen(Color.FromArgb(155, Color.MidnightBlue));

        public Brush Fill = new SolidBrush(Color.FromArgb(155, Color.AliceBlue));

        public bool IsFilled { set; get; }

        private PointLatLng centerPoint;
        private PointLatLng edgePoint;

        /// <summary>
        /// Create a circle with a center point and a edge point
        /// </summary>
        /// <param name="centerPoint">circle center point</param>
        /// <param name="edgePoint">circle edge point</param>
        public GMapCircleMarker2(PointLatLng centerPoint, PointLatLng edgePoint)
            : base(centerPoint)
        {
            this.centerPoint = centerPoint;
            this.edgePoint = edgePoint;
            IsFilled = true;
        }

        public override void OnRender(Graphics g)
        {
            GPoint cp = this.Overlay.Control.FromLatLngToLocal(centerPoint);
            GPoint ep = this.Overlay.Control.FromLatLngToLocal(edgePoint);

            double dis = (ep.X-cp.X)* (ep.X-cp.X) + (ep.Y-cp.Y)*(ep.Y-cp.Y);
            int R = (int)Math.Sqrt(dis)*2;

            this.Size = new Size(R, R);
            this.Offset = new Point(-R / 2, -R / 2);

            Rectangle rect = new System.Drawing.Rectangle(LocalPosition.X, LocalPosition.Y, R, R);

            if (IsFilled)
            {
                g.FillEllipse(Fill, rect);
            }
            g.DrawEllipse(Stroke, rect);
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
