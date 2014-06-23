using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapTools
{
    public class DrawDistanceEventArgs : EventArgs
    {
        public List<LineMarker> LineMarkers { set; get; }
        public List<DrawDistanceMarker> DistanceMarkers { set; get; }
        public DrawDeleteMarker DistanceDeleteMarker { set; get; }

        public DrawDistanceEventArgs(List<LineMarker> lineMarkers,List<DrawDistanceMarker> distanceMarkers)
        {
            this.LineMarkers = lineMarkers;
            this.DistanceMarkers = distanceMarkers;
            PointLatLng lastPoint = DistanceMarkers[DistanceMarkers.Count - 1].Position;
            PointLatLng p = new PointLatLng(lastPoint.Lat, lastPoint.Lng);
            this.DistanceDeleteMarker = new DrawDeleteMarker(p);
        }
    }
}
