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

        public GMapDrawRectangle Rectangle { set; get; }

        public GMapPolygon Polygon { get; set; }

        public GMapDrawCircle Circle { get; set; }

        public GMapRoute Route { set; get; }

        public DrawEventArgs()
        {
            
        }

        public DrawEventArgs(DrawingMode drawingMode, List<PointLatLng> drawingPoints, Pen stroke, Brush fill)
        {
            DrawingMode = drawingMode;
            if (drawingMode == DrawingMode.Polygon)
            {
                Polygon = new GMapPolygon(drawingPoints, drawingMode.ToString());
                Polygon.Fill = fill;
                Polygon.Stroke = stroke;
                Polygon.IsHitTestVisible = true;
            }
            else if (drawingMode == DrawingMode.Rectangle)
            {
                Rectangle = new GMapDrawRectangle(drawingPoints, drawingMode.ToString());
                Rectangle.Fill = fill;
                Rectangle.Stroke = stroke;
                Rectangle.IsHitTestVisible = true;
            }
            else if (drawingMode == DrawingMode.Route)
            {
                Route = new GMapRoute(drawingPoints, drawingMode.ToString());
                if (stroke != null)
                {
                    Route.Stroke = stroke;
                }
                Route.IsHitTestVisible = true;
            }
        }

        public DrawEventArgs(DrawingMode drawingMode, PointLatLng centerPoint, PointLatLng edgePoint, Pen stroke, Brush fill)
        {
            DrawingMode = drawingMode;
            Circle = new GMapDrawCircle(centerPoint, edgePoint, stroke,fill);
        }
    }
}
