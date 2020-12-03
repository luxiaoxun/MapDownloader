using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt.Baidu
{
    public class BaiduProjection : PureProjection
    {
        // Fields
        public static readonly BaiduProjection Instance = new BaiduProjection();
        private static readonly double MinLatitude = -85.05112878;
        private static readonly double MaxLatitude = 85.05112878;
        private static readonly double MinLongitude = -180.0;
        private static readonly double MaxLongitude = 180.0;
        private readonly GSize tileSize = new GSize(256, 256);

        public override GPoint FromLatLngToPixel(double lat, double lng, int zoom)
        {
            PointLatLng p = LonLatToMercator(lng, lat);
            p.Lat -= 20000.0;
            double levelResolution = this.GetLevelResolution(zoom);
            long x = (long)((p.Lng - this.BaiduProjectedOrigin.Lng) / levelResolution);
            return new GPoint(x, (long)((this.BaiduProjectedOrigin.Lat - p.Lat) / levelResolution));
        }

        public override PointLatLng FromPixelToLatLng(long x, long y, int zoom)
        {
            double levelResolution = this.GetLevelResolution(zoom);
            double x_level = x * levelResolution;
            double y_level = y * levelResolution;
            //this.GetTileMatrixMinXY(zoom);
            double m_x = x_level + this.BaiduProjectedOrigin.Lng;
            double m_y = this.BaiduProjectedOrigin.Lat - y_level;
            m_y += 20000.0;
            PointLatLng p = this.MercatorToLonLat(m_x, m_y);
            if (p.Lat < MinLatitude)
            {
                p.Lat = MinLatitude;
            }
            if (p.Lat > MaxLatitude)
            {
                p.Lat = MaxLatitude;
            }
            if (p.Lng < MinLongitude)
            {
                p.Lng = MinLongitude;
            }
            if (p.Lng > MaxLongitude)
            {
                p.Lng = MaxLongitude;
            }
            return p;
        }

        public override double GetGroundResolution(int zoom, double latitude)
        {
            return this.GetLevelResolution(zoom);
        }

        public double GetLevelResolution(int level)
        {
            return Math.Pow(2.0, (double)(18 - level));
        }

        public override GSize GetTileMatrixMaxXY(int zoom)
        {
            return GetTile(this.BaiduProjectedOrigin.Lng, this.BaiduProjectedOrigin.Lat, this.ProjectedBounds.Right, this.ProjectedBounds.Bottom, this.GetLevelResolution(zoom), 0x100, 0x100);
        }

        public override GSize GetTileMatrixMinXY(int zoom)
        {
            return GetTile(this.BaiduProjectedOrigin.Lng, this.BaiduProjectedOrigin.Lat, this.ProjectedBounds.Left, this.ProjectedBounds.Top, this.GetLevelResolution(zoom), 0x100, 0x100);
        }

        private PointLatLng LonLatToMercator(double X, double Y)
        {
            double x = ((X * 3.1415926535897931) * 6378137.0) / 180.0;
            double num = Math.Log(Math.Tan(((90.0 + Y) * 3.1415926535897931) / 360.0)) / 0.017453292519943295;
            double y = ((num * 3.1415926535897931) * 6378137.0) / 180.0;
            return new PointLatLng(y, x);
        }

        private PointLatLng MercatorToLonLat(double x, double y)
        {
            double lng = (x / (3.1415926535897931 * this.Axis)) * 180.0;
            y = (y / (3.1415926535897931 * this.Axis)) * 180.0;
            return new PointLatLng(57.295779513082323 * ((2.0 * Math.Atan(Math.Exp((y * 3.1415926535897931) / 180.0))) - 1.5707963267948966), lng);
        }

        public GSize GetTile(double originX, double originY, double x, double y, double resolution, int tileWidth = 0x100, int tileHeight = 0x100)
        {
            double w = (x - originX) / (tileWidth * resolution);
            double h = (originY - y) / (tileHeight * resolution);
            long width = (long)Math.Floor(w);
            return new GSize(width, (long)Math.Floor(h));
        }

        // Properties
        public override double Axis
        {
            get
            {
                return 6378137.0;
            }
        }

        private PointLatLng BaiduProjectedOrigin
        {
            get
            {
                return new PointLatLng(this.GetLevelResolution(1) * 256.0, -this.GetLevelResolution(1) * 256.0);
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
                return new BoundingBox(-3.1415926535897931 * this.Axis, -3.1415926535897931 * this.Axis, 3.1415926535897931 * this.Axis, 3.1415926535897931 * this.Axis);
            }
        }

        public PointLatLng TileOrigin
        {
            get
            {
                return this.MercatorToLonLat(this.BaiduProjectedOrigin.Lng, this.BaiduProjectedOrigin.Lat);
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

    internal class BoundingBox
    {
        public double Left { set; get; }

        public double Bottom { set; get; }

        public double Right { set; get; }

        public double Top { set; get; }

        public BoundingBox(double left, double bottom, double right, double top)
        {
            Left = left;
            Bottom = bottom;
            Right = right;
            Top = top;
        }
    }
}
