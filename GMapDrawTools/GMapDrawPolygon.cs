using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapDrawTools
{
    public class GMapDrawPolygon : GMapPolygon
    {
        public GMapDrawPolygon(List<PointLatLng> points, string name)
            : base(points, name)
        {

        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
