using System;
using System.Collections.Generic;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GMapMarkerLib
{
    public class GMapMarkerScopePie : GMapMarker
    {
        /// <summary>
        /// 边缘画笔
        /// </summary>
        public Pen Stroke = new Pen(Color.FromArgb(0, Color.Transparent));

        // 填充颜色
        public Color FillColor = Color.FromArgb(88, Color.DarkOrange);

        // 填充背景
        private PathGradientBrush pthGrBrush = null;

        // 渐变路径
        GraphicsPath path;
        
        /// <summary>
        /// 半径，单位为米
        /// </summary>
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

        private int startAngle;
        public int StartAngle
        {
            get
            {
                return this.startAngle;
            }
            set
            {
                this.startAngle = value - 90;
            }
        }
        
        private int sweepAngle;
        public int SweepAngle
        {
            get
            {
                return this.sweepAngle;
            }
            set
            {
                this.sweepAngle = value;
            }
        }

        public GMapMarkerScopePie(PointLatLng pos, int startangle, int sweepangle, int r = 300)
            : base(pos)
        {
            this.radius = r;
            this.startAngle = startangle - 90;
            this.sweepAngle = sweepangle;
        }

        /// <summary>
        /// Override Render function
        /// </summary>
        /// <param name="g"></param>
        public override void OnRender(Graphics g)
        {
            // 将距离转换成像素长度
            float r = (float)((radius) / Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, Position.Lat));
            // 外接圆
            RectangleF rect = new RectangleF((float)LocalPosition.X - r, (float)LocalPosition.Y - r, (2 * r), (2 * r));
            // 渐变
            path = new GraphicsPath();
            path.AddEllipse(rect);
            pthGrBrush = new PathGradientBrush(path);
            pthGrBrush.CenterColor = Color.FromArgb(86, Color.DarkOrange);
            Color[] colors = { Color.FromArgb(0, Color.Transparent) };
            pthGrBrush.SurroundColors = colors;
            // 绘制区域
            g.DrawPie(Stroke, rect, this.startAngle, this.sweepAngle);
            g.FillPie(pthGrBrush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            if (Stroke != null)
            {
                Stroke.Dispose(); 
                Stroke = null;
            }

            if (pthGrBrush != null)
            {
                pthGrBrush.Dispose();
                pthGrBrush = null;
            }

            if (path != null)
            {
                path.Dispose();
                path = null;
            }

            base.Dispose();
        }
    }
}
