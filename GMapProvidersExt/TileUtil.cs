using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt
{
    public static class TileUtil
    {
        // Methods
        public static GSize GetTile(double originX, double originY, double x, double y, double resolution, int tileWidth = 0x100, int tileHeight = 0x100)
        {
            double d = (x - originX) / (tileWidth * resolution);
            double num2 = (originY - y) / (tileHeight * resolution);
            long width = (long)Math.Floor(d);
            return new GSize(width, (long)Math.Floor(num2));
        }
    }
}
