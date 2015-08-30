using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapMarkerLib
{
    public class GMapMarkerBezier : GMapMarker
    {
        // Fields
        public PointLatLng endPos;
        private bool isArrow;
        public PointLatLng startPos;
        private Pen pen = new Pen(Color.Red, 1f);

        private string tipText;
        public Font TipFont { set; get; }
        public Brush TipBrush { set; get; }
        private StringFormat TipFormat = new StringFormat();

        private BezierCurve bc = new BezierCurve();

        // Methods
        public GMapMarkerBezier(PointLatLng startpos, PointLatLng endpos, bool isArrow = true, string tip = null)
            : base(startpos)
        {
            this.startPos = startpos;
            this.endPos = endpos;
            this.isArrow = isArrow;

            this.tipText = tip;
            TipFont = new Font("微软雅黑", 11, FontStyle.Regular, GraphicsUnit.Pixel);
            TipBrush = new SolidBrush(Color.DarkViolet);
        }

        Point[] GetBezierPoints(Point[] controlPoints, int outputSegmentCount)
        {
            List<double> ptList = new List<double>();
            foreach (var d in controlPoints)
            {
                ptList.Add(d.X);
                ptList.Add(d.Y);
            }
            double[] ptind = new double[ptList.Count];
            double[] p = new double[outputSegmentCount];
            ptList.CopyTo(ptind, 0);

            bc.Bezier2D(ptind, (outputSegmentCount) / 2, p);

            List<Point> points = new List<Point>();
            for (int i = 1; i != outputSegmentCount - 1; i += 2)
            {
                points.Add(new Point((int)p[i + 1], (int)p[i]));
            }

            return points.ToArray();
        }

        public override void OnRender(Graphics g)
        {
            base.OnRender(g);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (this.isArrow)
            {
                pen.EndCap = LineCap.ArrowAnchor;
                pen.CustomEndCap = new AdjustableArrowCap(5f, 10f, true);
            }
            GMapControl control = this.Overlay.Control;
            
            Point point = new Point(base.LocalPosition.X, base.LocalPosition.Y);
            Point point2 = new Point(base.LocalPosition.X + (((int)control.FromLatLngToLocal(this.endPos).X) - ((int)control.FromLatLngToLocal(this.startPos).X)),
                (base.LocalPosition.Y + ((int)control.FromLatLngToLocal(this.endPos).Y)) - ((int)control.FromLatLngToLocal(this.startPos).Y));
            double x = Math.Sqrt(Math.Pow((double)(point2.X - point.X), 2.0) + Math.Pow((double)(point2.Y - point.Y), 2.0)) / 4.0;
            int num2 = 1;
            if ((point2.Y - point.Y) != 0)
            {
                num2 = (point2.Y - point.Y) / Math.Abs((int)(point2.Y - point.Y));
            }
            else if ((point2.X - point.X) != 0)
            {
                num2 = (point2.X - point.X) / Math.Abs((int)(point2.X - point.X));
            }
            double num3 = ((double)(point.X - point2.X)) / ((double)(point2.Y - point.Y));
            PointF tf = new PointF((((float)(3 * point.X)) / 4f) + (((float)point2.X) / 4f), (((float)(3 * point.Y)) / 4f) + (((float)point2.Y) / 4f));
            PointF tf2 = new PointF((((float)point.X) / 4f) + (((float)(3 * point2.X)) / 4f), (((float)point.Y) / 4f) + (((float)(3 * point2.Y)) / 4f));
            double num4 = tf.Y - (num3 * tf.X);
            double num5 = tf2.Y - (num3 * tf2.X);
            double num6 = Math.Pow(num3, 2.0) + 1.0;
            double num7 = 2.0 * (((num3 * num4) - (num3 * tf.Y)) - tf.X);
            double num8 = (Math.Pow((double)tf.X, 2.0) + Math.Pow(num4 - tf.Y, 2.0)) - Math.Pow(x, 2.0);
            double num9 = (-num7 + (num2 * Math.Sqrt(Math.Pow(num7, 2.0) - ((4.0 * num6) * num8)))) / (2.0 * num6);
            double num10 = (num3 * num9) + num4;
            double num11 = Math.Pow(num3, 2.0) + 1.0;
            double num12 = 2.0 * (((num3 * num5) - (num3 * tf2.Y)) - tf2.X);
            double num13 = (Math.Pow((double)tf2.X, 2.0) + Math.Pow(num5 - tf2.Y, 2.0)) - Math.Pow(x, 2.0);
            double num14 = (-num12 + (num2 * Math.Sqrt(Math.Pow(num12, 2.0) - ((4.0 * num11) * num13)))) / (2.0 * num11);
            double num15 = (num3 * num14) + num5;
            PointF tf3 = new PointF((float)num9, (float)num10);
            PointF tf4 = new PointF((float)num14, (float)num15);
            if ((point2.Y - point.Y) == 0)
            {
                tf3 = new PointF(tf.X, tf.Y + ((float)(num2 * x)));
                tf4 = new PointF(tf2.X, tf.Y + ((float)(num2 * x)));
            }
            g.DrawBezier(pen, (PointF)point, tf3, tf4, (PointF)point2);

            List<Point> pointList = new List<Point>();
            pointList.Add(point);
            pointList.Add(new Point((int)tf3.X, (int)tf3.Y));
            pointList.Add(new Point((int)tf4.X, (int)tf4.Y));
            pointList.Add(point2);

            Point[] points = GetBezierPoints(pointList.ToArray(), 1000);

            if (tipText != null)
            {
                System.Drawing.Size st = g.MeasureString(tipText, TipFont).ToSize();
                g.DrawString(tipText, TipFont, TipBrush, points[points.Length / 2], TipFormat);
            }
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

            if (TipFormat != null)
            {
                TipFormat.Dispose();
                TipFormat = null;
            }

            base.Dispose();
        }

    }
}
