using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Drawing;

namespace GMapMarkerLib
{
    public class GMapTipMarker : GMapMarker
    {
        private Image image;

        public bool IsHighlight = true;
        public Pen HighlightPen { set; get; }

        public string TipText { set; get; }

        public Font TipFont = new Font(FontFamily.GenericSansSerif, 14, FontStyle.Regular, GraphicsUnit.Pixel);
        public Brush TipBrush = new SolidBrush(Color.Navy);
        public StringFormat TipStringFormat = new StringFormat();

        public GMapTipMarker(GMap.NET.PointLatLng p, Image image, string tipText)
            : base(p)
        {
            Size = new System.Drawing.Size(image.Width, image.Height);
            Offset = new System.Drawing.Point(-Size.Width / 2, -Size.Height / 2);
            this.image = image;
            HighlightPen = new System.Drawing.Pen(Brushes.Red,2);
            TipText = tipText;
        }

        public override void OnRender(Graphics g)
        {
            if (image == null) return;

            Rectangle rect = new Rectangle(LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height);
            g.DrawImage(image, rect);

            if (IsMouseOver && IsHighlight)
            {
                g.DrawRectangle(HighlightPen, rect);
            }

            System.Drawing.Size st = g.MeasureString(TipText, TipFont).ToSize();
            Point point = new Point(LocalPosition.X + Size.Width / 2 - st.Width/2, LocalPosition.Y + Size.Height);
            g.DrawString(TipText, TipFont, TipBrush, point, TipStringFormat);
        }

        public override void Dispose()
        {
            if (HighlightPen != null)
            {
                HighlightPen.Dispose();
                HighlightPen = null;
            }

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
