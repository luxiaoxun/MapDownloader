using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMapTools
{
    public class DrawDistanceEventArgs : EventArgs
    {
        public List<LineMarker> LineMarkers { set; get; }
        public List<DrawDistanceMarker> DistanceMarkers { set; get; }

        public DrawDistanceEventArgs(List<LineMarker> lineMarkers,List<DrawDistanceMarker> distanceMarkers)
        {
            this.LineMarkers = lineMarkers;
            this.DistanceMarkers = distanceMarkers;
        }
    }
}
