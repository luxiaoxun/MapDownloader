using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Drawing;

namespace MapDownloader
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

        //public List<RouteDataColor> RouteDataColorList { set; get; }

        //private Brush GetFillBrush(double value)
        //{
        //    if (value == 0)
        //    {
        //        return null;
        //    }
        //    foreach (var colorData in RouteDataColorList)
        //    {
        //        if (value >= colorData.ValueLess && value < colorData.ValueBig)
        //        {
        //            return new SolidBrush(colorData.FillBrushColor);
        //        }
        //    }

        //    return DefaultFillBrush;
        //}

        public override void OnRender(Graphics g)
        {
            //if (CheckedKey != null && this.Data.SignalDataDictionary.ContainsKey(CheckedKey))
            //{
                
            //}

            int R = 6;
            //Brush fillBrush = GetFillBrush(this.Data.SignalDataDictionary[CheckedKey]);
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
