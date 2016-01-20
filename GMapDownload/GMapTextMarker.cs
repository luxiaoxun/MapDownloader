using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Drawing;

namespace GMapDownload
{
    public class GMapTextMarker:GMapMarker
    {

        public string TipText { set; get; }

        public Font TipFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Regular, GraphicsUnit.Pixel);
        public Brush TipBrush = new SolidBrush(Color.Navy);
        public StringFormat TipStringFormat = new StringFormat();

        public GMapTextMarker(GMap.NET.PointLatLng p, string tipText)
            : base(p)
        {
            
            //Size = new System.Drawing.Size(image.Width, image.Height);
            //Offset = new System.Drawing.Point(-st/2, 0);

            TipText = tipText;
        }

        public override void OnRender(Graphics g)
        {
            System.Drawing.Size st = g.MeasureString(TipText, TipFont).ToSize();
            Point point = new Point(LocalPosition.X - st.Width/2, LocalPosition.Y);
            g.DrawString(TipText, TipFont, TipBrush, point, TipStringFormat);
        }

        public override void Dispose()
        {
            if (TipFont != null)
            {
                TipFont.Dispose();
                TipFont = null;
            }

            if (TipBrush != null)
            {
                TipBrush.Dispose();
                TipBrush = null;
            }

            if (TipStringFormat != null)
            {
                TipStringFormat.Dispose();
                TipStringFormat = null;
            }

            base.Dispose();
        }
    }
}
