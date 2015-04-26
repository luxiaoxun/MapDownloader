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

        public GMapDrawPolygon Polygon { get; set; }

        public GMapDrawCircle Circle { get; set; }

        public GMapDrawRoute Route { set; get; }

        public GMapDrawLine Line { set; get; }

        public DrawEventArgs()
        {
            
        }

        public DrawEventArgs(DrawingMode drawingMode, List<PointLatLng> drawingPoints, Pen stroke, Brush fill)
        {
            DrawingMode = drawingMode;
            if (drawingMode == DrawingMode.Polygon)
            {
                Polygon = new GMapDrawPolygon(drawingPoints, drawingMode.ToString());
                if (fill != null)
                {
                    Polygon.Fill = (Brush)fill.Clone();
                }
                if (stroke != null)
                {
                    Polygon.Stroke = (Pen)stroke.Clone();
                }
                Polygon.IsHitTestVisible = true;
            }
            else if (drawingMode == DrawingMode.Rectangle)
            {
                Rectangle = new GMapDrawRectangle(drawingPoints, drawingMode.ToString());
                if (fill != null)
                {
                    Rectangle.Fill = (Brush)fill.Clone();
                }
                if (stroke != null)
                {
                    Rectangle.Stroke = (Pen)stroke.Clone();
                }
                Rectangle.IsHitTestVisible = true;
            }
            else if (drawingMode == DrawingMode.Route)
            {
                Route = new GMapDrawRoute(drawingPoints, drawingMode.ToString());
                if (stroke != null)
                {
                    Route.Stroke = (Pen)stroke.Clone();
                }
                Route.IsHitTestVisible = true;
            }
            else if (drawingMode == DrawingMode.Line)
            {
                Line = new GMapDrawLine(drawingPoints,drawingMode.ToString());
                if (stroke != null)
                {
                    Line.Stroke = (Pen)stroke.Clone();
                }
                Line.IsHitTestVisible = true;
            }
        }

        public DrawEventArgs(DrawingMode drawingMode, PointLatLng centerPoint, PointLatLng edgePoint, Pen stroke, Brush fill)
        {
            DrawingMode = drawingMode;
            Circle = new GMapDrawCircle(centerPoint, edgePoint, stroke,fill);
        }
    }
}
