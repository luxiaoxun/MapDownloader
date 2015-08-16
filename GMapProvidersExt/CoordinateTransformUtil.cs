using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMapCommonType;
using OSGeo.OSR;

namespace GMapProvidersExt
{
    public static class CoordinateTransformUtil
    {
        // Fields
        private static object transFormLock;

        // Methods
        static CoordinateTransformUtil()
        {
            transFormLock = new object();
        }

        public static Point2D LonLat2Mercator(Point2D lonLat)
        {
            double x = ((lonLat.X * 3.1415926535897931) * 6378137.0) / 180.0;
            double num2 = Math.Log(Math.Tan(((90.0 + lonLat.Y) * 3.1415926535897931) / 360.0)) / 0.017453292519943295;
            return new Point2D(x, ((num2 * 3.1415926535897931) * 6378137.0) / 180.0);
        }

        public static Point2D LonLat2Mercator(PointLatLng lonLat)
        {
            double x = ((lonLat.Lng * 3.1415926535897931) * 6378137.0) / 180.0;
            double num2 = Math.Log(Math.Tan(((90.0 + lonLat.Lat) * 3.1415926535897931) / 360.0)) / 0.017453292519943295;
            return new Point2D(x, ((num2 * 3.1415926535897931) * 6378137.0) / 180.0);
        }

        public static Point2D Mercator2lonLat(Point2D pt)
        {
            double x = (pt.X / 20037508.342789244) * 180.0;
            double num2 = (pt.Y / 20037508.342789244) * 180.0;
            return new Point2D(x, 57.295779513082323 * ((2.0 * Math.Atan(Math.Exp((num2 * 3.1415926535897931) / 180.0))) - 1.5707963267948966));
        }

        public static Point2D Transform(this Point2D point, CoordinateTransformation transformObj = null)
        {
            double[] numArray = new double[3];
            numArray[0] = point.X;
            numArray[1] = point.Y;
            double[] inout = numArray;
            transformObj.TransformPoint(inout);
            return new Point2D(inout[0], inout[1]);
        }

        public static Point2D Transform(this Point2D point, string sourceProjection, string targetProjection)
        {
            Point2D pointd;
            if (sourceProjection.Equals(targetProjection))
            {
                return point;
            }
            int epsgCodeFromWKT = ProjectionUtil.GetEpsgCodeFromWKT(sourceProjection);
            if (epsgCodeFromWKT > 0)
            {
                sourceProjection = ProjectionUtil.GetWKTFromEpsgCode(epsgCodeFromWKT);
            }
            int epsgCode = ProjectionUtil.GetEpsgCodeFromWKT(targetProjection);
            if (epsgCode > 0)
            {
                targetProjection = ProjectionUtil.GetWKTFromEpsgCode(epsgCode);
            }
            if (epsgCodeFromWKT == epsgCode)
            {
                return point;
            }
            if ((epsgCodeFromWKT == 0x10e6) && (epsgCode == 0xf11))
            {
                return LonLat2Mercator(point);
            }
            if ((epsgCodeFromWKT == 0xf11) && (epsgCode == 0x10e6))
            {
                return Mercator2lonLat(point);
            }
            using (SpatialReference reference = new SpatialReference(sourceProjection))
            {
                using (SpatialReference reference2 = new SpatialReference(targetProjection))
                {
                    using (CoordinateTransformation transformation = new CoordinateTransformation(reference, reference2))
                    {
                        double[] numArray = new double[3];
                        numArray[0] = point.X;
                        numArray[1] = point.Y;
                        double[] inout = numArray;
                        transformation.TransformPoint(inout);
                        reference.Dispose();
                        reference2.Dispose();
                        transformation.Dispose();
                        pointd = new Point2D(inout[0], inout[1]);
                    }
                }
            }
            return pointd;
        }

        public static void Transform(ref PointLatLng point, string sourceProjection, string targetProjection)
        {
            Point2D pointd = new Point2D(point.Lng, point.Lat).Transform(sourceProjection, targetProjection);
            point.Lng = pointd.X;
            point.Lat = pointd.Y;
        }

        public static Point2D TransformFromWGS84(this PointLatLng latLng, string targetProjection)
        {
            return new Point2D(latLng.Lng, latLng.Lat).Transform(ProjectionUtil.GetWKTFromEpsgCode(0x10e6), targetProjection);
        }

        public static PointLatLng TransformToWGS84(this Point2D point, string sourceProjection)
        {
            Point2D pointd = point.Transform(sourceProjection, ProjectionUtil.GetWKTFromEpsgCode(0x10e6));
            return new PointLatLng(pointd.Y, pointd.X);
        }

        public static List<PointLatLng> TransformToWGS84(this List<PointLatLng> points, string sourceProjection)
        {
            List<PointLatLng> list = new List<PointLatLng>();
            foreach (PointLatLng lng in points)
            {
                Point2D pointd2 = new Point2D(lng.Lng, lng.Lat).Transform(sourceProjection, ProjectionUtil.GetWKTFromEpsgCode(0x10e6));
                list.Add(new PointLatLng(pointd2.Y, pointd2.X));
            }
            return list;
        }
    }

}
