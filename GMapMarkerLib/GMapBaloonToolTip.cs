using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.ToolTips;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GMapMarkerLib
{
    public class GMapBaloonToolTip : GMapToolTip
    {
        public float Radius = 10f;

        public static readonly new Pen DefaultStroke = new Pen(Color.FromArgb(140, Color.Navy));

        static GMapBaloonToolTip()
        {
            DefaultStroke.Width = 3;
            DefaultStroke.LineJoin = LineJoin.Round;
            DefaultStroke.StartCap = LineCap.RoundAnchor;
        }

        public GMapBaloonToolTip(GMapMarker marker)
            : base(marker)
        {
            Stroke = DefaultStroke;
            Fill = Brushes.FloralWhite;
        }

        public override void OnRender(Graphics g)
        {
            System.Drawing.Size st = g.MeasureString(Marker.ToolTipText, Font).ToSize();
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y - st.Height, st.Width + TextPadding.Width, st.Height + TextPadding.Height);
            rect.Offset(Offset.X, Offset.Y);

            using (GraphicsPath objGP = new GraphicsPath())
            {
                objGP.AddLine(rect.X + 2 * Radius, rect.Y + rect.Height, rect.X + Radius, rect.Y + rect.Height + Radius);
                objGP.AddLine(rect.X + Radius, rect.Y + rect.Height + Radius, rect.X + Radius, rect.Y + rect.Height);

                objGP.AddArc(rect.X, rect.Y + rect.Height - (Radius * 2), Radius * 2, Radius * 2, 90, 90);
                objGP.AddLine(rect.X, rect.Y + rect.Height - (Radius * 2), rect.X, rect.Y + Radius);
                objGP.AddArc(rect.X, rect.Y, Radius * 2, Radius * 2, 180, 90);
                objGP.AddLine(rect.X + Radius, rect.Y, rect.X + rect.Width - (Radius * 2), rect.Y);
                objGP.AddArc(rect.X + rect.Width - (Radius * 2), rect.Y, Radius * 2, Radius * 2, 270, 90);
                objGP.AddLine(rect.X + rect.Width, rect.Y + Radius, rect.X + rect.Width, rect.Y + rect.Height - (Radius * 2));
                objGP.AddArc(rect.X + rect.Width - (Radius * 2), rect.Y + rect.Height - (Radius * 2), Radius * 2, Radius * 2, 0, 90); // Corner

                objGP.CloseFigure();

                g.FillPath(Fill, objGP);
                g.DrawPath(Stroke, objGP);
            }
            g.DrawString(Marker.ToolTipText, Font, Foreground, rect, Format);
        }
    }
}
