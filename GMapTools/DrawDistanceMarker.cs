using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapTools
{
    public class DrawDistanceMarker:GMapMarker
    {
        public DrawDistanceMarker(PointLatLng p, string tipText) : base(p)
        {

            this.ToolTip = new DrawDistanceMarkerToolTip(this);
            this.ToolTipMode = MarkerTooltipMode.Always;
            this.ToolTipText = tipText;
        }

        public Pen DrawPen = new Pen(Color.FromArgb(155, Color.MidnightBlue));
        public Brush FillBrush = new SolidBrush(Color.FromArgb(255, Color.Red));

        public override void OnRender(Graphics g)
        {
            int R = 4;
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
