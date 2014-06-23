using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapTools
{
    public class DrawDistance
    {
        private GMapOverlay tempOverlay = new GMapOverlay("tempPolygonsOverlay");

        private bool isStarted = false;
        private ToolTip toolTip = new ToolTip();
        private PointLatLng lastPointLatLng = PointLatLng.Empty;
        private TempLineMarker tempLine = null;
        private List<double> distanceList = new List<double>(); 

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
                    if (isEnable == true)
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

        public event EventHandler<DrawDistanceEventArgs> DrawComplete;

        public DrawDistance(GMapControl mapControl)
        {
            MapControl = mapControl;
            toolTip.SetToolTip(MapControl,null);

            this.toolTip.ShowAlways = true;
            this.toolTip.InitialDelay = 10;
            this.toolTip.AutoPopDelay = 800;
            this.toolTip.ReshowDelay = 1;
        }

        private void Active()
        {
            this.MapControl.CanDragMap = false;
            if (!MapControl.Overlays.Contains(tempOverlay))
            {
                MapControl.Overlays.Add(tempOverlay);
            }
            if (distanceList != null && distanceList.Count > 0)
            {
                distanceList.Clear();
            }

            
            MapControl.MouseDown += new System.Windows.Forms.MouseEventHandler(MapControl_MouseDown);
            MapControl.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(MapControl_MouseDoubleClick);
            MapControl.MouseMove += new System.Windows.Forms.MouseEventHandler(MapControl_MouseMove);
        }

        private void Deactive()
        {
            this.MapControl.CanDragMap = true;
            if (MapControl.Overlays.Contains(tempOverlay))
            {
                MapControl.Overlays.Remove(tempOverlay);
            }
            MapControl.MouseDown -= new System.Windows.Forms.MouseEventHandler(MapControl_MouseDown);
            MapControl.MouseDoubleClick -= new System.Windows.Forms.MouseEventHandler(MapControl_MouseDoubleClick);
            MapControl.MouseMove -= new System.Windows.Forms.MouseEventHandler(MapControl_MouseMove);
        }

        private void ClearTempDrawing()
        {
            tempOverlay.Markers.Clear();
            if (tempLine != null)
            {
                tempLine.Dispose();
            }
        }

        private void MapControl_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (isEnable && isStarted)
            {
                IsEnable = false;
                this.toolTip.Active = false;
                isStarted = false;

                if (DrawComplete != null)
                {
                    List<LineMarker> lineMarkers = new List<LineMarker>();
                    List<DrawDistanceMarker> distanceMarkers = new List<DrawDistanceMarker>();
                    foreach (GMapMarker marker in tempOverlay.Markers)
                    {
                        if (marker is LineMarker)
                        {
                            LineMarker line = marker as LineMarker;
                            lineMarkers.Add(new LineMarker(line.startPoint,line.endPoint));
                        }
                        if (marker is DrawDistanceMarker)
                        {
                            DrawDistanceMarker distanceMarker = marker as DrawDistanceMarker;
                            distanceMarkers.Add(new DrawDistanceMarker(distanceMarker.Position,distanceMarker.ToolTipText));
                        }
                    }
                    DrawComplete(this, new DrawDistanceEventArgs(lineMarkers, distanceMarkers));
                }

                ClearTempDrawing();
            }
        }

        void MapControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (isEnable && isStarted && lastPointLatLng!=PointLatLng.Empty)
            {
                PointLatLng p = MapControl.FromLocalToLatLng(e.X, e.Y);
                double x = p.Lng - lastPointLatLng.Lng;
                double y = p.Lat - lastPointLatLng.Lat;
                double dis = GMapHelper.GetDistanceInMeter(p, lastPointLatLng);
                double sum = distanceList.Sum();
                dis += sum;
                string s = string.Format("总长：{0:0.00}米", dis);
                //this.toolTip.Show(s, this.MapControl);
                this.toolTip.SetToolTip(this.MapControl, s);
                
                tempLine.EndPoint = p;
                this.MapControl.UpdateMarkerLocalPosition(tempLine);
                this.MapControl.Refresh();
            }
        }

        private void MapControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && !isStarted && isEnable)
            {
                isStarted = true;
                this.toolTip.Active = true;
                PointLatLng p = MapControl.FromLocalToLatLng(e.X, e.Y);
                DrawDistanceMarker marker = new DrawDistanceMarker(p,"起点");
                tempOverlay.Markers.Add(marker);
                lastPointLatLng = p;

                tempLine = new TempLineMarker(p, p);
                tempOverlay.Markers.Add(tempLine);
            }
            else if (e.Button == MouseButtons.Left && isStarted && isEnable)
            {
                PointLatLng p = MapControl.FromLocalToLatLng(e.X, e.Y);
                
                double x = p.Lng - lastPointLatLng.Lng;
                double y = p.Lat - lastPointLatLng.Lat;
                double dis = GMapHelper.GetDistanceInMeter(p, lastPointLatLng);
                distanceList.Add(dis);
                double sum = distanceList.Sum();
                string s = string.Format("{0:0.00}米", sum);
                
                DrawDistanceMarker marker = new DrawDistanceMarker(p, s);
                tempOverlay.Markers.Add(marker);

                LineMarker line = new LineMarker(new PointLatLng(lastPointLatLng.Lat,lastPointLatLng.Lng), p);
                tempOverlay.Markers.Add(line);

                if (tempOverlay.Markers.Contains(tempLine))
                {
                    tempOverlay.Markers.Remove(tempLine);
                }
                tempLine = new TempLineMarker(p, p);
                tempOverlay.Markers.Add(tempLine);

                lastPointLatLng = p;
            }
        }
    }
}
