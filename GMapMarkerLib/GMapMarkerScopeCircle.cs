using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapMarkerLib
{
    public class GMapMarkerScopeCircle : GMapMarker
    {
        public Pen Stroke = new Pen(Color.FromArgb(0, Color.Transparent));

        public Color FillColor = Color.FromArgb(88, Color.DarkOrange);

        private PathGradientBrush pthGrBrush = null;

        private GraphicsPath path;

        private int radius;

        public int Radius
        {
            get
            {
                return this.radius;
            }
            set
            {
                this.radius = value;
            }
        }

        public GMapMarkerScopeCircle(PointLatLng pos, int r = 300)
            : base(pos)
        {
            this.radius = r;
        }

        public override void Dispose()
        {
            if (this.Stroke != null)
            {
                this.Stroke.Dispose();
                this.Stroke = null;
            }
            if (this.pthGrBrush != null)
            {
                this.pthGrBrush.Dispose();
                this.pthGrBrush = null;
            }
            if (this.path != null)
            {
                this.path.Dispose();
                this.path = null;
            }
            base.Dispose();
        }

        public override void OnRender(Graphics g)
        {
            double num = (double)this.radius;
            PureProjection projection = base.Overlay.Control.MapProvider.Projection;
            int zoom = checked((int)base.Overlay.Control.Zoom);
            PointLatLng position = base.Position;
            float r = (float)(num / projection.GetGroundResolution(zoom, position.Lat));
            Point localPosition = base.LocalPosition;
            float x = (float)localPosition.X - r;
            localPosition = base.LocalPosition;
            RectangleF rect = new RectangleF(x, (float)localPosition.Y - r, 2f * r, 2f * r);
            this.path = new GraphicsPath();
            this.path.AddEllipse(rect);
            this.pthGrBrush = new PathGradientBrush(this.path)
            {
                CenterColor = Color.FromArgb(86, Color.DarkOrange)
            };
            Color[] colorArray = new Color[] { Color.FromArgb(0, Color.Transparent) };
            this.pthGrBrush.SurroundColors = colorArray;
            g.FillEllipse(this.pthGrBrush, rect);
            g.DrawEllipse(this.Stroke, rect);
        }
    }
}
