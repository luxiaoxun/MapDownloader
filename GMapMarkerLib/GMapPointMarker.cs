using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Drawing;

namespace GMapMarkerLib
{
    public class GMapPointMarker:GMapMarker
    {
        private PointLatLng center;

        public Pen DrawPen = new Pen(Color.FromArgb(155, Color.MidnightBlue));
        public Brush FillBrush = new SolidBrush(Color.FromArgb(255, Color.Blue));

        public GMapPointMarker(PointLatLng p)
            : base(p)
        {
            this.center = p;
        }

        public override void OnRender(Graphics g)
        {
            int R = 6;
            g.DrawEllipse(DrawPen, LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R);
            g.FillEllipse(FillBrush, LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R);
        }

        public override void Dispose()
        {
            if (DrawPen != null)
            {
                DrawPen.Dispose();
                DrawPen = null;
            }

            if (FillBrush != null)
            {
                FillBrush.Dispose();
                FillBrush = null;
            }
            base.Dispose();
        }

    }
}
