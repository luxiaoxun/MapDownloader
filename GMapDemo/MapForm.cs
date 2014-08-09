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
using GMapDrawTools;
using GMapTools;
using Microsoft.SqlServer.Server;
using NetUtilityLib;
using System.Reflection;
using System.Diagnostics;
using System.IO;

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

        private DrawDistance drawDistance;

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
            InitChinaRegion();

            draw = new Draw(this.mapControl);
            draw.DrawComplete += new EventHandler<DrawEventArgs>(draw_DrawComplete);

            drawDistance = new DrawDistance(this.mapControl);
            drawDistance.DrawComplete += new EventHandler<DrawDistanceEventArgs>(drawDistance_DrawComplete);
        }

        private void InitUI()
        {
            this.buttonMapType.Image = Properties.Resources.weixing;
            this.buttonMapType.Click +=new EventHandler(buttonMapType_Click);

            mapType = MapType.Common;
            mapProviderType = MapProviderType.google;

            this.panelMap.SizeChanged += new EventHandler(panelMap_SizeChanged);
            List<string> regionNames = GMapChinaRegion.MapRegion.GetAllRegionName();
            foreach (var regionName in regionNames)
            {
                this.comboBoxRegion.Items.Add(regionName);
               
            }
            this.comboBoxRegion.SelectedValueChanged += new EventHandler(comboBoxRegion_SelectedValueChanged);

            this.checkBoxTileHost.CheckedChanged += new EventHandler(checkBoxTileHost_CheckedChanged);
        }

        # region Leafletjs web demo

        bool TryExtractLeafletjs()
        {
            try
            {
                string launch = string.Empty;

                var x = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                foreach (var f in x)
                {
                    if (f.Contains("leafletjs"))
                    {
                        var fName = f.Replace("GMapWinFormDemo.", string.Empty);
                        fName = fName.Replace(".", "\\");
                        var ll = fName.LastIndexOf("\\");
                        var name = fName.Substring(0, ll) + "." + fName.Substring(ll + 1, fName.Length - ll - 1);

                        //Demo.WindowsForms.leafletjs.dist.leaflet.js

                        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(f))
                        {
                            string fileFullPath = mapControl.CacheLocation + name;

                            if (fileFullPath.Contains("gmap.html"))
                            {
                                launch = fileFullPath;
                            }

                            var dir = Path.GetDirectoryName(fileFullPath);
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }

                            using (FileStream fileStream = System.IO.File.Create(fileFullPath, (int)stream.Length))
                            {
                                // Fill the bytes[] array with the stream data
                                byte[] bytesInStream = new byte[stream.Length];
                                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);

                                // Use FileStream object to write to the specified file
                                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(launch))
                {
                    System.Diagnostics.Process.Start(launch);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("TryExtractLeafletjs: " + ex);
                return false;
            }
            return true;
        }

        // http://leafletjs.com/
        // Leaflet is a modern open-source JavaScript library for mobile-friendly interactive maps
        // Leaflet is designed with simplicity, performance and usability in mind. It works efficiently across all major desktop and mobile platforms out of the box
        void checkBoxTileHost_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTileHost.Checked)
            {
                try
                {
                    mapControl.Manager.EnableTileHost(8844);
                    TryExtractLeafletjs();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("EnableTileHost: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                mapControl.Manager.DisableTileHost();
            }
        }

        #endregion 

        #region china region

        private void InitChinaRegion()
        {
            TreeNode rootNode = new TreeNode("中国");
            this.treeView1.Nodes.Add(rootNode);
            //Country china = GetCountryDataFromFile(@"F:\GMap\china.xml");
            //string file = System.Windows.Forms.Application.StartupPath + "\\china";
            //string file = System.Windows.Forms.Application.StartupPath + "\\china-province city";
            //Country china = GMapChinaRegion.ChinaMapRegion.GetChinaRegionFromJsonFile(file);
            string file = System.Windows.Forms.Application.StartupPath + "\\china-province city.xml";
            Country china = GMapChinaRegion.ChinaMapRegion.GetChinaRegionFromXmlFile(file);
            foreach (var provice in china.Province)
            {
                TreeNode pNode = new TreeNode(provice.name);
                pNode.Tag = provice;
                foreach (var city in provice.City)
                {
                    TreeNode cNode = new TreeNode(city.name);
                    cNode.Tag = city;
                    //foreach (var piecearea in city.Piecearea)
                    //{
                    //    TreeNode areaNode = new TreeNode(piecearea.name);
                    //    areaNode.Tag = piecearea;
                    //    cNode.Nodes.Add(areaNode);
                    //}
                    pNode.Nodes.Add(cNode);
                }
                rootNode.Nodes.Add(pNode);
            }

            this.treeView1.CheckBoxes = true;

            //this.treeView1.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseDoubleClick);
            this.treeView1.AfterCheck += new TreeViewEventHandler(treeView1_AfterCheck);
        }

        void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked)
            {
                string name = e.Node.Text;
                string rings = null;
                switch (e.Node.Level)
                {
                    case 0:
                        break;
                    case 1:
                        Province province = e.Node.Tag as Province;
                        name = province.name;
                        rings = province.rings;
                        break;
                    case 2:
                        City city = e.Node.Tag as City;
                        name = city.name;
                        rings = city.rings;
                        break;
                }
                if (rings != null)
                {
                    GMapPolygon polygon = ChinaMapRegion.GetRegionPolygon(name, rings);
                    if (polygon != null)
                    {
                        //regionOverlay.Polygons.Clear();
                        regionOverlay.Polygons.Add(polygon);
                        RectLatLng rect = GMapChinaRegion.ChinaMapRegion.GetRegionMaxRect(polygon);
                        this.mapControl.SetZoomToFitRect(rect);
                    }
                }
            }
            else
            {
                string name = e.Node.Text;
                for (int i = regionOverlay.Polygons.Count-1; i >=0; --i)
                {
                    if (regionOverlay.Polygons[i].Name == name)
                    {
                        regionOverlay.Polygons.RemoveAt(i);
                    }
                }
            }
        }

        void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                string name = e.Node.Text;
                string rings = null;
                switch (e.Node.Level)
                {
                    case 0:
                        break;
                    case 1:
                        Province province = e.Node.Tag as Province;
                        name = province.name;
                        rings = province.rings;
                        break;
                    case 2:
                        City city = e.Node.Tag as City;
                        name = city.name;
                        rings = city.rings;
                        break;
                }
                if (rings != null)
                {
                    GMapPolygon polygon = ChinaMapRegion.GetRegionPolygon(name, rings);
                    if (polygon != null)
                    {
                        regionOverlay.Polygons.Clear();
                        regionOverlay.Polygons.Add(polygon);
                        RectLatLng rect = GMapChinaRegion.ChinaMapRegion.GetRegionMaxRect(polygon);
                        this.mapControl.SetZoomToFitRect(rect);
                    }
                }
            }
        }

        void comboBoxRegion_SelectedValueChanged(object sender, EventArgs e)
        {
            string selectedName = this.comboBoxRegion.GetItemText(this.comboBoxRegion.SelectedItem);
            GMapPolygon p = GMapChinaRegion.MapRegion.CreateMapPolygon(selectedName);
            if (p != null)
            {
                regionOverlay.Polygons.Clear();
                regionOverlay.Polygons.Add(p);
                RectLatLng rect = GMapChinaRegion.MapRegion.GetRegionMaxRect(p);
                this.mapControl.SetZoomToFitRect(rect);
            }
        }

        #endregion

        void panelMap_SizeChanged(object sender, EventArgs e)
        {
            this.buttonMapType.Location = new Point(
                this.panelMenu.Location.X + panelMap.Width - 80, 
                this.panelMenu.Location.Y);
            this.comboBoxRegion.Location = new Point(this.menuStrip1.Location.X + menuStrip1.Width - 100, this.menuStrip1.Location.Y);
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

            //GMapMarkerCircle mc = new GMapMarkerCircle(point,200);
            //markersOverlay.Markers.Add(mc);

            //GMapPolygon circle = CirclePolygon.CreateCircle(point, 1, "my circle");
            //markersOverlay.Polygons.Add(circle);

            //GMapPolygon sector = CirclePolygon.CreateSector(point, 1, 25, 90);
            //markersOverlay.Polygons.Add(sector);

            //GMapMarkerCircleOffline cc = new GMapMarkerCircleOffline(point, 0.01);
            //markersOverlay.Markers.Add(cc);

        }

        void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            PointLatLng point = mapControl.FromLocalToLatLng(e.X, e.Y);
            this.toolStripStatusLabelCurrentPos.Text = "当前坐标："+point.ToString();

            if (e.Button == System.Windows.Forms.MouseButtons.Left && isLeftButtonDown)
            {
                if (currentMarker != null && currentMarker is GMapFlashMarker)
                {
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

                //GifImage gif = new GifImage(Properties.Resources.your_sister);
                //GMapMarkerAnimation ani = new GMapMarkerAnimation(point, gif);
                //markersOverlay.Markers.Add(ani);

                //GMapMarker marker = new GMapMarkerDirection(point, Properties.Resources.arrow, 45);
                //objects.Markers.Add(marker);

                //GMapMarker marker = new GMapMarkerTip(point, bitmap, "图标A");
                //objects.Markers.Add(marker);
            }
        }

        #region Marker操作

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
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(Cursor.Position);
                if (item is GMapFlashMarker)
                {
                    currentMarker = item as GMapFlashMarker;
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                if (item is DrawDeleteMarker)
                {
                    currentMarker = item as DrawDeleteMarker;

                    GMapOverlay overlay = currentMarker.Overlay;
                    if (overlay.Markers.Contains(currentMarker))
                    {
                        overlay.Markers.Remove(currentMarker);
                    }

                    if (this.mapControl.Overlays.Contains(overlay))
                    {
                        this.mapControl.Overlays.Remove(overlay);
                    }
                }
            }
        }

        private void buttonBeginBlink_Click(object sender, EventArgs e)
        {
            foreach (GMapMarker m in markersOverlay.Markers)
            {
                if (m is GMapFlashMarker)
                {
                    GMapFlashMarker marker = m as GMapFlashMarker;
                    marker.StartFlash();
                }
            }
        }

        private void buttonStopBlink_Click(object sender, EventArgs e)
        {
            foreach (GMapMarker m in markersOverlay.Markers)
            {
                if (m is GMapFlashMarker)
                {
                    GMapFlashMarker marker = m as GMapFlashMarker;
                    marker.StopFlash();
                }
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

        #endregion

        #region 地址解析与路径查找

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

        #endregion

        #region 地图选择

        private void 谷歌地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapProviderType != MapProviderType.google)
            {
                mapProviderType = MapProviderType.google;
                mapControl.MapProvider = GMapProviders.GoogleChinaMap;
                mapType = MapType.Common;
                this.buttonMapType.Image = Properties.Resources.weixing;
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
            else if (mapType == MapType.Common && mapProviderType == MapProviderType.bing)
            {
                mapType = MapType.Satellite;
                this.buttonMapType.Image = Properties.Resources.ditu;
                mapControl.MapProvider = GMapProviders.BingSatelliteMap;
            }
            else if (mapType == MapType.Satellite && mapProviderType == MapProviderType.bing)
            {
                mapType = MapType.Common;
                this.buttonMapType.Image = Properties.Resources.weixing;
                mapControl.MapProvider = GMapProviders.BingMap;
            }
        }

        private void 必应地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mapProviderType != MapProviderType.bing)
            {
                mapProviderType = MapProviderType.bing;
                mapControl.MapProvider = GMapProviders.BingMap;
                mapType = MapType.Common;
                this.buttonMapType.Image = Properties.Resources.weixing;
            }
        }

        #endregion

        #region 地图操作

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

        private void buttonPrefetch_Click(object sender, EventArgs e)
        {
            RectLatLng area = mapControl.SelectedArea;
            if (!area.IsEmpty)
            {
                for (int i = (int)mapControl.Zoom; i <= mapControl.MaxZoom; i++)
                {
                    DialogResult res = MessageBox.Show("Ready ripp at Zoom = " + i + " ?", "GMap.NET", MessageBoxButtons.YesNoCancel);

                    if (res == DialogResult.Yes)
                    {
                        using (TilePrefetcher obj = new TilePrefetcher())
                        {
                            obj.Overlay = markersOverlay; // set overlay if you want to see cache progress on the map
                            obj.Shuffle = mapControl.Manager.Mode != AccessMode.CacheOnly;
                            obj.Owner = this;
                            obj.ShowCompleteMessage = true;
                            obj.Start(area, i, mapControl.MapProvider, mapControl.Manager.Mode == AccessMode.CacheOnly ? 0 : 100, mapControl.Manager.Mode == AccessMode.CacheOnly ? 0 : 1);
                        }
                    }
                    else if (res == DialogResult.No)
                    {
                        continue;
                    }
                    else if (res == DialogResult.Cancel)
                    {
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select map area holding ALT", "GMap.NET", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion

        #region 画图操作

        void draw_DrawComplete(object sender, DrawEventArgs e)
        {
            if (e != null && (e.Polygon != null || e.Circle != null || e.Route != null))
            {
                switch (e.DrawingMode)
                {
                    case DrawingMode.Polygon:
                        polygonsOverlay.Polygons.Add(e.Polygon);
                        break;
                    case DrawingMode.Rectangle:
                        polygonsOverlay.Polygons.Add(e.Polygon);
                        break;
                    case DrawingMode.Circle:
                        polygonsOverlay.Markers.Add(e.Circle);
                        break;
                    case DrawingMode.Route:
                        polygonsOverlay.Routes.Add(e.Route);
                        break;
                    default:
                        draw.IsEnable = false;
                        break;
                }
            }
            draw.IsEnable = false;
        }

        private void buttonCircle_Click(object sender, EventArgs e)
        {
            draw.DrawingMode = DrawingMode.Circle;
            draw.IsEnable = true;
        }

        private void buttonRectangle_Click(object sender, EventArgs e)
        {
            draw.DrawingMode = DrawingMode.Rectangle;
            draw.IsEnable = true;
        }

        private void buttonPolygon_Click(object sender, EventArgs e)
        {
            draw.DrawingMode = DrawingMode.Polygon;
            draw.IsEnable = true;
        }

        private void buttonPolyline_Click(object sender, EventArgs e)
        {
            draw.DrawingMode = DrawingMode.Route;
            draw.IsEnable = true;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (polygonsOverlay != null)
            {
                polygonsOverlay.Polygons.Clear();
                polygonsOverlay.Markers.Clear();
                polygonsOverlay.Routes.Clear();
            }
        }

        #endregion

        #region 测距

        void drawDistance_DrawComplete(object sender, DrawDistanceEventArgs e)
        {
            if (e != null)
            {
                GMapOverlay distanceOverlay = new GMapOverlay();
                this.mapControl.Overlays.Add(distanceOverlay);
                foreach (LineMarker line in e.LineMarkers)
                {
                    distanceOverlay.Markers.Add(line);
                }
                foreach (DrawDistanceMarker marker in e.DistanceMarkers)
                {
                    distanceOverlay.Markers.Add(marker);
                }
                distanceOverlay.Markers.Add(e.DistanceDeleteMarker);
            }
            drawDistance.IsEnable = false;
        }

        private void buttonDistance_Click(object sender, EventArgs e)
        {
            if (drawDistance != null)
            {
                drawDistance.IsEnable = true;
            }
        }

        #endregion

        private void buttonClearSArea_Click(object sender, EventArgs e)
        {
            if (this.mapControl != null)
            {
                this.mapControl.SelectedArea = GMap.NET.RectLatLng.Empty;
            }
        }
        
    }
}
