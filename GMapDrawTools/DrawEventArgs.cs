using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapDrawTools
{
    public class DrawEventArgs : EventArgs
    {
        public DrawingMode DrawingMode { get; private set; }

        public GMapPolygon Polygon { get; set; }

        public GMapDrawingCircle Circle { get; set; }

        public GMapRoute Route { set; get; }

        public DrawEventArgs()
        {
            
        }

        public DrawEventArgs(DrawingMode drawingMode, List<PointLatLng> drawingPoints)
        {
            DrawingMode = drawingMode;
            if (drawingMode == DrawingMode.Polygon || drawingMode == DrawingMode.Rectangle)
            {
                Polygon = new GMapPolygon(drawingPoints, drawingMode.ToString());
                Polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                Polygon.Stroke = new Pen(Color.Blue, 2);
                Polygon.IsHitTestVisible = true;
            }
            else if (drawingMode == DrawingMode.Route)
            {
                Route = new GMapRoute(drawingPoints, drawingMode.ToString());
                Route.Stroke = new Pen(Color.Blue, 2);
                Route.IsHitTestVisible = true;
            }
        }

        public DrawEventArgs(DrawingMode drawingMode, PointLatLng centerPoint, PointLatLng edgePoint)
        {
            DrawingMode = drawingMode;
            Circle = new GMapDrawingCircle(centerPoint,edgePoint);
        }
    }
}
