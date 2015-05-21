using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt
{
    public class SogouProjection : PureProjection
    {
        public static readonly SogouProjection Instance = new SogouProjection();

        static readonly double MinLatitude = -85;
        static readonly double MaxLatitude = 85;
        static readonly double MinLongitude = -180;
        static readonly double MaxLongitude = 180;
        static readonly int[] LLBAND = { 75, 60, 45, 30, 15, 0 };
        
        public override RectLatLng Bounds
        {
            get
            {
                return RectLatLng.FromLTRB(MinLongitude, MaxLatitude, MaxLongitude, MinLatitude);
            }
        }

        readonly GSize tileSize = new GSize(256, 256);
        public override GSize TileSize
        {
            get
            {
                return tileSize;
            }
        }

        public override double Axis
        {
            get
            {
                return 6378137;
            }
        }

        public override double Flattening
        {
            get
            {
                return (1.0 / 298.257223563);
            }
        }
        public override GPoint FromLatLngToPixel(double lat, double lng, int zoom)
        {
            GPoint ret = GPoint.Empty;

            lat = lat - 14.307 - 0.0070715774074;
            lng = lng - 45.245 + 0.004644012451;

            lat = Clip(lat, MinLatitude, MaxLatitude);
            lng = Clip(lng, MinLongitude, MaxLongitude);

            double x = (lng + 180) / 360;
            double sinLatitude = Math.Sin(lat * Math.PI / 180);
            double y = 0.5 - Math.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Math.PI);

            GSize s = GetTileMatrixSizePixel(zoom);
            long mapSizeX = s.Width;
            long mapSizeY = s.Height;

            ret.X = (long)Clip(x * mapSizeX + 0.5, 0, mapSizeX - 1);
            ret.Y = (long)Clip(y * mapSizeY + 0.5, 0, mapSizeY - 1);

            return ret;
        }
        
        public override PointLatLng FromPixelToLatLng(long x, long y, int zoom)
        {
            PointLatLng ret = PointLatLng.Empty;

            GSize s = GetTileMatrixSizePixel(zoom);
            double mapSizeX = s.Width;
            double mapSizeY = s.Height;

            double xx = (Clip(x, 0, mapSizeX - 1) / mapSizeX) - 0.5;
            double yy = 0.5 - (Clip(y, 0, mapSizeY - 1) / mapSizeY);

            ret.Lat = 90 - 360 * Math.Atan(Math.Exp(-yy * 2 * Math.PI)) / Math.PI + 14.307 + 0.0070715774074;
            ret.Lng = 360 * xx + 45.245 - 0.004644012451;

            return ret;
        }

        public override GSize GetTileMatrixMinXY(int zoom)
        {
            return new GSize(0, 0);
        }

        public override GSize GetTileMatrixMaxXY(int zoom)
        {
            long xy = (1 << zoom);
            return new GSize(xy - 1, xy - 1);
        }
    }
}

