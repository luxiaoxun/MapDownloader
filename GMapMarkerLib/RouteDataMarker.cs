using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Drawing;

namespace GMapMarkerLib
{
    public class RouteDataMarker : GMapMarker
    {
        public RouteMapData Data { set; get; }

        public string CheckedKey { set; get; }

        private PointLatLng center;

        public Pen DrawPen = new Pen(Color.FromArgb(155, Color.MidnightBlue));
        public Brush DefaultFillBrush = new SolidBrush(Color.FromArgb(255, Color.Blue));

        public RouteDataMarker(PointLatLng p, RouteMapData data)
            : base(p)
        {
            this.center = p;
            this.Data = data;
        }

        public override void OnRender(Graphics g)
        {
            int R = 6;
            Brush fillBrush = DefaultFillBrush;
            if (fillBrush != null)
            {
                g.DrawEllipse(DrawPen, LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R);
                g.FillEllipse(fillBrush, LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R);
            }
        }

        public override void Dispose()
        {
            if (DrawPen != null)
            {
                DrawPen.Dispose();
                DrawPen = null;
            }

            if (DefaultFillBrush != null)
            {
                DefaultFillBrush.Dispose();
                DefaultFillBrush = null;
            }
            base.Dispose();
        }
    }
}
