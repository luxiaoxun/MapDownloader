using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using GMap.NET.WindowsForms;
using GMap.NET;

namespace GMapMarkerLib
{
    public class BezierCurve
    {
        private double[] FactorialLookup;

        public BezierCurve()
        {
            CreateFactorialTable();
        }

        // just check if n is appropriate, then return the result
        private double factorial(int n)
        {
            if (n < 0) { throw new Exception("n is less than 0"); }
            if (n > 32) { throw new Exception("n is greater than 32"); }

            return FactorialLookup[n]; /* returns the value n! as a SUMORealing point number */
        }

        // create lookup table for fast factorial calculation
        private void CreateFactorialTable()
        {
            // fill untill n=32. The rest is too high to represent
            double[] a = new double[33];
            a[0] = 1.0;
            a[1] = 1.0;
            a[2] = 2.0;
            a[3] = 6.0;
            a[4] = 24.0;
            a[5] = 120.0;
            a[6] = 720.0;
            a[7] = 5040.0;
            a[8] = 40320.0;
            a[9] = 362880.0;
            a[10] = 3628800.0;
            a[11] = 39916800.0;
            a[12] = 479001600.0;
            a[13] = 6227020800.0;
            a[14] = 87178291200.0;
            a[15] = 1307674368000.0;
            a[16] = 20922789888000.0;
            a[17] = 355687428096000.0;
            a[18] = 6402373705728000.0;
            a[19] = 121645100408832000.0;
            a[20] = 2432902008176640000.0;
            a[21] = 51090942171709440000.0;
            a[22] = 1124000727777607680000.0;
            a[23] = 25852016738884976640000.0;
            a[24] = 620448401733239439360000.0;
            a[25] = 15511210043330985984000000.0;
            a[26] = 403291461126605635584000000.0;
            a[27] = 10888869450418352160768000000.0;
            a[28] = 304888344611713860501504000000.0;
            a[29] = 8841761993739701954543616000000.0;
            a[30] = 265252859812191058636308480000000.0;
            a[31] = 8222838654177922817725562880000000.0;
            a[32] = 263130836933693530167218012160000000.0;
            FactorialLookup = a;
        }

        private double Ni(int n, int i)
        {
            double ni;
            double a1 = factorial(n);
            double a2 = factorial(i);
            double a3 = factorial(n - i);
            ni = a1 / (a2 * a3);
            return ni;
        }

        // Calculate Bernstein basis
        private double Bernstein(int n, int i, double t)
        {
            double basis;
            double ti; /* t^i */
            double tni; /* (1 - t)^i */

            /* Prevent problems with pow */

            if (t == 0.0 && i == 0)
                ti = 1.0;
            else
                ti = Math.Pow(t, i);

            if (n == i && t == 1.0)
                tni = 1.0;
            else
                tni = Math.Pow((1 - t), (n - i));

            //Bernstein basis
            basis = Ni(n, i) * ti * tni;
            return basis;
        }

        public void Bezier2D(double[] b, int cpts, double[] p)
        {
            int npts = (b.Length) / 2;
            int icount, jcount;
            double step, t;

            // Calculate points on curve

            icount = 0;
            t = 0;
            step = (double)1.0 / (cpts - 1);

            for (int i1 = 0; i1 != cpts; i1++)
            {
                if ((1.0 - t) < 5e-6)
                    t = 1.0;

                jcount = 0;
                p[icount] = 0.0;
                p[icount + 1] = 0.0;
                for (int i = 0; i != npts; i++)
                {
                    double basis = Bernstein(npts - 1, i, t);
                    p[icount] += basis * b[jcount];
                    p[icount + 1] += basis * b[jcount + 1];
                    jcount = jcount + 2;
                }

                icount += 2;
                t += step;
            }
        }

        public PointLatLng[] CreateBesizer(GMapControl mapControl, PointLatLng startPos, PointLatLng endPos)
        {
            Point point = new Point((int)mapControl.FromLatLngToLocal(startPos).X, (int)mapControl.FromLatLngToLocal(startPos).Y);
            Point point2 = new Point((int)mapControl.FromLatLngToLocal(endPos).X, (int)mapControl.FromLatLngToLocal(endPos).Y);
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
            List<PointLatLng> pointList = new List<PointLatLng>();
            PointLatLng pll3 = mapControl.FromLocalToLatLng((int)tf3.X, (int)tf3.Y);
            PointLatLng pll4 = mapControl.FromLocalToLatLng((int)tf4.X, (int)tf4.Y);
            pointList.Add(startPos);
            pointList.Add(pll3);
            pointList.Add(pll4);
            pointList.Add(endPos);
            PointLatLng[] points = GetBezierPoints(pointList.ToArray(), 1000);
            return points;
        }

        PointLatLng[] GetBezierPoints(PointLatLng[] controlPoints, int outputSegmentCount)
        {
            List<double> ptList = new List<double>();
            foreach (var d in controlPoints)
            {
                ptList.Add(d.Lat);
                ptList.Add(d.Lng);
            }
            double[] ptind = new double[ptList.Count];
            double[] p = new double[outputSegmentCount];
            ptList.CopyTo(ptind, 0);
            Bezier2D(ptind, (outputSegmentCount) / 2, p);

            List<PointLatLng> points = new List<PointLatLng>();
            for (int i = 1; i != outputSegmentCount - 1; i += 2)
            {
                points.Add(new PointLatLng(p[i + 1], p[i]));
            }

            return points.ToArray();
        }
    }
}
