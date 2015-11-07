using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt
{
    public class WGS84Projection : PureProjection
    {
        // Fields
        public static readonly WGS84Projection Instance = new WGS84Projection();
        public readonly double MaxLatitude = 90;
        public readonly double MaxLongitude = 180;
        public readonly double MinLatitude = -90;
        public readonly double MinLongitude = -180;
        private static readonly double orignX = -180;
        private static readonly double orignY = 90;
        private GSize tileSize;

        public WGS84Projection()
        {
            this.tileSize = new GSize(0x100, 0x100);
            EpsgCode = 4326;
        }

        public override GPoint FromLatLngToPixel(double lat, double lng, int zoom)
        {
            GPoint empty = GPoint.Empty;
            lat = PureProjection.Clip(lat, this.MinLatitude, this.MaxLatitude);
            lng = PureProjection.Clip(lng, this.MinLongitude, this.MaxLongitude);
            double tileMatrixResolution = GetTileMatrixResolution(zoom);
            empty.X = (long)Math.Round((double)((lng - orignX) / tileMatrixResolution));
            empty.Y = (long)Math.Round((double)((orignY - lat) / tileMatrixResolution));
            return this.RectiryPixelPoint(empty);
        }

        public override PointLatLng FromPixelToLatLng(long x, long y, int zoom)
        {
            PointLatLng empty = PointLatLng.Empty;
            double tileMatrixResolution = GetTileMatrixResolution(zoom);
            empty.Lat = orignY - (y * tileMatrixResolution);
            empty.Lng = (x * tileMatrixResolution) + orignX;
            if (empty.Lat < this.MinLatitude)
            {
                empty.Lat = this.MinLatitude;
            }
            if (empty.Lat > this.MaxLatitude)
            {
                empty.Lat = this.MaxLatitude;
            }
            if (empty.Lng < this.MinLongitude)
            {
                empty.Lng = this.MinLongitude;
            }
            if (empty.Lng > this.MaxLongitude)
            {
                empty.Lng = this.MaxLongitude;
            }
            return empty;
        }

        public override double GetGroundResolution(int zoom, double latitude)
        {
            return GetTileMatrixResolution(zoom);
        }

        public double GetLevelResolution(int level)
        {
            return GetTileMatrixResolution(level);
        }

        public override GSize GetTileMatrixMaxXY(int zoom)
        {
            GPoint p = this.FromLatLngToPixel(this.MinLatitude, this.MaxLongitude, zoom);
            return new GSize(this.FromPixelToTileXY(p));
        }

        public override GSize GetTileMatrixMinXY(int zoom)
        {
            GPoint p = this.FromLatLngToPixel(this.MaxLatitude, this.MinLongitude, zoom);
            return new GSize(this.FromPixelToTileXY(p));
        }

        public static double GetTileMatrixResolution(int zoom)
        {
            double num = 1.40625;
            for (int i = 0; i < zoom; i++)
            {
                num /= 2.0;
            }
            return num;
        }

        private GPoint RectiryPixelPoint(GPoint pt)
        {
            if ((pt.X % this.TileSize.Width) == 0)
            {
                pt.X--;
            }
            if ((pt.Y % this.TileSize.Height) == 0)
            {
                pt.Y--;
            }
            return pt;
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
                return RectLatLng.FromLTRB(this.MinLongitude, this.MaxLatitude, this.MaxLongitude, this.MinLatitude);
            }
        }

        public override double Flattening
        {
            get
            {
                return 0.0033528106647474805;
            }
        }

        public PointLatLng TileOrigin
        {
            get
            {
                return new PointLatLng(90.0, -180.0);
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
