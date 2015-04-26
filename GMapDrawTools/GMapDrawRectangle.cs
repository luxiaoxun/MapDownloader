using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapDrawTools
{
    public class GMapDrawRectangle : GMapPolygon
    {
        public GMapDrawRectangle(List<PointLatLng> points, string name)
            : base(points, name)
        {

        }
    }
}
