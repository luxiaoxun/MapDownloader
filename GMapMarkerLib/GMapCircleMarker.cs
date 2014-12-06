using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapMarkerLib
{
    public class GMapCircleMarker : GMapMarker
    {
        /// <summary>
        /// In Meters
        /// </summary>
        public int Radius;

        public Pen Stroke = new Pen(Color.FromArgb(155, Color.MidnightBlue));

        public Brush Fill = new SolidBrush(Color.FromArgb(155, Color.AliceBlue));

        public bool IsFilled { set; get; }

        /// <summary>
        /// Create a circle with MapProvider.Projection.GetGroundResolution
        /// </summary>
        /// <param name="p">circle center</param>
        /// <param name="radius">circle radius in meters</param>
        public GMapCircleMarker(PointLatLng p, int radius)
            : base(p)
        {
            //Radius = 100; // 100m
            Radius = radius;
            IsHitTestVisible = true;
            IsFilled = true;
        }

        public override void OnRender(Graphics g)
        {
            int R = (int)((Radius) / Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, Position.Lat)) * 2;
            if(R <= 0) return;

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
