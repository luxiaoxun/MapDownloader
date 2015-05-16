using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapPolygonLib
{
    public class GMapAreaPolygon:GMapPolygon
    {
        public GMapAreaPolygon(List<PointLatLng> points, string name) : base(points, name)
        {
            this.Stroke = new Pen(Color.Blue);
            this.Stroke.Width = 3f;
            this.Stroke.DashStyle = DashStyle.Dash;
            //this.Fill = new SolidBrush(Color.Azure);
            this.Fill = new SolidBrush(Color.FromArgb(100,240,255,255));

            this.IsHitTestVisible = true;
        }
    }
}
