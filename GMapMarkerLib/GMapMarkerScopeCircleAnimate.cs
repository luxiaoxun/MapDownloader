using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapMarkerLib
{
    public class GMapMarkerScopeCircleAnimate : GMapAnimateMarker
    {
        private int radius;

        private int maxradius;
        
        private Rectangle rect;
        
        private int r;

        public GMapMarkerScopeCircleAnimate(GMapControl map, PointLatLng pos, int r = 300)
            : base(map, pos)
        {
            this.r = r;
            int zoom = (int)map.Zoom;
            this.radius = (int)(r / map.MapProvider.Projection.GetGroundResolution(zoom, Position.Lat)) / 2; 
        }

        public override void OnRender(Graphics g)
        {
            int zoom = (int)Overlay.Control.Zoom;
            this.maxradius = (int)(r / Overlay.Control.MapProvider.Projection.GetGroundResolution(zoom, Position.Lat));

            if (maxradius == 0)
            {
                maxradius = 5;
            }
            if (radius == 0)
            {
                radius = 1;
            }
            this.rect = new Rectangle(LocalPosition.X - radius, LocalPosition.Y - radius, 2 * radius, 2 * radius);

            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddEllipse(this.rect);
            PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);
            Color[] colorArray = new Color[] { Color.FromArgb(0, 0, 255, 0) };
            pathGradientBrush.SurroundColors = colorArray;
            float[] singleArray = new float[] { 0f, 0.6f, 0.1f, 0.1f, 0.5f, 0f, 0f, 0f, 0f, 0f };
            float[] singleArray1 = new float[] { 0f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f };
            Blend blend = new Blend()
            {
                Factors = singleArray,
                Positions = singleArray1
            };
            pathGradientBrush.Blend = blend;
            pathGradientBrush.CenterPoint = new PointF((float)LocalPosition.X, (float)LocalPosition.Y);
            pathGradientBrush.CenterColor = Color.FromArgb(255, 255, 0, 0);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillEllipse(pathGradientBrush, this.rect);

            this.radius += 2;
            this.rect = new Rectangle(LocalPosition.X - this.radius, LocalPosition.Y - this.radius, 2 * this.radius, 2 * this.radius);
         
            if (this.radius > this.maxradius)
            {
                this.radius = this.maxradius/5;
            }
            GPoint local = Overlay.Control.FromLatLngToLocal(base.Position);
            Rectangle rectangle = new Rectangle((int)local.X - this.maxradius, (int)local.Y - this.maxradius, 2 * this.maxradius, 2 * this.maxradius);
            base.RefreshAnimateMarkerRegion(rectangle);

            graphicsPath.Dispose();
            pathGradientBrush.Dispose();
        }
    }
}
