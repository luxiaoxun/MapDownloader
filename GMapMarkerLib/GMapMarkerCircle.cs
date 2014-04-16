using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapMarkerLib
{
    public class GMapMarkerCircle : GMapMarker
    {
        /// <summary>
        /// In Meters
        /// </summary>
        public int Radius;

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

        /// <summary>
        /// Create a circle with MapProvider.Projection.GetGroundResolution
        /// </summary>
        /// <param name="p">circle center</param>
        /// <param name="radius">circle radius in meters</param>
        public GMapMarkerCircle(PointLatLng p, int radius)
            : base(p)
        {
            //Radius = 100; // 100m
            Radius = radius;
            IsHitTestVisible = false;
        }

        public override void OnRender(Graphics g)
        {
            int R = (int)((Radius) / Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, Position.Lat)) * 2;

            if (IsFilled)
            {
                g.FillEllipse(Fill, new System.Drawing.Rectangle(LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R));
            }
            g.DrawEllipse(Stroke, new System.Drawing.Rectangle(LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R));
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
