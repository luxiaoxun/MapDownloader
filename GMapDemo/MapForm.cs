using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.Markers;
using GMapMarkerLib;
using GMapProvidersExt;
using GMapChinaRegion;
using GMapTools;

namespace GMapWinFormDemo
{
    public partial class MapForm : Form
    {
        private GMapOverlay markersOverlay = new GMapOverlay("markers"); //放置marker的图层
        private GMapMarker currentMarker;
        private bool isLeftButtonDown = false;
        private GMapPolygon currentPolygon;

        private GMapOverlay polygonsOverlay = new GMapOverlay("polygonsOverlay"); //放置polygon的图层

        private GMapOverlay locations = new GMapOverlay("locations"); //放置搜索结果的图层
        private GeocodingProvider gp; //地址编码服务
        List<PointLatLng> searchResult = new List<PointLatLng>(); //搜索结果
        PointLatLng start = PointLatLng.Empty; //路径开始点
        PointLatLng end = PointLatLng.Empty;   //路径结束点

        private MapType mapType;
        private MapProviderType mapProviderType;
        private GMapOverlay regionOverlay = new GMapOverlay("region");

        private Draw draw;

        public MapForm()
        {
            InitializeComponent();
            
            try
            {
                System.Net.IPHostEntry e = System.Net.Dns.GetHostEntry("www.google.com.hk");
            }
            catch
            {
                mapControl.Manager.Mode = AccessMode.CacheOnly;
                MessageBox.Show("No internet connection avaible, going to CacheOnly mode.", "GMap.NET Demo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            mapControl.CacheLocation = Environment.CurrentDirectory + "\\GMapCache\\"; //缓存位置
            mapControl.MapProvider = GMapProviders.GoogleChinaMap; //google china 地图
            mapControl.MinZoom = 2;  //最小比例
            mapControl.MaxZoom = 24; //最大比例
            mapControl.Zoom = 10;     //当前比例
            mapControl.ShowCenter = false; //不显示中心十字点
            mapControl.DragButton = System.Windows.Forms.MouseButtons.Left; //左键拖拽地图
            mapControl.Position = new PointLatLng(32.064, 118.704); //地图中心位置：南京
            mapControl.MouseWheelZoomType = MouseWheelZoomType.MousePositionWithoutCenter;

            mapControl.MouseClick += new MouseEventHandler(mapControl_MouseClick);
            mapControl.MouseDown += new MouseEventHandler(mapControl_MouseDown);
            mapControl.MouseUp += new MouseEventHandler(mapControl_MouseUp);
            mapControl.MouseMove += new MouseEventHandler(mapControl_MouseMove);
            mapControl.MouseDoubleClick += new MouseEventHandler(mapControl_MouseDoubleClick);

            mapControl.OnMarkerClick += new MarkerClick(mapControl_OnMarkerClick);
            mapControl.OnMarkerEnter += new MarkerEnter(mapControl_OnMarkerEnter);
            mapControl.OnMarkerLeave += new MarkerLeave(mapControl_OnMarkerLeave);

            mapControl.OnPolygonEnter += new PolygonEnter(mapControl_OnPolygonEnter);
            mapControl.OnPolygonLeave += new PolygonLeave(mapControl_OnPolygonLeave);

            mapControl.Overlays.Add(markersOverlay);
            mapControl.Overlays.Add(locations);
            mapControl.Overlays.Add(regionOverlay);
            mapControl.Overlays.Add(polygonsOverlay);

            gp = mapControl.MapProvider as GeocodingProvider;
            if (gp == null) //地址转换服务，没有就使用OpenStreetMap
            {
                gp = GMapProviders.OpenStreetMap as GeocodingProvider;
            }
            GMapProvider.Language = LanguageType.ChineseSimplified; //使用的语言，默认是英文

            InitUI();

            draw = new Draw(this.mapControl);
            draw.DrawComplete += new EventHandler<DrawEventArgs>(draw_DrawComplete);
        }

        void draw_DrawComplete(object sender, DrawEventArgs e)
        {
            if (e != null && (e.Polygon != null || e.Circle != null))
            {
                switch (e.DrawingMode)
                {
                    case DrawingMode.Polygon:
                        {
                            polygonsOverlay.Polygons.Add(e.Polygon);
                            break;
                        }
                    case DrawingMode.Rectangle:
                        {
                            polygonsOverlay.Polygons.Add(e.Polygon);
                            break;
                        }
                    case DrawingMode.Circle:
                        {
                            polygonsOverlay.Markers.Add(e.Circle);
                            break;
                        }
                    default:
                        draw.IsEnable = false;
                        break;
                }
            }
            draw.IsEnable = false;
        }

        private void InitUI()
        {
            this.谷歌地图ToolStripMenuItem.Enabled = false;
            this.buttonMapType.Image = Properties.Resources.weixing;

            mapType = MapType.Common;
            mapProviderType = MapProviderType.google;

            this.panelMap.SizeChanged += new EventHandler(panelMap_SizeChanged);
            List<string> regionNames = GMapChinaRegion.MapRegion.GetAllRegionName();
            foreach (var regionName in regionNames)
            {
                this.comboBoxRegion.Items.Add(regionName);
               
            }
            this.comboBoxRegion.SelectedValueChanged += new EventHandler(comboBoxRegion_SelectedValueChanged);
        }

        void comboBoxRegion_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                string selectedName = this.comboBoxRegion.GetItemText(this.comboBoxRegion.SelectedItem);
                GMapPolygon p = GMapChinaRegion.MapRegion.CreateMapPolygon(selectedName);
                if (p != null)
                {
                    regionOverlay.Polygons.Clear();
                    regionOverlay.Polygons.Add(p);
                    //RectLatLng rect = new RectLatLng(p.Points[0].Lat,p.Points[0].Lng,Math.Abs(p.Points[p.Points.Count/2].Lng-p.Points[0].Lng),Math.Abs(p.Points[p.Points.Count/2].Lat-p.Points[0].Lat));
                    RectLatLng rect = GMapChinaRegion.MapRegion.GetRegionMaxRect(p);
                    this.mapControl.SetZoomToFitRect(rect);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        void panelMap_SizeChanged(object sender, EventArgs e)
        {
            this.buttonMapType.Location = new Point(this.panelMap.Location.X + panelMap.Width - 65, this.panelMap.Location.Y + this.menuStrip1.Height + 10);
            this.comboBoxRegion.Location = new Point(this.menuStrip1.Location.X + panelMap.Width - 90, this.menuStrip1.Location.Y );
        }

        void mapControl_OnPolygonLeave(GMapPolygon item)
        {
            currentPolygon = null;
            item.Stroke.Color = Color.Blue;
        }

        void mapControl_OnPolygonEnter(GMapPolygon item)
        {
            currentPolygon = item;
            item.Stroke.Color = Color.Red;
        }

        void mapControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == System.Windows.Forms.MouseButtons.Left)
            //{
            //    PointLatLng point = mapControl.FromLocalToLatLng(e.X, e.Y);
            //    GeoCoderStatusCode statusCode = GeoCoderStatusCode.Unknow;
            //    Placemark? place = gp.GetPlacemark(point, out statusCode);
            //    if (statusCode == GeoCoderStatusCode.G_GEO_SUCCESS)
            //    {
            //        GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.green);
            //        marker.ToolTipText = place.Value.Address;
            //        marker.ToolTipMode = MarkerTooltipMode.Always;

            //        locations.Markers.Add(marker);
            //    }
            //}

            PointLatLng point = mapControl.FromLocalToLatLng(e.X, e.Y);

            //GMapMarkerCircle mc = new GMapMarkerCircle(point);
            //objects.Markers.Add(mc);

            //GMapPolygon circle = CirclePolygon.CreateCircle(point, 1, "my circle");
            //objects.Polygons.Add(circle);

            //GMapPolygon sector = CirclePolygon.CreateSector(point, 1, 25, 90);
            //objects.Polygons.Add(sector);

            //GMapMarkerCircleOffline cc = new GMapMarkerCircleOffline(point, 1);
            //markersOverlay.Markers.Add(cc);

        }

        void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && isLeftButtonDown)
            {
                if (currentMarker != null)
                {
                    PointLatLng point = mapControl.FromLocalToLatLng(e.X, e.Y);
                    currentMarker.Position = point;
                }
            }
        }

        void mapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                isLeftButtonDown = false;
            }
        }

        void mapControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                isLeftButtonDown = true;
            }
        }

        void mapControl_OnMarkerLeave(GMapMarker item)
        {
            currentMarker = null;
        }

        void mapControl_OnMarkerEnter(GMapMarker item)
        {
            currentMarker = item;
        }

        void mapControl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.contextMenuStrip1.Show(Cursor.Position);
                if (item is GMapMarkerFlash)
                {
                    currentMarker = item as GMapMarkerFlash;
                }
            }
        }

        void mapControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                PointLatLng point = mapControl.FromLocalToLatLng(e.X, e.Y);
                
                //GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.green);
                //markersOverlay.Markers.Add(marker);

                //Bitmap bitmap = Properties.Resources.point_blue;
                //GMapMarker marker = new GMapMarkerFlash(point, bitmap);
                //markersOverlay.Markers.Add(marker);

                //GifImage gif = new GifImage(@"F:\GMap\GMapDemo\GMapDemo\Resources\your_sister.gif");
                GifImage gif = new GifImage(Properties.Resources.your_sister);
                GMapMarkerAnimation ani = new GMapMarkerAnimation(point, gif);
                markersOverlay.Markers.Add(ani);

                //GMapMarker marker = new GMapMarkerDirection(point, Properties.Resources.arrow, 45);
                //objects.Markers.Add(marker);

                //GMapMarker marker = new GMapMarkerTip(point, bitmap, "图标A");
                //objects.Markers.Add(marker);
            }
        }

        private void buttonBeginBlink_Click(object sender, EventArgs e)
        {
            foreach (GMapMarker m in markersOverlay.Markers)
            {
                if (m is GMapMarkerFlash)
                {
                    GMapMarkerFlash marker = m as GMapMarkerFlash;
                    marker.StartFlash();
                }
            }
        }

        private void buttonStopBlink_Click(object sender, EventArgs e)
        {
            foreach (GMapMarker m in markersOverlay.Markers)
            {
                if (m is GMapMarkerFlash)
                {
                    GMapMarkerFlash marker = m as GMapMarkerFlash;
                    marker.StopFlash();
                }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            searchResult.Clear();
            locations.Markers.Clear();
            this.comboBoxSearchResult.Items.Clear();

            string searchStr = this.textBoxSearch.Text;
            GeoCoderStatusCode statusCode = gp.GetPoints(searchStr, out searchResult);
            if (statusCode == GeoCoderStatusCode.G_GEO_SUCCESS)
            {
                foreach (PointLatLng point in searchResult)
                {
                    GMarkerGoogle marker = new GMarkerGoogle(point, GMarkerGoogleType.arrow);

                    GeoCoderStatusCode placeMarkResult = new GeoCoderStatusCode();
                    Placemark? place = gp.GetPlacemark(point, out placeMarkResult);
                    locations.Markers.Add(marker);
                    this.comboBoxSearchResult.Items.Add(place.Value.Address);
                }
                mapControl.ZoomAndCenterMarkers(locations.Id);
            }
        }

        private void comboBoxSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxSearchResult.SelectedIndex < 0)
            {
                return;
            }
            locations.Clear();
            GMarkerGoogle marker = new GMarkerGoogle(searchResult[this.comboBoxSearchResult.SelectedIndex], GMarkerGoogleType.red);
            locations.Markers.Add(marker);
            mapControl.Position = this.searchResult[this.comboBoxSearchResult.SelectedIndex];
        }

        private void buttonSetStart_Click(object sender, EventArgs e)
        {
            if (currentMarker != null)
            {
                start = currentMarker.Position;
            }
        }

        private void buttonSetEnd_Click(object sender, EventArgs e)
        {
            if (currentMarker != null)
            {
                end = currentMarker.Position;
            }
        }

        private void buttonFindRoute_Click(object sender, EventArgs e)
        {
            RoutingProvider rp = mapControl.MapProvider as RoutingProvider;
            if (rp == null)
            {
                rp = GMapProviders.OpenStreetMap; // use OpenStreetMap if provider does not implement routing
            }

            MapRoute route = rp.GetRoute(start, end, false, false, (int)mapControl.Zoom);
            if (route != null)
            {
                // add route
                GMapRoute r = new GMapRoute(route.Points, route.Name);
                r.IsHitTestVisible = true;
                locations.Routes.Add(r);

                // add route start/end marks
                GMapMarker m1 = new GMarkerGoogle(start, GMarkerGoogleType.green_big_go);
                m1.ToolTipText = "Start: " + route.Name;
                m1.ToolTipMode = MarkerTooltipMode.Always;

                GMapMarker m2 = new GMarkerGoogle(end, GMarkerGoogleType.red_big_stop);
                m2.ToolTipText = "End: " + end.ToString();
                m2.ToolTipMode = MarkerTooltipMode.Always;

                markersOverlay.Markers.Add(m1);
                markersOverlay.Markers.Add(m2);

                mapControl.ZoomAndCenterRoute(r);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentMarker != null)
            {
                if (markersOverlay.Markers.Contains(currentMarker))
                {
                    markersOverlay.Markers.Remove(currentMarker);
                    currentMarker.Dispose();
                }
            }
        }

        private void 谷歌地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapProviderType != MapProviderType.google)
            {
                mapProviderType = MapProviderType.google;
                mapControl.MapProvider = GMapProviders.GoogleChinaMap;
                mapType = MapType.Common;
                this.buttonMapType.Image = Properties.Resources.weixing;
                this.谷歌地图ToolStripMenuItem.Enabled = false;
                this.百度地图ToolStripMenuItem.Enabled = true;
                this.高德地图ToolStripMenuItem.Enabled = true;
                this.腾讯地图ToolStripMenuItem.Enabled = true;
            }
        }

        private void 高德地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapProviderType != MapProviderType.amap)
            {
                mapProviderType = MapProviderType.amap;
                mapControl.MapProvider = GMapProvidersExt.AMapProvider.Instance;
                mapType = MapType.Common;
                this.buttonMapType.Image = Properties.Resources.weixing;
                this.高德地图ToolStripMenuItem.Enabled = false;
                this.谷歌地图ToolStripMenuItem.Enabled = true;
                this.百度地图ToolStripMenuItem.Enabled = true;
                this.腾讯地图ToolStripMenuItem.Enabled = true;
            }
        }

        private void 腾讯地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapProviderType != MapProviderType.tengxun)
            {
                mapProviderType = MapProviderType.tengxun;
                mapControl.MapProvider = GMapProvidersExt.SosoMapProvider.Instance;
                mapType = MapType.Common;
                this.buttonMapType.Image = Properties.Resources.weixing;
                this.腾讯地图ToolStripMenuItem.Enabled = false;
                this.高德地图ToolStripMenuItem.Enabled = true;
                this.谷歌地图ToolStripMenuItem.Enabled = true;
                this.百度地图ToolStripMenuItem.Enabled = true;
            }
        }

        private void 百度地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapProviderType != MapProviderType.baidu)
            {
                mapProviderType = MapProviderType.baidu;
                mapControl.MapProvider = GMapProvidersExt.BaiduMapProvider.Instance;
                mapType = MapType.Common;
                this.buttonMapType.Image = Properties.Resources.weixing;
                this.谷歌地图ToolStripMenuItem.Enabled = true;
                this.百度地图ToolStripMenuItem.Enabled = false;
                this.高德地图ToolStripMenuItem.Enabled = true;
                this.腾讯地图ToolStripMenuItem.Enabled = true;
            }
        }

        private void buttonMapType_Click(object sender, EventArgs e)
        {
            if (mapType == MapType.Common && mapProviderType == MapProviderType.google)
            {
                mapType = MapType.Satellite;
                this.buttonMapType.Image = Properties.Resources.ditu;
                mapControl.MapProvider = GMapProviders.GoogleChinaSatelliteMap;
            }
            else if (mapType == MapType.Satellite && mapProviderType == MapProviderType.google)
            {
                mapType = MapType.Common;
                this.buttonMapType.Image = Properties.Resources.weixing;
                mapControl.MapProvider = GMapProviders.GoogleChinaMap;
            }
            else if (mapType == MapType.Common && mapProviderType == MapProviderType.baidu)
            {
                mapType = MapType.Satellite;
                this.buttonMapType.Image = Properties.Resources.ditu;
                mapControl.MapProvider = GMapProvidersExt.BaiduMapSateliteProvider.Instance;
            }
            else if (mapType == MapType.Satellite && mapProviderType == MapProviderType.baidu)
            {
                mapType = MapType.Common;
                this.buttonMapType.Image = Properties.Resources.weixing;
                mapControl.MapProvider = GMapProvidersExt.BaiduMapProvider.Instance;
            }
            else if (mapType == MapType.Common && mapProviderType == MapProviderType.amap)
            {
                mapType = MapType.Satellite;
                this.buttonMapType.Image = Properties.Resources.ditu;
                mapControl.MapProvider = GMapProvidersExt.AMapSateliteProvider.Instance;
            }
            else if (mapType == MapType.Satellite && mapProviderType == MapProviderType.amap)
            {
                mapType = MapType.Common;
                this.buttonMapType.Image = Properties.Resources.weixing;
                mapControl.MapProvider = GMapProvidersExt.AMapProvider.Instance;
            }
            else if (mapType == MapType.Common && mapProviderType == MapProviderType.tengxun)
            {
                mapType = MapType.Satellite;
                this.buttonMapType.Image = Properties.Resources.ditu;
                mapControl.MapProvider = GMapProvidersExt.SosoMapSateliteProvider.Instance;
            }
            else if (mapType == MapType.Satellite && mapProviderType == MapProviderType.tengxun)
            {
                mapType = MapType.Common;
                this.buttonMapType.Image = Properties.Resources.weixing;
                mapControl.MapProvider = GMapProvidersExt.SosoMapProvider.Instance;
            }
        }

        private void 保存为图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "PNG (*.png)|*.png";
                    dialog.FileName = "GMap.NET image";
                    Image image = this.mapControl.ToImage();
                    if (image != null)
                    {
                        using (image)
                        {
                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                string fileName = dialog.FileName;
                                if (!fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                                {
                                    fileName += ".png";
                                }
                                image.Save(fileName);
                                MessageBox.Show("图片已保存： " + dialog.FileName, "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("图片保存失败： " + exception.Message, "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void 保存缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.ShowExportDialog();
        }

        private void 读取缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.ShowImportDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //drawingMode = DrawingMode.Circle;
            //this.mapControl.CanDragMap = false;
            draw.DrawingMode = DrawingMode.Circle;
            draw.IsEnable = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //drawingMode = DrawingMode.Rectangle;
            //this.mapControl.CanDragMap = false;
            draw.DrawingMode = DrawingMode.Rectangle;
            draw.IsEnable = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            draw.DrawingMode = DrawingMode.Polygon;
            draw.IsEnable = true;
            
            //drawingMode = DrawingMode.Polygon;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (polygonsOverlay != null)
            {
                polygonsOverlay.Polygons.Clear();
                polygonsOverlay.Markers.Clear();
            }
        }
    }
}
