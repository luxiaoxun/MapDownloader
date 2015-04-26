using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMapCommonType;
using GMap.NET;

namespace GMapProvidersExt.Baidu
{
    public class BaituProjection : PureProjection
    {
        // Fields
        public static readonly BaituProjection Instance;
        private static readonly double MaxLatitude;
        private static readonly double MaxLongitude;
        private static readonly double MinLatitude;
        private static readonly double MinLongitude;
        private readonly GSize tileSize;

        // Methods
        static BaituProjection()
        {
            //Class3.VhQqLwFzr0qRr();
            Instance = new BaituProjection();
            MinLatitude = -85.05112878;
            MaxLatitude = 85.05112878;
            MinLongitude = -180.0;
            MaxLongitude = 180.0;
        }

        private BaituProjection()
        {
            //Class3.VhQqLwFzr0qRr();
            this.tileSize = new GSize(0x100, 0x100);
            //base.EpsgCode = 0xf11;
        }

        public override GPoint FromLatLngToPixel(double lat, double lng, int zoom)
        {
            Point2D pointd = LonLat2Mercator(new Point2D(lng, lat));
            pointd.Y -= 20000.0;
            this.GetTileMatrixMinXY(zoom);
            double levelResolution = this.GetLevelResolution(zoom);
            long x = (long)((pointd.X - this.BaiduProjectedOrigin.X) / levelResolution);
            return new GPoint(x, (long)((this.BaiduProjectedOrigin.Y - pointd.Y) / levelResolution));
        }

        public override PointLatLng FromPixelToLatLng(long x, long y, int zoom)
        {
            double levelResolution = this.GetLevelResolution(zoom);
            double num2 = x * levelResolution;
            double num3 = y * levelResolution;
            this.GetTileMatrixMinXY(zoom);
            double num4 = num2 + this.BaiduProjectedOrigin.X;
            double num5 = this.BaiduProjectedOrigin.Y - num3;
            num5 += 20000.0;
            PointLatLng lng = this.MercatorToLonLat(num4, num5);
            if (lng.Lat < MinLatitude)
            {
                lng.Lat = MinLatitude;
            }
            if (lng.Lat > MaxLatitude)
            {
                lng.Lat = MaxLatitude;
            }
            if (lng.Lng < MinLongitude)
            {
                lng.Lng = MinLongitude;
            }
            if (lng.Lng > MaxLongitude)
            {
                lng.Lng = MaxLongitude;
            }
            return lng;
        }

        public override double GetGroundResolution(int zoom, double latitude)
        {
            return this.GetLevelResolution(zoom);
        }

        public double GetLevelResolution(int level)
        {
            return Math.Pow(2.0, (double)(0x12 - level));
        }

        public override GSize GetTileMatrixMaxXY(int zoom)
        {
            return TileUtil.GetTile(this.BaiduProjectedOrigin.X, this.BaiduProjectedOrigin.Y, this.ProjectedBounds.Right, this.ProjectedBounds.Bottom, this.GetLevelResolution(zoom), 0x100, 0x100);
        }

        public override GSize GetTileMatrixMinXY(int zoom)
        {
            return TileUtil.GetTile(this.BaiduProjectedOrigin.X, this.BaiduProjectedOrigin.Y, this.ProjectedBounds.Left, this.ProjectedBounds.Top, this.GetLevelResolution(zoom), 0x100, 0x100);
        }

        private static Point2D LonLat2Mercator(Point2D lonLat)
        {
            double x = ((lonLat.X * 3.1415926535897931) * 6378137.0) / 180.0;
            double num2 = Math.Log(Math.Tan(((90.0 + lonLat.Y) * 3.1415926535897931) / 360.0)) / 0.017453292519943295;
            return new Point2D(x, ((num2 * 3.1415926535897931) * 6378137.0) / 180.0);
        }

        private PointLatLng MercatorToLonLat(double x, double y)
        {
            double lng = (x / (3.1415926535897931 * this.Axis)) * 180.0;
            y = (y / (3.1415926535897931 * this.Axis)) * 180.0;
            return new PointLatLng(57.295779513082323 * ((2.0 * Math.Atan(Math.Exp((y * 3.1415926535897931) / 180.0))) - 1.5707963267948966), lng);
        }

        // Properties
        public override double Axis
        {
            get
            {
                return 6378137.0;
            }
        }

        private Point2D BaiduProjectedOrigin
        {
            get
            {
                return new Point2D(-this.GetLevelResolution(1) * 256.0, this.GetLevelResolution(1) * 256.0);
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

        private BoundingBox ProjectedBounds
        {
            get
            {
                return new BoundingBox(-3.1415926535897931 * this.Axis, -3.1415926535897931 * this.Axis, 3.1415926535897931 * this.Axis, 3.1415926535897931 * this.Axis, null);
            }
        }

        public PointLatLng TileOrigin
        {
            get
            {
                return this.MercatorToLonLat(this.BaiduProjectedOrigin.X, this.BaiduProjectedOrigin.Y);
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
