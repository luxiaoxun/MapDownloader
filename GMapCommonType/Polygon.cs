using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMapCommonType;

namespace GMapCommonType
{
    public class Polygon : Geometry
    {
        // Fields
        private List<Point2D> points;

        // Methods
        public Polygon()
        {
            this.points = new List<Point2D>();
        }

        public Polygon(List<Point2D> points)
        {
            this.points = points;
        }

        // Properties
        public override GeometryType GeoType
        {
            get
            {
                return GeometryType.Polygon;
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
