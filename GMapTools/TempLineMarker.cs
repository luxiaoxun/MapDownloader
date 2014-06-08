using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapTools
{
    public class TempLineMarker:GMapMarker
    {
        private Pen LinePen = new Pen(Color.Red, 2);

        public PointLatLng StartPoint { set; get; }
        public PointLatLng EndPoint { set; get; }

        public TempLineMarker(PointLatLng start, PointLatLng end) : base(start)
        {
            StartPoint = start;
            EndPoint = end;
        }

        public override void OnRender(Graphics g)
        {
            GPoint ep = this.Overlay.Control.FromLatLngToLocal(EndPoint);
            GPoint sp = this.Overlay.Control.FromLatLngToLocal(StartPoint);

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
