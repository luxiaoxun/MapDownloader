using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMapCommonType;

namespace GMapUtil
{
    public class KmlPlaceMark
    {
        // Methods
        public KmlPlaceMark()
        {
        }

        public KmlPlaceMark(KmlPlaceMark mark)
        {
            this.Name = mark.Name;
            this.Description = mark.Description;
            this.Geometry = mark.Geometry;
        }

        // Properties
        public string Description { get; set; }

        public Geometry Geometry { get; set; }

        public string Name { get; set; }

        public string StyleUrl { get; set; }
    }
}
