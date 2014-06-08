using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapTools
{
    public class LineMarker:GMapMarker
    {
        private Pen LinePen = new Pen(Color.Red, 2);

        public PointLatLng startPoint;
        public PointLatLng endPoint;

        public LineMarker(PointLatLng start, PointLatLng end) : base(start)
        {
            startPoint = start;
            endPoint = end;
        }

        public override void OnRender(Graphics g)
        {
            GPoint ep = this.Overlay.Control.FromLatLngToLocal(endPoint);
            GPoint sp = this.Overlay.Control.FromLatLngToLocal(startPoint);

            Point end = new Point((int)(LocalPosition.X + ep.X - sp.X), (int)(LocalPosition.Y + ep.Y - sp.Y));
            g.DrawLine(LinePen, LocalPosition.X, LocalPosition.Y, end.X, end.Y);
        }

        public override void Dispose()
        {
            if (LinePen != null)
            {
                LinePen.Dispose();
                LinePen = null;
            }

            base.Dispose();
        }
    }
}
