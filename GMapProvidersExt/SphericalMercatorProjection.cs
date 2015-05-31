using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMapCommonType;

namespace GMapProvidersExt
{
    public class SphericalMercatorProjection : PureProjection
    {
        // Fields
        public static readonly SphericalMercatorProjection Instance;
        private static readonly double MaxLatitude;
        private static readonly double MaxLongitude;
        private static readonly double MinLatitude;
        private static readonly double MinLongitude;
        private readonly GSize tileSize;

        public int EpsgCode;

        // Methods
        static SphericalMercatorProjection()
        {
            Instance = new SphericalMercatorProjection();
            MinLatitude = -85.05112878;
            MaxLatitude = 85.05112878;
            MinLongitude = -180.0;
            MaxLongitude = 180.0;
        }

        private SphericalMercatorProjection()
        {
            this.tileSize = new GSize(0x100, 0x100);
            EpsgCode = 0xf11;
        }

        public Point2D GetProjectedPoint(PointLatLng pointLatLng)
        {
            if (this.EpsgCode == 0x10e6)
            {
                return new Point2D(pointLatLng.Lng, pointLatLng.Lat);
            }
            if (this.EpsgCode == 0xf11)
            {
                return LonLat2Mercator(pointLatLng.Lng, pointLatLng.Lat);
            }
            return pointLatLng.TransformFromWGS84(ProjectionUtil.GetWKTFromEpsgCode(this.EpsgCode));
        }

        private static Point2D LonLat2Mercator(double x, double y)
        {
            double num = ((x * 3.1415926535897931) * 6378137.0) / 180.0;
            double num2 = Math.Log(Math.Tan(((90.0 + y) * 3.1415926535897931) / 360.0)) / 0.017453292519943295;
            return new Point2D(num, ((num2 * 3.1415926535897931) * 6378137.0) / 180.0);
        }

        public override GPoint FromLatLngToPixel(double lat, double lng, int zoom)
        {
            //GPoint point;
            Point2D projectedPoint = GetProjectedPoint(new PointLatLng(lat, lng));
            double x = projectedPoint.X;
            double y = projectedPoint.Y;
            long X = (long)Math.Round((double)((x - this.MercatorOrigin.Lng) / this.GetLevelResolution(zoom)));
            long Y = (long)Math.Round((double)((this.MercatorOrigin.Lat - y) / this.GetLevelResolution(zoom)));
            return new GPoint(getCorrectPixel(X), getCorrectPixel(Y));
        }

        public override PointLatLng FromPixelToLatLng(long x, long y, int zoom)
        {
            PointLatLng lng = new PointLatLng
                              {
                                  Lng = (-3.1415926535897931 * this.Axis) + (x * this.GetLevelResolution(zoom)),
                                  Lat = (3.1415926535897931 * this.Axis) - (y * this.GetLevelResolution(zoom))
                              };
            PointLatLng lng2 = this.MercatorToLonLat(lng.Lng, lng.Lat);
            if (lng2.Lat < MinLatitude)
            {
                lng2.Lat = MinLatitude;
            }
            if (lng2.Lat > MaxLatitude)
            {
                lng2.Lat = MaxLatitude;
            }
            if (lng2.Lng < MinLongitude)
            {
                lng2.Lng = MinLongitude;
            }
            if (lng2.Lng > MaxLongitude)
            {
                lng2.Lng = MaxLongitude;
            }
            return lng2;
        }

        private long getCorrectPixel(long p)
        {
            if ((p % this.TileSize.Width) == 0)
            {
                p--;
            }
            return p;
        }

        public double GetLevelResolution(int level)
        {
            return (((3.1415926535897931 * this.Axis) * 2.0) / (Math.Pow(2.0, (double)level) * this.TileSize.Width));
        }

        public double GetLevelScale(int level)
        {
            return Math.Round((double)((this.GetLevelResolution(level) * 96.0) / 0.0254), 2);
        }

        public override GSize GetTileMatrixMaxXY(int zoom)
        {
            long num = ((int)1) << zoom;
            return new GSize(num - 1, num - 1);
        }

        public override GSize GetTileMatrixMinXY(int zoom)
        {
            return new GSize(0, 0);
        }

        private PointLatLng MercatorToLonLat(double x, double y)
        {
            double lng = (x / (3.1415926535897931 * this.Axis)) * 180.0;
            y = (y / (3.1415926535897931 * this.Axis)) * 180.0;
            double lat = 57.295779513082323 *
                         ((2.0 * Math.Atan(Math.Exp((y * 3.1415926535897931) / 180.0))) - 1.5707963267948966);
            return new PointLatLng(lat, lng);
        }

        // Properties
        public override double Axis
        {
            get
            {
                return 6378137.0;
            }
        }

        public override RectLatLng Bounds
        {
            get
            {
                return RectLatLng.FromLTRB(MinLongitude, MaxLatitude, MaxLongitude, MinLatitude);
            }
        }

        public override double Flattening
        {
            get
            {
                return 0.0033528106647474627;
            }
        }

        private PointLatLng MercatorOrigin
        {
            get
            {
                return new PointLatLng(3.1415926535897931 * this.Axis, -3.1415926535897931 * this.Axis);
            }
        }

        public override GSize TileSize
        {
            get
            {
                return this.tileSize;
            }
        }
    }
}
