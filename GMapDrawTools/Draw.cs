using System;
using System.Collections.Generic;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapDrawTools
{
    public class Draw
    {
        private GMapOverlay tempPolygonsOverlay = new GMapOverlay("tempPolygonsOverlay");//放置polygon的临时图层
        private List<PointLatLng> drawingPoints = new List<PointLatLng>(); //多边形的点集
        private GMapPolygon drawingPolygon = null; //正在画的polygon
        private GMapDrawCircle drawingCircle = null; //正在画的circle
        private GMapDrawRoute drawingRoute = null;
        private GMapDrawLine drawingLine = null;
        private bool isLeftButtonDown = false;

        private Pen stroke = new Pen(Color.Blue, 2);
        public Pen Stroke
        {
            set { stroke = value; }
            get { return stroke; }
        }

        private Brush fill = null;
        public Brush Fill
        {
            set { fill = value; }
            get { return fill; }
        }

        public GMapControl MapControl
        {
            set;
            get;
        }

        private bool isEnable = false;
        public bool IsEnable
        {
            get { return isEnable; }
            set
            {
                if (isEnable != value)
                {
                    isEnable = value;
                    if (isEnable == true && DrawingMode != DrawingMode.None)
                    {
                        Active();
                    }
                    else
                    {
                        Deactive();
                    }
                }
            }
        }

        private DrawingMode drawingMode = DrawingMode.None;

        public DrawingMode DrawingMode
        {
            set
            {
                drawingMode = value;
            }
            get
            {
                return drawingMode;
            }
        }

        public event EventHandler<DrawEventArgs> DrawComplete; 

        public Draw(GMapControl mapControl)
        {
            MapControl = mapControl;
            DrawingMode = DrawingMode.None;
        }

        private void Active()
        {
            this.MapControl.CanDragMap = false;
            if (!MapControl.Overlays.Contains(tempPolygonsOverlay))
            {
                MapControl.Overlays.Add(tempPolygonsOverlay);
            }
            
            MapControl.MouseDown += new System.Windows.Forms.MouseEventHandler(MapControl_MouseDown);
            MapControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(MapControl_MouseDoubleClick);
            MapControl.MouseUp += new System.Windows.Forms.MouseEventHandler(MapControl_MouseUp);
            MapControl.MouseMove += new System.Windows.Forms.MouseEventHandler(MapControl_MouseMove);
        }

        private void Deactive()
        {
            this.MapControl.CanDragMap = true;
            if (MapControl.Overlays.Contains(tempPolygonsOverlay))
            {
                MapControl.Overlays.Remove(tempPolygonsOverlay);
            }
            MapControl.MouseDown -= new System.Windows.Forms.MouseEventHandler(MapControl_MouseDown);
            MapControl.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(MapControl_MouseDoubleClick);
            MapControl.MouseUp -= new System.Windows.Forms.MouseEventHandler(MapControl_MouseUp);
            MapControl.MouseMove -= new System.Windows.Forms.MouseEventHandler(MapControl_MouseMove);
        }

        private void OnDrawComplete(DrawEventArgs e)
        {
            var handler = DrawComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void ClearTempDrawing()
        {
            tempPolygonsOverlay.Polygons.Clear();
            tempPolygonsOverlay.Markers.Clear();
            tempPolygonsOverlay.Routes.Clear();

            if (drawingPolygon != null)
            {
                drawingPolygon.Dispose();
                drawingPolygon = null;
            }
            drawingPoints.Clear();

            if (drawingCircle != null)
            {
                drawingCircle.Dispose();
                drawingCircle = null;
            }
            if (drawingRoute != null)
            {
                drawingRoute.Dispose();
                drawingRoute = null;
            }
            if (drawingLine != null)
            {
                drawingLine.Dispose();
                drawingLine = null;
            }
            drawingMode = DrawingMode.None;
        }

        private void MapControl_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (drawingMode == DrawingMode.Polygon && drawingPolygon != null)
            {
                //Double click to complete drawing polygon 
                //Remove the duplicated last point
                List<PointLatLng> drawPoints = new List<PointLatLng>();
                drawPoints.AddRange(drawingPoints.GetRange(0, drawingPoints.Count-1));
                DrawEventArgs args = new DrawEventArgs(drawingMode, drawPoints, Stroke,Fill);
                OnDrawComplete(args);

                ClearTempDrawing();
            }

            if (drawingMode == DrawingMode.Route && drawingRoute != null)
            {
                //Double click to complete drawing polygon 
                //Remove the duplicated last point
                List<PointLatLng> drawPoints = new List<PointLatLng>();
                drawPoints.AddRange(drawingPoints.GetRange(0, drawingPoints.Count - 1));
                DrawEventArgs args = new DrawEventArgs(drawingMode, drawPoints, Stroke, Fill);
                OnDrawComplete(args);

                ClearTempDrawing();
            }
        }

        void MapControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                isLeftButtonDown = false;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left && drawingMode == DrawingMode.Rectangle &&
                drawingPolygon != null)
            {
                PointLatLng startPos = drawingPoints[0];
                PointLatLng currentPos = MapControl.FromLocalToLatLng(e.X, e.Y);
                drawingPoints.Clear();
                drawingPoints.Add(startPos);
                drawingPoints.Add(new PointLatLng(currentPos.Lat, startPos.Lng));
                drawingPoints.Add(currentPos);
                drawingPoints.Add(new PointLatLng(startPos.Lat, currentPos.Lng));

                DrawEventArgs args = new DrawEventArgs(drawingMode, drawingPoints, Stroke, Fill);
                OnDrawComplete(args);

                ClearTempDrawing();
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left && drawingMode == DrawingMode.Circle &&
                drawingCircle != null)
            {
                PointLatLng center = drawingPoints[0];
                PointLatLng currentPos = MapControl.FromLocalToLatLng(e.X, e.Y);
                drawingCircle.EdgePoint = currentPos;

                DrawEventArgs args = new DrawEventArgs(drawingMode, center, currentPos, Stroke, Fill);
                OnDrawComplete(args);

                ClearTempDrawing();
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left && drawingMode == DrawingMode.Line &&
                drawingLine != null)
            {
                List<PointLatLng> drawPoints = drawingLine.Points;
                if (drawPoints.Count == 2) //2 points for a line
                {
                    DrawEventArgs args = new DrawEventArgs(drawingMode, drawPoints, Stroke, Fill);
                    OnDrawComplete(args);

                    ClearTempDrawing();
                }
            }
        }

        void MapControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && isLeftButtonDown &&
                drawingMode == DrawingMode.Rectangle)
            {
                if (drawingPoints.Count > 0 && drawingPolygon != null)
                {
                    PointLatLng startPos = drawingPoints[0];
                    PointLatLng currentPos = MapControl.FromLocalToLatLng(e.X, e.Y);

                    drawingPolygon.Points.Clear();
                    drawingPolygon.Points.Add(startPos);
                    drawingPolygon.Points.Add(new PointLatLng(currentPos.Lat, startPos.Lng));
                    drawingPolygon.Points.Add(currentPos);
                    drawingPolygon.Points.Add(new PointLatLng(startPos.Lat, currentPos.Lng));
                    MapControl.UpdatePolygonLocalPosition(drawingPolygon);
                    MapControl.Refresh();
                }
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left && isLeftButtonDown &&
                drawingMode == DrawingMode.Circle)
            {
                if (drawingCircle != null)
                {
                    PointLatLng currentPos = MapControl.FromLocalToLatLng(e.X, e.Y);
                    drawingCircle.EdgePoint = currentPos;
                    MapControl.UpdateMarkerLocalPosition(drawingCircle);
                    MapControl.Refresh();
                }
            }

            if (DrawingMode == DrawingMode.Route)
            {
                if (drawingRoute != null && drawingPoints.Count > 0)
                {
                    PointLatLng currentPos = MapControl.FromLocalToLatLng(e.X, e.Y);

                    drawingRoute.Points.Clear();
                    drawingRoute.Points.AddRange(drawingPoints);
                    drawingRoute.Points.Add(currentPos);
                    MapControl.UpdateRouteLocalPosition(drawingRoute);
                    MapControl.Refresh();
                }
            }

            if (DrawingMode == DrawingMode.Line)
            {
                if (drawingLine != null && drawingPoints.Count > 0)
                {
                    PointLatLng startPos = drawingPoints[0];
                    PointLatLng currentPos = MapControl.FromLocalToLatLng(e.X, e.Y);

                    drawingLine.Points.Clear();
                    drawingLine.Points.Add(startPos);
                    drawingLine.Points.Add(currentPos);
                    MapControl.UpdateRouteLocalPosition(drawingLine);
                    MapControl.Refresh();
                }
            }
        }

        private void MapControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                isLeftButtonDown = true;

                if (drawingMode == DrawingMode.Polygon)
                {
                    drawingPoints.Add(MapControl.FromLocalToLatLng(e.X, e.Y));
                    if (drawingPolygon == null)
                    {
                        drawingPolygon = new GMapPolygon(drawingPoints, "Polygon");
                        drawingPolygon.Fill = Fill;
                        drawingPolygon.Stroke = Stroke;
                        drawingPolygon.IsHitTestVisible = false;
                        tempPolygonsOverlay.Polygons.Add(drawingPolygon);
                        MapControl.Refresh();
                    }
                    else
                    {
                        drawingPolygon.Points.Clear();
                        drawingPolygon.Points.AddRange(drawingPoints);
                        MapControl.UpdatePolygonLocalPosition(drawingPolygon);
                        MapControl.Refresh();
                    }
                }

                if (drawingMode == DrawingMode.Route)
                {
                    drawingPoints.Add(MapControl.FromLocalToLatLng(e.X, e.Y));
                    if (drawingRoute == null)
                    {
                        drawingRoute = new GMapDrawRoute(drawingPoints, "Route");
                        if (Stroke != null)
                        {
                            drawingRoute.Stroke = Stroke;
                        }
                        drawingRoute.IsHitTestVisible = false;
                        tempPolygonsOverlay.Routes.Add(drawingRoute);
                        MapControl.Refresh();
                    }
                    else
                    {
                        drawingRoute.Points.Clear();
                        drawingRoute.Points.AddRange(drawingPoints);
                        MapControl.UpdateRouteLocalPosition(drawingRoute);
                        MapControl.Refresh();
                    }
                }

                if (drawingMode == DrawingMode.Line)
                {
                    drawingPoints.Add(MapControl.FromLocalToLatLng(e.X, e.Y));
                    if (drawingLine == null)
                    {
                        drawingLine = new GMapDrawLine(drawingPoints, "Line");
                        if (Stroke != null)
                        {
                            drawingLine.Stroke = Stroke;
                        }
                        drawingLine.IsHitTestVisible = false;
                        tempPolygonsOverlay.Routes.Add(drawingLine);
                        MapControl.Refresh();
                    }
                    else
                    {
                        drawingLine.Points.Clear();
                        drawingLine.Points.AddRange(drawingPoints);
                        MapControl.UpdateRouteLocalPosition(drawingLine);
                        MapControl.Refresh();
                    }
                }

                if (drawingMode == DrawingMode.Rectangle)
                {
                    drawingPoints.Add(MapControl.FromLocalToLatLng(e.X, e.Y));
                    if (drawingPolygon == null)
                    {
                        drawingPolygon = new GMapPolygon(drawingPoints, "Rectangle");
                        drawingPolygon.Fill = Fill;
                        drawingPolygon.Stroke = Stroke;
                        drawingPolygon.IsHitTestVisible = false;
                        tempPolygonsOverlay.Polygons.Add(drawingPolygon);
                    }
                }

                if (drawingMode == DrawingMode.Circle)
                {
                    PointLatLng center = MapControl.FromLocalToLatLng(e.X, e.Y);
                    drawingPoints.Add(center);
                    if (drawingCircle == null)
                    {
                        drawingCircle = new GMapDrawCircle(center, center, Stroke, Fill);
                        tempPolygonsOverlay.Markers.Add(drawingCircle);
                    }
                }
            }
        }
    }
}
