using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.CacheProviders;
using GMapDrawTools;
using NetUtilityLib;
using GMapChinaRegion;
using MySql.Data.MySqlClient;
using log4net;
using GMapPolygonLib;
using GMapMarkerLib;
using System.Reflection;

namespace MapDownloader
{
    public partial class MainForm : DevComponents.DotNetBar.Office2007Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));
        //private string conString = @"Server=127.0.0.1;Port=3306;Database=mapcache;Uid=root;Pwd=admin;";
        private static string conStringFormat = "Server={0};Port={1};Database={2};Uid={3};Pwd={4};";
        private static string conString;

        private Draw draw;
        private GMapOverlay polygonsOverlay = new GMapOverlay("polygonsOverlay"); //放置polygon的图层
        private GMapDrawRectangle rectangle;

        //中国省市边界
        private Country china;
        private bool isCountryLoad = false;
        private GMapOverlay regionOverlay = new GMapOverlay("region");

        private RectLatLng selectedRect = RectLatLng.Empty;

        private int retryNum = 3;
        private string tilePath = "D:\\GisMap";
        private SQLitePureImageCache sqliteCache = new SQLitePureImageCache();
        private MySQLPureImageCache mysqlCache = new MySQLPureImageCache();
        //private PrefetchTiles prefetchTiles = null;

        private bool isLeftButtonDown = false;
        private GMapMarkerEllipse currentDragableNode = null;
        private List<GMapMarkerEllipse> currentDragableNodes;
        private GMapAreaPolygon currentDragableRoute;

        public MainForm()
        {
            InitializeComponent();

            InitMap();

            InitUI();

            InitMySQLConString();
        }

        //初始化地图
        private void InitMap()
        {
            mapControl.ShowCenter = false;
            mapControl.DragButton = System.Windows.Forms.MouseButtons.Left;
            mapControl.CacheLocation = Environment.CurrentDirectory + "\\MapCache\\"; //缓存位置

            //mapControl.MapProvider = GMapProviders.GoogleChinaMap;
            mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduMapProvider.Instance;
            mapControl.Position = new PointLatLng(32.043, 118.773);
            mapControl.MinZoom = 1;
            mapControl.MaxZoom = 18;
            mapControl.Zoom = 9;

            mapControl.Overlays.Add(polygonsOverlay);
            mapControl.Overlays.Add(regionOverlay);

            this.mapControl.MouseMove += new MouseEventHandler(mapControl_MouseMove);
            this.mapControl.MouseDown += new MouseEventHandler(mapControl_MouseDown);
            this.mapControl.MouseUp += new MouseEventHandler(mapControl_MouseUp);
            this.mapControl.OnMarkerEnter += new MarkerEnter(mapControl_OnMarkerEnter);
            this.mapControl.OnMarkerLeave += new MarkerLeave(mapControl_OnMarkerLeave);
            this.mapControl.OnPolygonClick += new PolygonClick(mapControl_OnPolygonClick);
            this.mapControl.OnPolygonEnter += new PolygonEnter(mapControl_OnPolygonEnter);
            this.mapControl.OnPolygonLeave += new PolygonLeave(mapControl_OnPolygonLeave);

            draw = new Draw(this.mapControl);
            draw.DrawComplete += new EventHandler<DrawEventArgs>(draw_DrawComplete);
        }

        #region 地图控件事件

        void mapControl_OnMarkerLeave(GMapMarker item)
        {
            if (!isLeftButtonDown)
            {
                if (item is GMapMarkerEllipse)
                {
                    currentDragableNode = null;
                }
            }
        }

        void mapControl_OnMarkerEnter(GMapMarker item)
        {
            if (!isLeftButtonDown)
            {
                if (item is GMapMarkerEllipse)
                {
                    currentDragableNode = item as GMapMarkerEllipse;
                }
            }
        }

        void mapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isLeftButtonDown = false;
                currentDragableNode = null;
            }
        }

        void mapControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isLeftButtonDown = true;
            }
        }

        //Mouse move 事件
        void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                PointLatLng p = this.mapControl.FromLocalToLatLng(e.X, e.Y);

                int zoom = (int)this.mapControl.Zoom;
                double resolution = this.mapControl.MapProvider.Projection.GetGroundResolution(zoom, this.mapControl.Position.Lat);

                this.toolStripStatusTip.Text = string.Format("显示级别：{0} 分辨率：{1:F3}米/像素 坐标：{2:F4},{3:F4}", zoom, resolution, p.Lng, p.Lat);

                if (isLeftButtonDown && currentDragableNode != null)
                {
                    int? tag = (int?)this.currentDragableNode.Tag;
                    if (tag.HasValue && this.currentDragableRoute != null)
                    {
                        int? nullable2 = tag;
                        int count = this.currentDragableRoute.Points.Count;
                        if (nullable2.GetValueOrDefault() < count)
                        {
                            this.currentDragableRoute.Points[tag.Value] = p;
                            this.mapControl.UpdatePolygonLocalPosition(this.currentDragableRoute);
                        }
                    }
                    this.currentDragableNode.Position = p;
                    this.currentDragableNode.ToolTipText = string.Format("X={0} Y={1}", p.Lng.ToString("0.0000"), p.Lat.ToString("0.0000"));
                    this.currentDragableNode.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                    this.mapControl.UpdateMarkerLocalPosition(this.currentDragableNode);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        void mapControl_OnPolygonLeave(GMapPolygon item)
        {
            if (item is GMapAreaPolygon)
            {
                GMapAreaPolygon areaPolygon = item as GMapAreaPolygon;
                if (currentDragableRoute != null && currentDragableRoute == areaPolygon)
                {
                    currentDragableRoute = item as GMapAreaPolygon;
                    currentDragableRoute.Stroke.Color = Color.Blue;
                }
            }

        }

        void mapControl_OnPolygonEnter(GMapPolygon item)
        {
            if (item is GMapAreaPolygon)
            {
                GMapAreaPolygon areaPolygon = item as GMapAreaPolygon;
                if (currentDragableRoute != null && currentDragableRoute == areaPolygon)
                {
                    currentDragableRoute = item as GMapAreaPolygon;
                    currentDragableRoute.Stroke.Color = Color.Red;
                }
            }
        }

        void mapControl_OnPolygonClick(GMapPolygon item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (item is GMapAreaPolygon && currentDragableRoute != null)
                {
                    this.contextMenuStripSelectedArea.Show(Cursor.Position);
                }
            }
        }

        #endregion

        //初始化UI
        private void InitUI()
        {
            ShowDownloadTip(false);
            this.serverAndCacheToolStripMenuItem.Checked = true;
            this.xPanderPanel2.ExpandClick += new EventHandler<EventArgs>(xPanderPanel2_ExpandClick);

            this.radioButtonMySQL.CheckedChanged += new EventHandler(radioButtonMySQL_CheckedChanged);
            this.radioButtonSQLite.CheckedChanged += new EventHandler(radioButtonSQLite_CheckedChanged);
            this.radioButtonDisk.CheckedChanged += new EventHandler(radioButtonDisk_CheckedChanged);

            this.buttonDownload.Click += new EventHandler(buttonDownload_Click);
            this.buttonMapImage.Click += new EventHandler(buttonMapImage_Click);

            this.checkBoxItemShowGrid.CheckedChanged += new DevComponents.DotNetBar.CheckBoxChangeEventHandler(checkBoxItemShowGrid_CheckedChanged);
            this.checkBoxCacheServer.CheckedChanged += new DevComponents.DotNetBar.CheckBoxChangeEventHandler(checkBoxCacheServer_CheckedChanged);
        }

        #region 离线服务

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
                        var fName = f.Replace("MapDownloader.", string.Empty);
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
                log.Error("TryExtractLeafletjs: " + ex);
                return false;
            }
            return true;
        }

        void checkBoxCacheServer_CheckedChanged(object sender, DevComponents.DotNetBar.CheckBoxChangeEventArgs e)
        {
            if (checkBoxCacheServer.Checked)
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

        #region 加载中国区域

        void xPanderPanel2_ExpandClick(object sender, EventArgs e)
        {
            if (!isCountryLoad)
            {
                InitChinaRegion();
                isCountryLoad = true;
            }
        }

        private void InitChinaRegion()
        {
            DevComponents.AdvTree.Node rootNode = new DevComponents.AdvTree.Node("中国");
            this.advTreeChina.Nodes.Add(rootNode);
            rootNode.Expand();

            //异步加载中国省市边界
            BackgroundWorker loadChinaWorker = new BackgroundWorker();
            loadChinaWorker.DoWork += new DoWorkEventHandler(loadChinaWorker_DoWork);
            loadChinaWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadChinaWorker_RunWorkerCompleted);
            loadChinaWorker.RunWorkerAsync();
        }

        void loadChinaWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (china == null)
            {
                log.Warn("加载中国省市边界失败！");
                return;
            }

            try
            {
                foreach (var provice in china.Province)
                {
                    DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node(provice.name);
                    pNode.Tag = provice;
                    //pNode.ContextMenuStrip = this.contextMenuStripRegion;
                    foreach (var city in provice.City)
                    {
                        DevComponents.AdvTree.Node cNode = new DevComponents.AdvTree.Node(city.name);
                        cNode.Tag = city;
                        //cNode.ContextMenuStrip = this.contextMenuStripRegion;
                        pNode.Nodes.Add(cNode);
                    }
                    DevComponents.AdvTree.Node rootNode = this.advTreeChina.Nodes[0];
                    rootNode.Nodes.Add(pNode);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            this.advTreeChina.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(advTreeChina_NodeClick);

        }

        void advTreeChina_NodeClick(object sender, DevComponents.AdvTree.TreeNodeMouseEventArgs e)
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
                        GMapAreaPolygon areaPolygon = new GMapAreaPolygon(polygon.Points, name);
                        currentDragableRoute = areaPolygon;
                        regionOverlay.Polygons.Clear();
                        regionOverlay.Polygons.Add(areaPolygon);
                        RectLatLng rect = GMapUtil.PolygonUtils.GetRegionMaxRect(polygon);
                        this.mapControl.SetZoomToFitRect(rect);
                        selectedRect = rect;
                    }
                }
            }
        }

        void loadChinaWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //string file = System.Windows.Forms.Application.StartupPath + "\\chinaBoundry";
            //if (System.IO.File.Exists(file))
            //{
            //    china = GMapChinaRegion.ChinaMapRegion.GetChinaRegionFromJsonFile(file);
            //}
            try
            {
                byte[] buffer = Properties.Resources.chinaBoundryBinary;
                china = GMapChinaRegion.ChinaMapRegion.GetChinaRegionFromJsonBinaryBytes(buffer);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion 

        //初始化App config配置信息
        private void InitMySQLConString()
        {
            try
            {
                string ip = ConfigHelper.GetAppConfig("MySQLServerIP");
                string port = ConfigHelper.GetAppConfig("MySQLServerPort");
                string dbName = ConfigHelper.GetAppConfig("Database");
                string userID = ConfigHelper.GetAppConfig("UserID");
                string password = ConfigHelper.GetAppConfig("Password");
                string retryStr = ConfigHelper.GetAppConfig("Retry");
                string center = ConfigHelper.GetAppConfig("MapCenter");
                string[] centerPoints = center.Split(',');
                if (centerPoints.Length == 2)
                {
                    if (mapControl != null)
                    {
                        mapControl.Position = new PointLatLng(double.Parse(centerPoints[1]), double.Parse(centerPoints[0]));
                    }
                }
                retryNum = int.Parse(retryStr);

                conString = string.Format(conStringFormat, ip, port, dbName, userID, password);
                if (mysqlCache != null)
                {
                    mysqlCache.ConnectionString = conString;
                }

                tilePath = ConfigHelper.GetAppConfig("TilePath");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        //是否显示网格
        void checkBoxItemShowGrid_CheckedChanged(object sender, DevComponents.DotNetBar.CheckBoxChangeEventArgs e)
        {
            if (this.checkBoxItemShowGrid.Checked)
            {
                this.mapControl.ShowTileGridLines = true;
            }
            else
            {
                this.mapControl.ShowTileGridLines = false;
            }
        }

        //画图完成函数
        void draw_DrawComplete(object sender, DrawEventArgs e)
        {
            try
            {
                if (e != null && (e.Polygon != null || e.Rectangle != null || e.Circle != null || e.Line != null))
                {
                    switch (e.DrawingMode)
                    {
                        case DrawingMode.Polygon:
                            polygonsOverlay.Polygons.Add(e.Polygon);
                            selectedRect = GetRectLatLngFromDrawing(e.Polygon);
                            break;
                        case DrawingMode.Rectangle:
                            polygonsOverlay.Polygons.Add(e.Rectangle);
                            selectedRect = GetRectLatLngFromDrawing(e.Rectangle);
                            break;
                        case DrawingMode.Circle:
                            polygonsOverlay.Markers.Add(e.Circle);
                            break;
                        case DrawingMode.Line:
                            polygonsOverlay.Routes.Add(e.Line);
                            break;
                        case DrawingMode.Route:
                            polygonsOverlay.Routes.Add(e.Route);
                            break;
                        default:
                            draw.IsEnable = false;
                            break;
                    }
                }
            }
            finally
            {
                draw.IsEnable = false;
            }
        }

        //从画图“矩形”得到矩形范围
        private RectLatLng GetRectLatLngFromDrawing(GMapPolygon rectangle)
        {
            RectLatLng rect = RectLatLng.Empty;
            if (rectangle != null && rectangle.Points.Count==4)
            {
                PointLatLng leftTop = rectangle.Points[0];
                PointLatLng rightBottom = rectangle.Points[2];
                rect = new RectLatLng(leftTop.Lat, leftTop.Lng, leftTop.Lat - rightBottom.Lat, rightBottom.Lng - leftTop.Lng);
            }
            return rect;
        }

        #region 存储方式

        void radioButtonSQLite_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonSQLite.Checked)
            {
                mapControl.Manager.PrimaryCache = sqliteCache;
            }
        }

        void radioButtonMySQL_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonMySQL.Checked)
            {
                mapControl.Manager.PrimaryCache = mysqlCache;
                //if (mysqlCache.Initialized)
                //{
                    
                //}
                //else
                //{
                //    MessageBox.Show("MySQL数据库连接错误！请检查配置是否正确？", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
        }

        void radioButtonDisk_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonDisk.Checked)
            {
                mapControl.Manager.PrimaryCache = sqliteCache;
            }
        }

        #endregion

        #region 地图下载

        void obj_PrefetchTileProgress(object sender, PrefetchTileEventArgs e)
        {
            if (e != null)
            {
                UpdateDownloadBar(e.ProgressValue,e.TileAllNum,e.TileCompleteNum,e.CurrentDownloadZoom);
            }
        }

        void obj_PrefetchTileStart(object sender, PrefetchTileEventArgs e)
        {
            ShowDownloadTip(true);
        }

        private void ShowDownloadTip(bool isVisible)
        {
            this.toolStripStatusDownload.Visible = isVisible;
            //this.toolStripProgressBarDownload.Visible = isVisible;
        }

        private void UpdateDownloadBar(int value, ulong allNum, ulong comNum, int zoom)
        {
            this.toolStripStatusDownload.Text = string.Format("下载进度：级别{0}，{1}/{2}",zoom,comNum,allNum);
        }

        void prefetchTiles_PrefetchTileComplete(object sender, PrefetchTileEventArgs e)
        {
            ShowDownloadTip(false);
            MessageBox.Show("地图下载完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void ResetToServerAndCacheMode()
        {
            this.mapControl.Manager.Mode = AccessMode.ServerAndCache;
            this.serverAndCacheToolStripMenuItem.Checked = true;
            this.本地缓存ToolStripMenuItem.Checked = false;
            this.在线服务ToolStripMenuItem.Checked = false;
        }

        private void DownloadMap()
        {
            RectLatLng area = selectedRect;
            if (!area.IsEmpty)
            {
                try
                {
                    int minZ = int.Parse(this.textBoxMinZoom.Text);
                    int maxZ = int.Parse(this.textBoxMaxZoom.Text);
                    minZ = minZ <= 0 ? 1 : minZ;
                    maxZ = maxZ >= mapControl.MaxZoom ? mapControl.MaxZoom : maxZ;
                    if (minZ <= maxZ)
                    {
                        ResetToServerAndCacheMode();
                        PrefetchTiles prefetchTiles = new PrefetchTiles();
                        prefetchTiles.Retry = retryNum;
                        prefetchTiles.PrefetchTileStart += new EventHandler<PrefetchTileEventArgs>(obj_PrefetchTileStart);
                        prefetchTiles.PrefetchTileProgress += new EventHandler<PrefetchTileEventArgs>(obj_PrefetchTileProgress);
                        prefetchTiles.PrefetchTileComplete += new EventHandler<PrefetchTileEventArgs>(prefetchTiles_PrefetchTileComplete);
                        if (this.radioButtonDisk.Checked)
                        {
                            //切片存在本地磁盘上
                            prefetchTiles.Start(area, minZ, maxZ, mapControl.MapProvider, 100, tilePath);
                        }
                        else
                        {
                            //切片存在数据库中
                            prefetchTiles.Start(area, minZ, maxZ, mapControl.MapProvider, 100);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //MessageBox.Show("请先用“矩形”画图工具选择区域", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CommonTools.PromptingMessage.PromptMessage(this, "请先用“矩形”画图工具选择区域");
            }
        }

        //下载地图
        private void buttonDownload_Click(object sender, EventArgs e)
        {
            DownloadMap();
        }

        #endregion

        #region 拼接大图

        void buttonMapImage_Click(object sender, EventArgs e)
        {
            RectLatLng area = selectedRect;
            if (!area.IsEmpty)
            {
                try
                {
                    ResetToServerAndCacheMode();
                    int zoom = int.Parse(this.textBoxImageZoom.Text);
                    //int retry = this.mapControl.Manager.Mode == AccessMode.CacheOnly ? 0 : 1; //是否重试
                    TileImageConnector tileImage = new TileImageConnector();
                    tileImage.Retry = retryNum;
                    tileImage.ImageTileComplete += new EventHandler(tileImage_ImageTileComplete);
                    tileImage.Start(this.mapControl.MapProvider, area, zoom);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                //MessageBox.Show("请先用“矩形”画图工具选择区域", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CommonTools.PromptingMessage.PromptMessage(this, "请先用“矩形”画图工具选择区域");
            }
        }

        void tileImage_ImageTileComplete(object sender, EventArgs e)
        {
            MessageBox.Show("拼接图生成完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        #endregion

        #region 地图切换

        private void 普通地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProviders.GoogleChinaMap;
        }

        private void 卫星地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProviders.GoogleChinaSatelliteMap;
        }

        private void 混合地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProviders.GoogleChinaHybridMap;
        }

        private void 普通地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduMapProvider.Instance;
        }

        private void 卫星地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduSatelliteMapProvider.Instance;
        }

        private void 混合地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduHybridMapProvider.Instance;
        }

        private void 普通地图ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.AMap.AMapProvider.Instance;
        }

        private void 卫星地图ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.AMap.AMapSateliteProvider.Instance;
        }

        private void 混合地图ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.AMap.AMapHybirdProvider.Instance;
        }

        private void 普通地图ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.SoSo.SosoMapProvider.Instance;
        }

        private void 卫星地图ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.SoSo.SosoMapSateliteProvider.Instance;
        }

        private void 混合地图ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.SoSo.SosoMapHybridProvider.Instance;
        }

        #endregion

        #region 画图工具

        private void 矩形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw.DrawingMode = DrawingMode.Rectangle;
            draw.IsEnable = true;
        }

        private void 圆形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw.DrawingMode = DrawingMode.Circle;
            draw.IsEnable = true;
        }

        private void 多边形ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw.DrawingMode = DrawingMode.Polygon;
            draw.IsEnable = true;
        }

        private void 线段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw.DrawingMode = DrawingMode.Line;
            draw.IsEnable = true;
        }

        private void 折线段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw.DrawingMode = DrawingMode.Route;
            draw.IsEnable = true;
        }

        private void 清除所有画图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.polygonsOverlay.Clear();
        }

        #endregion

        #region 地图访问模式

        private void serverAndCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.Manager.Mode = AccessMode.ServerAndCache;
            this.serverAndCacheToolStripMenuItem.Checked = true;
            this.本地缓存ToolStripMenuItem.Checked = false;
            this.在线服务ToolStripMenuItem.Checked = false;
        }

        private void 在线服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.Manager.Mode = AccessMode.ServerOnly;
            this.serverAndCacheToolStripMenuItem.Checked = false;
            this.本地缓存ToolStripMenuItem.Checked = false;
            this.在线服务ToolStripMenuItem.Checked = true;
        }

        private void 本地缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.Manager.Mode = AccessMode.CacheOnly;
            this.serverAndCacheToolStripMenuItem.Checked = false;
            this.本地缓存ToolStripMenuItem.Checked = true;
            this.在线服务ToolStripMenuItem.Checked = false;
        }

        #endregion

        private void 下载地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedRect = GMapUtil.PolygonUtils.GetRegionMaxRect(currentDragableRoute);
            DownloadMap();
        }

        private void 下载KMLToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 允许编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.允许编辑ToolStripMenuItem.Enabled = false;
            MapRoute route = currentDragableRoute;
            this.currentDragableNodes = new List<GMapMarkerEllipse>();
            for (int i = 0; i < route.Points.Count; i++)
            {
                GMapMarkerEllipse item = new GMapMarkerEllipse(route.Points[i])
                {
                    Pen = new Pen(Color.Blue)
                };
                item.Pen.Width = 2f;
                item.Pen.DashStyle = DashStyle.Solid;
                item.Fill = new SolidBrush(Color.FromArgb(0xff, Color.AliceBlue));
                item.Tag = i;
                this.currentDragableNodes.Add(item);
                this.regionOverlay.Markers.Add(item);
            }
        }

        private void 停止编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.允许编辑ToolStripMenuItem.Enabled = true;
            if (currentDragableNodes == null) return;
            for (int i = 0; i < currentDragableNodes.Count; ++i)
            {
                if (this.regionOverlay.Markers.Contains(currentDragableNodes[i]))
                {
                    this.regionOverlay.Markers.Remove(currentDragableNodes[i]);
                }
            }
        }
    }
}
