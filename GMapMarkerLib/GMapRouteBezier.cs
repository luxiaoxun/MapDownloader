using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapMarkerLib
{
    public class GMapRouteBezier : GMapRoute
    {
        public PointLatLng endPos;
        private bool isArrow;
        public PointLatLng startPos;
        private Pen pen = new Pen(Color.Red, 1f);
        private List<PointLatLng> points;
        private StringFormat TipFormat;
        public Font TipFont { set; get; }
        public Brush TipBrush { set; get; }
        private string tipText;

        // Methods
        public GMapRouteBezier(List<PointLatLng> points, PointLatLng startpos, PointLatLng endpos, bool isArrow = true, string tip = null)
            : base(points,"")
        {
            this.isArrow = isArrow;
            TipFont = new Font("微软雅黑", 14, FontStyle.Bold, GraphicsUnit.Pixel);
            TipBrush = new SolidBrush(Color.Blue);
            this.points = points;
            this.tipText = tip;

            if (this.isArrow)
            {
                pen.EndCap = LineCap.ArrowAnchor;
                pen.CustomEndCap = new AdjustableArrowCap(5f, 10f, true);
            }
            Stroke = pen;
        }

        /// <summary>
        /// 更改显示文字
        /// </summary>
        /// <param name="tipText"></param>
        public void SetTip(string tipText)
        {
            this.tipText = tipText;
        }
     
        public override void OnRender(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            
            if (tipText != null)
            {
                System.Drawing.Size st = g.MeasureString(tipText, TipFont).ToSize();
                PointLatLng position = points[points.Count/2];
                Point p = new Point((int)LocalPoints[LocalPoints.Count/2].X, (int)LocalPoints[LocalPoints.Count/2].Y);
                g.DrawString(tipText, TipFont, TipBrush, p, TipFormat);
            }

            base.OnRender(g);
        }

        public override void Dispose()
        {
            if (pen != null)
            {
                pen.Dispose();
                pen = null;
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

            base.Dispose();
        }

    }
}
