using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMapMarkerLib;

namespace GMapWinFormDemo
{
    public class HistoryGeoOverlay:GMapOverlay
    {
        private Timer timer = new Timer();

        public bool Follow { get; set; }

        private IList<HistoryGeoData> geoDataList = new List<HistoryGeoData>();

        private int index = 0;
        private bool isStarted;
        private bool isPaused;
        private PointLatLng lastPoint = PointLatLng.Empty;

        public HistoryGeoOverlay()
        {
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Tick += new EventHandler(timer_Tick);
            isStarted = false;
            isPaused = false;
            Follow = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (geoDataList != null)
            {
                int len = geoDataList.Count;
                if (index < len)
                {
                    var historyGeoData = geoDataList[index];

                    PointLatLng p = new PointLatLng(historyGeoData.Y,historyGeoData.X);
                    GMapPointMarker point = new GMapPointMarker(p);
                    this.Markers.Add(point);

                    if (lastPoint != PointLatLng.Empty)
                    {
                        GMapLineArrowMarker line = new GMapLineArrowMarker(lastPoint, p,true);
                        this.Markers.Add(line);
                    }

                    lastPoint = p;

                    if (Follow && this.Control != null)
                    {
                        this.Control.Position = p;
                    }

                    ++index;
                }
                else
                {
                    Stop();
                }
            }
            else
            {
                Stop();
            }
        }

        public void Stop()
        {
            timer.Stop();
            index = 0;
            isStarted = false;
            isPaused = false;
            lastPoint = PointLatLng.Empty;
        }

        public void Start(IList<HistoryGeoData> dataList)
        {
            Stop();

            if (dataList == null || dataList.Count <= 0)
            {
                return;
            }

            this.geoDataList = (from data in dataList
                                where data != null && data.X != 0 && data.Y != 0
                                orderby data.Time ascending
                                select data).ToList();
            if (geoDataList != null && geoDataList.Count > 0)
            {
                index = 0;
                this.Markers.Clear();
                StartTimer();
            }

        }

        public void StartTimer()
        {
            timer.Start();
            isStarted = true;
        }

        public void SetTimerInterval(int span)
        {
            if (isStarted)
            {
                timer.Stop();
            }
            timer.Interval = span;

            if (isStarted)
            {
                timer.Start();
            }
        }

        public void Pause()
        {
            if (isPaused)
            {
                return;
            }
            if (isStarted)
            {
                timer.Stop();
                isStarted = false;
            }
            isPaused = true;
        }

        public void Resume()
        {
            if (isPaused)
            {
                if (!isStarted)
                {
                    timer.Start();
                    isStarted = true;
                }
            }
            isPaused = false;
        }
    }
}
