using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMapCommonType;

namespace GMapCommonType
{
    public class Polyline : Geometry
    {
        // Fields
        private List<Point2D> points;

        // Methods
        public Polyline()
        {
            this.points = new List<Point2D>();
        }

        public Polyline(List<Point2D> points)
        {
            this.points = points;
        }

        // Properties
        public override GeometryType GeoType
        {
            get
            {
                return GeometryType.Polyline;
            }
        }

        public override List<Point2D> Points
        {
            get
            {
                return this.points;
            }
        }
    }


}
