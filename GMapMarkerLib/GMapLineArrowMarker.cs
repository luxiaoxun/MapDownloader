using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Drawing;

namespace GMapMarkerLib
{
    public class GMapLineArrowMarker:GMapMarker
    {
        private Pen pen;

        private PointLatLng beginPoint;
        private PointLatLng endPoint;

        public GMapLineArrowMarker(PointLatLng beginPoint, PointLatLng endPoint, bool isShowArrow):base(beginPoint)
        {
            this.beginPoint = beginPoint;
            this.endPoint = endPoint;
            pen = new Pen(Color.Red);

            if (isShowArrow)
            {
                pen.StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(4, 6, true);
            }
        }

        public override void OnRender(Graphics g)
        {
            GPoint begin = this.Overlay.Control.FromLatLngToLocal(beginPoint);
            GPoint end = this.Overlay.Control.FromLatLngToLocal(endPoint);
            Point endPos = new Point((int)(LocalPosition.X + end.X-begin.X),(int)(LocalPosition.Y+end.Y-begin.Y));
            g.DrawLine(pen, LocalPosition.X, LocalPosition.Y, endPos.X, endPos.Y);
        }

        public override void Dispose()
        {
            if (pen != null)
            {
                pen.Dispose();
                pen = null;
            }
            base.Dispose();
        }
    }
}
