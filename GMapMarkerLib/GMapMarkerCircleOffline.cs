using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapMarkerLib
{
    public class GMapMarkerCircleOffline : GMapMarker
    {
        /// <summary>
        /// In Meters
        /// </summary>
        public double Radius;

        /// <summary>
        /// specifies how the outline is painted
        /// </summary>
        public Pen Stroke = new Pen(Color.FromArgb(155, Color.MidnightBlue));

        /// <summary>
        /// background color
        /// </summary>
        public Brush Fill = new SolidBrush(Color.FromArgb(155, Color.AliceBlue));

        /// <summary>
        /// is filled
        /// </summary>
        public bool IsFilled = true;

        private PointLatLng center;
        private PointLatLng edgePoint;

        /// <summary>
        /// Create a circle with a radius
        /// </summary>
        /// <param name="p">circle center</param>
        /// <param name="radius">circle radius</param>
        public GMapMarkerCircleOffline(PointLatLng p, double radius)
            : base(p)
        {
            Radius = radius;
            IsHitTestVisible = true;

            center = p;
            edgePoint = new PointLatLng(p.Lat, p.Lng + radius);
        }

        public override void OnRender(Graphics g)
        {
            //int R = (int)((Radius) / Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, Position.Lat)) * 2;
            GPoint cp = this.Overlay.Control.FromLatLngToLocal(center);
            GPoint ep = this.Overlay.Control.FromLatLngToLocal(edgePoint);

            double dis = (ep.X-cp.X)* (ep.X-cp.X) + (ep.Y-cp.Y)*(ep.Y-cp.Y);
            int R = (int)Math.Sqrt(dis)*2;

            this.Size = new Size(R, R);
            this.Offset = new Point(-R / 2, -R / 2);

            if (IsFilled)
            {
                g.FillEllipse(Fill, new System.Drawing.Rectangle(LocalPosition.X, LocalPosition.Y, R, R));
            }
            g.DrawEllipse(Stroke, new System.Drawing.Rectangle(LocalPosition.X, LocalPosition.Y, R, R));
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
