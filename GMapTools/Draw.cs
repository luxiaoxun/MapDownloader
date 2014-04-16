using System;
using System.Collections.Generic;
using System.Drawing;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapTools
{
    public class Draw
    {
        private GMapOverlay tempPolygonsOverlay = new GMapOverlay("tempPolygonsOverlay");//放置polygon的临时图层
        private List<PointLatLng> drawingPoints = new List<PointLatLng>(); //多边形的点集
        private GMapPolygon drawingPolygon = null; //正在画的polygon
        private GMapDrawingCircle drawingCircle = null; //正在画的circle
        private bool isLeftButtonDown = false;

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

        private void MapControl_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (drawingMode == DrawingMode.Polygon && drawingPolygon != null)
            {
                DrawEventArgs args = new DrawEventArgs(drawingMode, drawingPoints);
                OnDrawComplete(args);
                
                tempPolygonsOverlay.Polygons.Clear();
                if (drawingPolygon != null)
                {
                    drawingPolygon.Dispose();
                    drawingPolygon = null;
                }
                drawingPoints.Clear();
                drawingMode = DrawingMode.None;
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
                drawingPoints.Add(startPos);

                DrawEventArgs args = new DrawEventArgs(drawingMode, drawingPoints);
                OnDrawComplete(args);

                tempPolygonsOverlay.Polygons.Clear();
                if (drawingPolygon != null)
                {
                    drawingPolygon.Dispose();
                    drawingPolygon = null;
                }
                drawingPoints.Clear();
                drawingMode = DrawingMode.None;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left && drawingMode == DrawingMode.Circle &&
                drawingCircle != null)
            {
                PointLatLng center = drawingPoints[0];
                PointLatLng currentPos = MapControl.FromLocalToLatLng(e.X, e.Y);
                drawingCircle.EdgePoint = currentPos;

                DrawEventArgs args = new DrawEventArgs(drawingMode,center,currentPos);
                OnDrawComplete(args);

                tempPolygonsOverlay.Markers.Clear();
                if (drawingCircle != null)
                {
                    drawingCircle.Dispose();
                    drawingCircle = null;
                }
                drawingPoints.Clear();
                drawingMode = DrawingMode.None;

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
                    drawingPolygon.Points.Add(startPos);
                    MapControl.UpdatePolygonLocalPosition(drawingPolygon);
                    MapControl.Refresh();
                }
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left && isLeftButtonDown &&
                drawingMode == DrawingMode.Circle)
            {
                if (drawingCircle != null)
                {
                    //PointLatLng startPos = drawingPoints[0];
                    PointLatLng currentPos = MapControl.FromLocalToLatLng(e.X, e.Y);
                    drawingCircle.EdgePoint = currentPos;
                    MapControl.UpdateMarkerLocalPosition(drawingCircle);
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
                        drawingPolygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                        drawingPolygon.Stroke = new Pen(Color.Blue, 2);
                        drawingPolygon.IsHitTestVisible = false;
                        tempPolygonsOverlay.Polygons.Add(drawingPolygon);
                        MapControl.Refresh();
                    }
                    else
                    {
                        drawingPolygon.Points.Clear();
                        drawingPolygon.Points.AddRange(drawingPoints);
                        //if (tempPolygonsOverlay.Polygons.Count == 0)
                        //{
                        //    tempPolygonsOverlay.Polygons.Add(drawingPolygon);
                        //}
                        //else
                        //{
                        //    MapControl.UpdatePolygonLocalPosition(drawingPolygon);
                        //}
                        MapControl.UpdatePolygonLocalPosition(drawingPolygon);
                        MapControl.Refresh();
                    }
                }

                if (drawingMode == DrawingMode.Rectangle)
                {
                    drawingPoints.Add(MapControl.FromLocalToLatLng(e.X, e.Y));
                    if (drawingPolygon == null)
                    {
                        drawingPolygon = new GMapPolygon(drawingPoints, "Rectangle");
                        drawingPolygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                        drawingPolygon.Stroke = new Pen(Color.Blue, 2);
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
                        drawingCircle = new GMapDrawingCircle(center, center);
                        tempPolygonsOverlay.Markers.Add(drawingCircle);
                    }
                }
            }
        }
    }
}
