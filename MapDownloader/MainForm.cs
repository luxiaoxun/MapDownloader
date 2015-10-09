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
using System.Reflection;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using GMap.NET.CacheProviders;
using NetUtil;
using GMapUtil;
using MySql.Data.MySqlClient;
using log4net;
using GMapChinaRegion;
using GMapPolygonLib;
using GMapMarkerLib;
using GMapDrawTools;
using GMapCommonType;
using GMapProvidersExt;
using GMapProvidersExt.Tencent;
using GMapProvidersExt.AMap;

namespace GMapDownloader
{
    public partial class MainForm : DevComponents.DotNetBar.Office2007Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

        //private string conString = @"Server=127.0.0.1;Port=3306;Database=mapcache;Uid=root;Pwd=admin;";
        private static string conStringFormat = "Server={0};Port={1};Database={2};Uid={3};Pwd={4};";
        private static string conString;

        private Draw draw;
        private GMapOverlay polygonsOverlay = new GMapOverlay("polygonsOverlay"); //放置polygon的图层
        private GMapOverlay poiOverlay = new GMapOverlay("poiOverlay"); //放置poi的图层

        //中国省市边界
        private Country china;
        private bool isCountryLoad = false;
        private GMapOverlay regionOverlay = new GMapOverlay("region");

        private int retryNum = 3;
        private string tilePath = "D:\\GisMap";
        private SQLitePureImageCache sqliteCache = new SQLitePureImageCache();
        private MySQLPureImageCache mysqlCache = new MySQLPureImageCache();

        private bool isLeftButtonDown = false;
        private GMapMarkerEllipse currentDragableNode = null;
        private List<GMapMarkerEllipse> currentDragableNodes;
        private GMapAreaPolygon currentAreaPolygon;
        private GMapPolygon currentDrawPolygon;

        private PointLatLng routeStartPoint = PointLatLng.Empty;
        private PointLatLng routeEndPoint = PointLatLng.Empty;
        private GPoint leftClickPoint = GPoint.Empty;
        private GMapOverlay routeOverlay = new GMapOverlay("routeOverlay");
        private string currentCenterCityName = "南京市";

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
            mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduMapProvider1.Instance;
            //mapControl.MapProvider = GMapProvidersExt.AMap.AMapProvider.Instance;
            mapControl.Position = new PointLatLng(32.043, 118.773);
            mapControl.MinZoom = 1;
            mapControl.MaxZoom = 18;
            mapControl.Zoom = 9;
            //mapControl.EmptyMapBackground = Color.Black;
            //mapControl.FillEmptyTiles = true;

            mapControl.Overlays.Add(polygonsOverlay);
            mapControl.Overlays.Add(regionOverlay);
            mapControl.Overlays.Add(poiOverlay);
            mapControl.Overlays.Add(routeOverlay);

            this.mapControl.MouseClick += new MouseEventHandler(mapControl_MouseClick);
            this.mapControl.MouseMove += new MouseEventHandler(mapControl_MouseMove);
            this.mapControl.MouseDown += new MouseEventHandler(mapControl_MouseDown);
            this.mapControl.MouseUp += new MouseEventHandler(mapControl_MouseUp);
            this.mapControl.OnMarkerEnter += new MarkerEnter(mapControl_OnMarkerEnter);
            this.mapControl.OnMarkerLeave += new MarkerLeave(mapControl_OnMarkerLeave);
            this.mapControl.OnPolygonClick += new PolygonClick(mapControl_OnPolygonClick);
            this.mapControl.OnPolygonEnter += new PolygonEnter(mapControl_OnPolygonEnter);
            this.mapControl.OnPolygonLeave += new PolygonLeave(mapControl_OnPolygonLeave);
            this.mapControl.OnPositionChanged += new PositionChanged(mapControl_OnPositionChanged);
            
            draw = new Draw(this.mapControl);
            draw.DrawComplete += new EventHandler<DrawEventArgs>(draw_DrawComplete);
        }

        #region 地图控件事件

        void mapControl_OnPositionChanged(PointLatLng point)
        {
            BackgroundWorker centerPositionWorker = new BackgroundWorker();
            centerPositionWorker.DoWork +=new DoWorkEventHandler(centerPositionWorker_DoWork);
            centerPositionWorker.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(centerPositionWorker_RunWorkerCompleted);
            centerPositionWorker.RunWorkerAsync(point);
        }

        void centerPositionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Placemark place = (Placemark)e.Result;
            this.toolStripStatusCenter.Text = place.Address;
            currentCenterCityName = place.CityName;
        }

        void centerPositionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            PointLatLng p = (PointLatLng)e.Argument;
            Placemark centerPosPlace = SoSoMapProvider.Instance.GetCenterNameByLocation(p);
            e.Result = centerPosPlace;
        }

        void mapControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                leftClickPoint = new GPoint(e.X, e.Y);
                this.contextMenuStripLocation.Show(Cursor.Position);
            }
        }

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

            //Get Mouse down PointLatLng
            //if (e.Button == System.Windows.Forms.MouseButtons.Right)
            //{
            //    PointLatLng p = this.mapControl.FromLocalToLatLng(e.X, e.Y);
            //    Guid id = Guid.NewGuid();
            //    JsonHelper.JsonSerializeToFile(p, this.mapControl.MapProvider.Name + id.ToString() + ".txt", Encoding.UTF8);
            //}
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
                    if (tag.HasValue && this.currentAreaPolygon != null)
                    {
                        int? nullable2 = tag;
                        int count = this.currentAreaPolygon.Points.Count;
                        if (nullable2.GetValueOrDefault() < count)
                        {
                            this.currentAreaPolygon.Points[tag.Value] = p;
                            this.mapControl.UpdatePolygonLocalPosition(this.currentAreaPolygon);
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
                if (currentAreaPolygon != null && currentAreaPolygon == areaPolygon)
                {
                    currentAreaPolygon = item as GMapAreaPolygon;
                    currentAreaPolygon.Stroke.Color = Color.Blue;
                }
            }
        }

        void mapControl_OnPolygonEnter(GMapPolygon item)
        {
            if (item is GMapAreaPolygon)
            {
                GMapAreaPolygon areaPolygon = item as GMapAreaPolygon;
                if (currentAreaPolygon != null && currentAreaPolygon == areaPolygon)
                {
                    currentAreaPolygon = item as GMapAreaPolygon;
                    currentAreaPolygon.Stroke.Color = Color.Red;
                }
            }
        }

        void mapControl_OnPolygonClick(GMapPolygon item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (item is GMapAreaPolygon && currentAreaPolygon != null)
                {
                    this.contextMenuStripSelectedArea.Show(Cursor.Position);
                }
                if (item is GMapDrawRectangle || item is GMapDrawPolygon)
                {
                    currentDrawPolygon = item;
                    this.contextMenuStripDrawPolygon.Show(Cursor.Position);
                }
            }
        }

        #endregion

        //初始化UI
        private void InitUI()
        {
            ShowDownloadTip(false);
            this.toolStripStatusPOIDownload.Visible = false;

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
                        var fName = f.Replace("GMapDownloader.", string.Empty);
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
                    foreach (var city in provice.City)
                    {
                        DevComponents.AdvTree.Node cNode = new DevComponents.AdvTree.Node(city.name);
                        cNode.Tag = city;
                        foreach (var piecearea in city.Piecearea)
                        {
                            DevComponents.AdvTree.Node areaNode = new DevComponents.AdvTree.Node(piecearea.name);
                            areaNode.Tag = piecearea;
                            cNode.Nodes.Add(areaNode);
                        }
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
            this.advTreeChina.SelectedNode = sender as DevComponents.AdvTree.Node;
            if (e.Button == MouseButtons.Left||e.Button==MouseButtons.Right)
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
                    case 3:
                        Piecearea piecearea = e.Node.Tag as Piecearea;
                        name = piecearea.name;
                        rings = piecearea.rings;
                        break;
                }
                if (rings != null && !string.IsNullOrEmpty(rings))
                {
                    GMapPolygon polygon = ChinaMapRegion.GetRegionPolygon(name, rings);
                    if (polygon != null)
                    {
                        GMapAreaPolygon areaPolygon = new GMapAreaPolygon(polygon.Points, name);
                        currentAreaPolygon = areaPolygon;
                        regionOverlay.Polygons.Clear();
                        regionOverlay.Polygons.Add(areaPolygon);
                        RectLatLng rect = GMapUtil.PolygonUtils.GetRegionMaxRect(polygon);
                        this.mapControl.SetZoomToFitRect(rect);
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
                //byte[] buffer = Properties.Resources.ChinaBoundryBinary_Province_City;
                byte[] buffer = Properties.Resources.ChinaBoundryBinaryAll;
                china = GMapChinaRegion.ChinaMapRegion.GetChinaRegionFromJsonBinaryBytes(buffer);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion 

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
        }

        private void UpdateDownloadBar(int value, ulong allNum, ulong comNum, int zoom)
        {
            this.toolStripStatusDownload.Text = string.Format("下载进度：级别{0}，{1}/{2}",zoom,comNum,allNum);
        }

        void prefetchTiles_PrefetchTileComplete(object sender, PrefetchTileEventArgs e)
        {
            MessageBox.Show("地图下载完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            ShowDownloadTip(false);
        }

        private void ResetToServerAndCacheMode()
        {
            if (this.mapControl.Manager.Mode != AccessMode.ServerAndCache)
            {
                this.mapControl.Manager.Mode = AccessMode.ServerAndCache;
                this.serverAndCacheToolStripMenuItem.Checked = true;
                this.本地缓存ToolStripMenuItem.Checked = false;
                this.在线服务ToolStripMenuItem.Checked = false;
            }
        }

        private void DownloadMap(GMapPolygon polygon)
        {
            if (polygon != null)
            {
                RectLatLng area = GMapUtil.PolygonUtils.GetRegionMaxRect(polygon);
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
                            prefetchTiles.Start(area, minZ, maxZ, mapControl.MapProvider, tilePath);
                        }
                        else
                        {
                            //切片存在数据库中
                            prefetchTiles.Start(area, minZ, maxZ, mapControl.MapProvider);
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
                CommonTools.PromptingMessage.PromptMessage(this, "请先用“矩形”画图工具选择区域");
            }
        }

        //下载地图
        private void buttonDownload_Click(object sender, EventArgs e)
        {
            if (currentDrawPolygon != null)
            {
                DownloadMap(currentDrawPolygon);
            }
            else if (currentAreaPolygon != null)
            {
                DownloadMap(currentAreaPolygon);
            }
            else
            {
                CommonTools.PromptingMessage.PromptMessage(this, "请先用“矩形”画图工具选择区域");
            }
        }

        private void 下载地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentAreaPolygon != null)
            {
                DownloadMap(currentAreaPolygon);
            }
            else
            {
                CommonTools.PromptingMessage.PromptMessage(this, "请先用“矩形”画图工具选择区域");
            }
        }

        private void 允许编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.允许编辑ToolStripMenuItem.Enabled = false;
            MapRoute route = currentAreaPolygon;
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

        #endregion

        #region 拼接大图

        void buttonMapImage_Click(object sender, EventArgs e)
        {
            if (currentDrawPolygon != null)
            {
                RectLatLng area = GMapUtil.PolygonUtils.GetRegionMaxRect(currentDrawPolygon);
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
            else if (currentAreaPolygon != null)
            {
                RectLatLng area = GMapUtil.PolygonUtils.GetRegionMaxRect(currentAreaPolygon);
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
                CommonTools.PromptingMessage.PromptMessage(this, "请先用“矩形”画图工具选择区域");
            }
        }

        void tileImage_ImageTileComplete(object sender, EventArgs e)
        {
            MessageBox.Show("拼接图生成完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        #endregion

        #region 地图切换

        private PointLatLng GetNewMapPosition(GMapProvider from, GMapProvider to)
        {
            PointLatLng pos = this.mapControl.Position;
            int zoom = (int)this.mapControl.Zoom;
            GPoint gpoint = from.Projection.FromLatLngToPixel(pos, zoom);

            return to.Projection.FromPixelToLatLng(gpoint, zoom);
        }

        //Google地图
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

        private void 地形图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProviders.GoogleChinaTerrainMap;
        }

        //Baidu地图
        private void 普通地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduMapProvider.Instance;
            //this.mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduMapProvider1.Instance;
        }

        private void 卫星地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduSatelliteMapProvider.Instance;
        }

        private void 混合地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduHybridMapProvider.Instance;
        }

        //高德地图
        private void 普通地图ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //PointLatLng p = GetNewMapPosition(this.mapControl.MapProvider, GMapProvidersExt.AMap.AMapProvider.Instance);
            //PointLatLng p = this.mapControl.Position;
            //GPoint posPixel = this.mapControl.PositionPixel;
            this.mapControl.MapProvider = GMapProvidersExt.AMap.AMapProvider.Instance;
            //this.mapControl.Position = this.mapControl.MapProvider.Projection.FromPixelToLatLng(posPixel, (int)this.mapControl.Zoom);
        }

        private void 卫星地图ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.AMap.AMapSateliteProvider.Instance;
        }

        private void 混合地图ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.AMap.AMapHybirdProvider.Instance;
        }

        //SoSo地图
        private void 普通地图ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //this.mapControl.MapProvider = GMapProvidersExt.SoSo.SosoMapProvider.Instance;
            this.mapControl.MapProvider = GMapProvidersExt.Tencent.SoSoMapProvider.Instance;
        }

        private void 卫星地图ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.SoSo.SosoMapSateliteProvider.Instance;
        }

        private void 混合地图ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.SoSo.SosoMapHybridProvider.Instance;
        }

        private void 地形地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = SoSoTerrainMapAnnoProvider.Instance;
        }

        //Here地图
        private void 普通地图ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProvidersExt.Here.NokiaMapProvider.Instance;
        }

        private void 卫星地图ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProvidersExt.Here.NokiaSatelliteMapProvider.Instance;
        }

        private void 混合地图ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProvidersExt.Here.NokiaHybridMapProvider.Instance;
        }

        //Bing地图
        private void 普通地图ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProviders.BingMap;
        }

        private void 卫星地图ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProviders.BingSatelliteMap;
        }

        private void 混合地图ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProviders.BingHybridMap;
        }

        private void 普通地图中文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Bing.BingChinaMapProvider.Instance;
        }

        //搜狗地图
        private void 普通地图ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Sogou.SogouMapProvider.Instance;
        }

        //天地图
        private void 街道地图球面墨卡托ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //PointLatLng p = GetNewMapPosition(this.mapControl.MapProvider, GMapProvidersExt.TianDitu.TiandituMapProviderWithAnno3857.Instance);
            //PointLatLng p = this.mapControl.Position;
            //GPoint posPixel = this.mapControl.PositionPixel;
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituMapProviderWithAnno.Instance;
            //this.mapControl.Position = p;
            //this.mapControl.Position = this.mapControl.MapProvider.Projection.FromPixelToLatLng(posPixel, (int)this.mapControl.Zoom);
        }

        private void 卫星地图球面墨卡托ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituSatelliteMapProvider.Instance;
        }

        private void 混合地图球面墨卡托ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituSatelliteMapProviderWithAnno.Instance;
        }

        private void 街道地图WGS84ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituMapProviderWithAnno4326.Instance;
        }

        private void 卫星地图WGS84ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituSatelliteMapProvider4326.Instance;
        }

        private void 混合地图WGS84ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituSatelliteMapProviderWithAnno4326.Instance;
        }

        //ArcGIS地图
        private void arcGIS街道地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISMapProvider.Instance;
        }

        private void arcGIS街道地图无POIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISMapProviderNoPoi.Instance;
        }

        private void arcGIS街道地图冷色版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISColdMapProvider.Instance;
        }

        private void arcGIS街道地图灰色版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISGrayMapProvider.Instance;
        }

        private void arcGIS街道地图暖色版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISWarmMapProvider.Instance;
        }

        private void arcGIS卫星地图无偏移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISSatelliteMapProvider.Instance;
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

        //画图完成函数
        void draw_DrawComplete(object sender, DrawEventArgs e)
        {
            try
            {
                if (e != null && (e.Polygon != null || e.Rectangle != null || e.Circle != null || e.Line != null || e.Route!=null))
                {
                    switch (e.DrawingMode)
                    {
                        case DrawingMode.Polygon:
                            polygonsOverlay.Polygons.Add(e.Polygon);
                            currentDrawPolygon = e.Polygon;
                            break;
                        case DrawingMode.Rectangle:
                            polygonsOverlay.Polygons.Add(e.Rectangle);
                            currentDrawPolygon = e.Rectangle;
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

        #region KML GPX 操作

        private void buttonItemReadGpx_Click(object sender, EventArgs e)
        {
            using (FileDialog dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "gpx";
                dialog.CheckPathExists = true;
                dialog.CheckFileExists = false;
                dialog.AddExtension = true;
                dialog.DefaultExt = "gpx";
                dialog.ValidateNames = true;
                dialog.Title = "选择GPX文件";
                dialog.Filter = "GPX文件 (*.gpx)|*.gpx|所有文件 (*.*)|*.*";
                dialog.FilterIndex = 1;
                dialog.RestoreDirectory = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string objectXml = File.ReadAllText(dialog.FileName);
                        gpxType type = this.mapControl.Manager.DeserializeGPX(objectXml);
                        if (type != null)
                        {
                            if ((type.trk != null) && (type.trk.Length > 0))
                            {
                                List<PointLatLng> points = new List<PointLatLng>();
                                foreach (trkType trk in type.trk)
                                {
                                    foreach (trksegType seg in trk.trkseg)
                                    {
                                        foreach (wptType p in seg.trkpt)
                                        {
                                            points.Add(new PointLatLng((double)p.lat, (double)p.lon));
                                        }
                                    }
                                    string name = string.IsNullOrEmpty(trk.name) ? string.Empty : trk.name;
                                    GMapRoute item = new GMapRoute(points, name)
                                    {
                                        Stroke = new Pen(Color.FromArgb(0x90, Color.Red))
                                    };
                                    item.Stroke.Width = 5f;
                                    item.Stroke.DashStyle = DashStyle.DashDot;
                                    this.polygonsOverlay.Routes.Add(item);
                                }
                            }
                            if ((type.rte != null) && (type.rte.Length > 0))
                            {
                                List<PointLatLng> points = new List<PointLatLng>();
                                foreach (rteType rte in type.rte)
                                {
                                    foreach (wptType p in rte.rtept)
                                    {
                                        points.Add(new PointLatLng((double)p.lat, (double)p.lon));
                                    }
                                    string str3 = string.IsNullOrEmpty(rte.name) ? string.Empty : rte.name;
                                    GMapRoute route2 = new GMapRoute(points, str3)
                                    {
                                        Stroke = new Pen(Color.FromArgb(0x90, Color.Red))
                                    };
                                    route2.Stroke.Width = 5f;
                                    route2.Stroke.DashStyle = DashStyle.DashDot;
                                    this.polygonsOverlay.Routes.Add(route2);
                                }
                            }
                            if (type.wpt != null && type.wpt.Length > 0)
                            {
                                foreach (wptType p in type.wpt)
                                {
                                    PointLatLng point = new PointLatLng((double)p.lat, (double)p.lon);
                                    GMarkerGoogle marker = new GMarkerGoogle(point, GMarkerGoogleType.blue_dot);
                                    this.polygonsOverlay.Markers.Add(marker);
                                }
                            }
                            this.mapControl.ZoomAndCenterRoutes(null);
                        }
                    }
                    catch (Exception exception)
                    {
                        CommonTools.PromptingMessage.ErrorMessage(this, "读取GPX文件错误");
                    }
                }
            }
        }

        private void SaveKmlToFile(MapRoute item, string name, string fileName)
        {
            if (item is GMapRoute)
            {
                GMapRoute route = (GMapRoute)item;
                KmlUtil.SaveLineString(route.Points, name, fileName);
            }
            else if (item is GMapRectangle)
            {
                GMapRectangle rectangle = (GMapRectangle)item;
                KmlUtil.SavePolygon(rectangle.Points, name, fileName);
            }
            else if (item is GMapPolygon)
            {
                GMapPolygon polygon = (GMapPolygon)item;
                KmlUtil.SavePolygon(polygon.Points, name, fileName);
            }

        }

        private void 下载KMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentAreaPolygon != null)
            {
                string name = "KmlFile.kml";
                SaveFileDialog dialog = new SaveFileDialog
                {
                    FileName = name,
                    Title = "选择Kml文件位置",
                    Filter = "Kml文件|*.kml"
                };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SaveKmlToFile(currentAreaPolygon, name, dialog.FileName);
                }
            }
        }

        private void DrawPlaceMark(KmlPlaceMark placeMark, bool centerToMark = true,
            bool showToolTip = true, GMarkerGoogleType markerType = GMarkerGoogleType.blue_pushpin, bool isTempMark = false)
        {
            if (((placeMark != null) && (placeMark.Geometry != null)) && (placeMark.Geometry.Points.Count != 0))
            {
                switch (placeMark.Geometry.GeoType)
                {
                    case GeometryType.Point:
                        {
                            GMarkerGoogle item = new GMarkerGoogle(placeMark.Geometry.ToPointLatLngs()[0], markerType);
                            this.polygonsOverlay.Markers.Add(item);
                            if (showToolTip)
                            {
                                if (!isTempMark)
                                {
                                    item.ToolTipText = placeMark.Name;
                                    item.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                                }
                                else
                                {
                                    item.ToolTipText = placeMark.Description;
                                }
                            }
                            if (centerToMark)
                            {
                                this.mapControl.ZoomAndCenterMarkers(this.polygonsOverlay.Id);
                            }
                            return;
                        }
                    case GeometryType.Polyline:
                        {
                            GMapRoute route = new GMapRoute(placeMark.Geometry.ToPointLatLngs(), "_kmlPolyline")
                            {
                                IsHitTestVisible = true
                            };
                            if (showToolTip)
                            {
                                int num2 = placeMark.Geometry.Points.Count / 2;
                                Point2D pointd2 = placeMark.Geometry.Points[num2];
                                GMapMarkerEllipse ellipse2 = new GMapMarkerEllipse(pointd2.ToPointLatLngs()[0])
                                {
                                    ToolTipText = string.Format("名称:{0}\r\n类型:{1}\r\n描述:{2}", placeMark.Name, "多边形", placeMark.Description),
                                    ToolTipMode = MarkerTooltipMode.OnMouseOver
                                };
                                //route.ToolTipMarker = ellipse2;
                                //route.ToolTipPosition = MapRouteToolTipPosition.Custom;
                            }
                            this.polygonsOverlay.Routes.Add(route);
                            if (centerToMark)
                            {
                                this.mapControl.ZoomAndCenterRoute(route);
                            }
                            return;
                        }
                    case GeometryType.Polygon:
                        {
                            GMapPolygon polygon = new GMapPolygon(placeMark.Geometry.ToPointLatLngs(), "_kmlPolygon")
                            {
                                Stroke = new Pen(Color.FromArgb(0xff, Color.Blue))
                            };
                            polygon.Stroke.Width = 2f;
                            polygon.Stroke.DashStyle = DashStyle.Dash;
                            polygon.Fill = new SolidBrush(Color.FromArgb(20, Color.Blue));
                            polygon.IsHitTestVisible = true;
                            //polygon.EnableRightClick = true;
                            if (showToolTip)
                            {
                                GMapMarkerEllipse ellipse = new GMapMarkerEllipse(placeMark.Geometry.Center.ToPointLatLngs()[0])
                                {
                                    ToolTipText = string.Format("名称:{0}\r\n类型:{1}\r\n描述:{2}", placeMark.Name, "多边形", placeMark.Description),
                                    ToolTipMode = MarkerTooltipMode.OnMouseOver
                                };
                                //polygon.ToolTipMarker = ellipse;
                                //polygon.ToolTipPosition = MapRouteToolTipPosition.Custom;
                            }
                            this.polygonsOverlay.Polygons.Add(polygon);
                            if (centerToMark)
                            {
                                this.mapControl.ZoomAndCenterRoute(polygon);
                                return;
                            }
                            return;
                        }
                }
            }
        }

        private void InitKMLPlaceMarks(List<KmlPlaceMark> placeMarks)
        {
            Polygon polygon = new Polygon();
            int num = 0;
            foreach (KmlPlaceMark mark in placeMarks)
            {
                num++;
                string str = "多边形";
                if (mark.Geometry.GeoType == GeometryType.Point)
                {
                    str = "点";
                }
                if (mark.Geometry.GeoType == GeometryType.Polyline)
                {
                    str = "折线";
                }
                //if (string.IsNullOrEmpty(mark.Name))
                //{
                //    this.cbx_SelectKML.Items.Add(string.Format("{0}:{1}", str, num));
                //}
                //else
                //{
                //    this.cbx_SelectKML.Items.Add(string.Format("{0}", mark.Name));
                //}
                DrawPlaceMark(mark, false, true, GMarkerGoogleType.blue_pushpin, false);
                polygon.Points.AddRange(mark.Geometry.Points);
            }
            BoundingBox envelope = polygon.Envelope;
            this.mapControl.SetZoomToFitRect(RectLatLng.FromLTRB(envelope.Left, envelope.Top, envelope.Right, envelope.Bottom));
            this.mapControl.Position = new PointLatLng(envelope.Center.Y, envelope.Center.X);
        }

        private void buttonItemReadKML_Click(object sender, EventArgs e)
        {
            try
            {
                using (FileDialog dialog = new OpenFileDialog())
                {
                    dialog.CheckPathExists = true;
                    dialog.CheckFileExists = false;
                    dialog.AddExtension = true;
                    dialog.ValidateNames = true;
                    dialog.Title = "选择KML文件";
                    dialog.FilterIndex = 1;
                    dialog.RestoreDirectory = true;
                    dialog.Filter = "KML文件 (*.kml)|*.kml|所有文件 (*.*)|*.*";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        this.polygonsOverlay.Clear();
                        InitKMLPlaceMarks(KmlUtil.GetPlaceMarksFromKmlFile(dialog.FileName));
                    }
                }
            }
            catch (Exception exception)
            {
                CommonTools.PromptingMessage.ErrorMessage(this, "下载KML文件时出现异常");
            }
        }

        #endregion

        #region POI查询

        List<Placemark> poisQueryResult = new List<Placemark>();
        int poiQueryCount = 0;

        private void queryProgressEvent(long completedCount, long total)
        {
            this.toolStripStatusPOIDownload.Text = string.Format("已找到：{0}条POI，还在查询中...", completedCount);
        }
        
        void poiWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (poisQueryResult != null && poisQueryResult.Count > 0)
            {
                foreach (Placemark place in poisQueryResult)
                {
                    GMarkerGoogle marker = new GMarkerGoogle(place.Point, GMarkerGoogleType.blue_dot);
                    marker.ToolTipText = place.Name+"\r\n"+place.Address+"\r\n"+place.Category;
                    this.poiOverlay.Markers.Add(marker);
                }
            }
            this.toolStripStatusPOIDownload.Text = string.Format("共找到：{0}条POI数据", poisQueryResult.Count);
        }

        void poiWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            POISearchArgument argument = e.Argument as POISearchArgument;
            if (argument != null)
            {
                string regionName = argument.Region;
                string poiQueryRectangleStr = argument.Rectangle;
                string keyWords = argument.KeyWord;
                SoSoMapProvider.Instance.GetPlacemarksByKeywords(keyWords, regionName, poiQueryRectangleStr,
                    "", "", this.queryProgressEvent, out this.poisQueryResult, ref this.poiQueryCount);
            }
        }

        //边界“POI查询”
        private void pOI查询ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PoiKeyWordForm poiKeyWord = new PoiKeyWordForm();
            if (poiKeyWord.ShowDialog() == DialogResult.OK)
            {
                string keyWord = poiKeyWord.GetKeyWord();
                this.poiOverlay.Markers.Clear();
                if (currentAreaPolygon != null)
                {
                    POISearchArgument argument = new POISearchArgument();
                    argument.KeyWord = keyWord;
                    argument.Region = currentAreaPolygon.Name;
                    RectLatLng rect = GMapUtil.PolygonUtils.GetRegionMaxRect(currentAreaPolygon);
                    argument.Rectangle = string.Format("{0},{1},{2},{3}",
                        new object[] { rect.LocationRightBottom.Lat, rect.LocationTopLeft.Lng, rect.LocationTopLeft.Lat, rect.LocationRightBottom.Lng });
                    
                    toolStripStatusPOIDownload.Visible = true;
                    BackgroundWorker poiWorker = new BackgroundWorker();
                    poiWorker.DoWork += new DoWorkEventHandler(poiWorker_DoWork);
                    poiWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(poiWorker_RunWorkerCompleted);
                    poiWorker.RunWorkerAsync(argument);
                }
            }
        }

        //画图“POI查询”
        private void pOI查询ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            PoiKeyWordForm poiKeyWord = new PoiKeyWordForm();
            if (poiKeyWord.ShowDialog() == DialogResult.OK)
            {
                string keyWord = poiKeyWord.GetKeyWord();
                this.poiOverlay.Markers.Clear();
                if (currentDrawPolygon != null)
                {
                    POISearchArgument argument = new POISearchArgument();
                    argument.KeyWord = keyWord;
                    argument.Region = "";
                    RectLatLng rect = GMapUtil.PolygonUtils.GetRegionMaxRect(currentDrawPolygon);
                    argument.Rectangle = string.Format("{0},{1},{2},{3}", 
                        new object[] { rect.LocationRightBottom.Lat, rect.LocationTopLeft.Lng, rect.LocationTopLeft.Lat, rect.LocationRightBottom.Lng });
                    toolStripStatusPOIDownload.Visible = true;
                    BackgroundWorker poiWorker = new BackgroundWorker();
                    poiWorker.DoWork += new DoWorkEventHandler(poiWorker_DoWork);
                    poiWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(poiWorker_RunWorkerCompleted);
                    poiWorker.RunWorkerAsync(argument);
                }
            }
        }

        //关键字POI查询
        private void buttonPOISearch_Click(object sender, EventArgs e)
        {
            string keywords = this.textBoxPOIkeyword.Text.Trim();
            if (!string.IsNullOrEmpty(keywords))
            {
                this.poiOverlay.Markers.Clear();
                this.listBoxAddress.Items.Clear();
                List<Placemark> queryResult = SoSoMapProvider.Instance.GetPlacemarksByKeywords(keywords);
                if (queryResult != null && queryResult.Count > 0)
                {
                    foreach (Placemark place in queryResult)
                    {
                        GMarkerGoogle marker = new GMarkerGoogle(place.Point, GMarkerGoogleType.blue_dot);
                        marker.ToolTipText = place.Name + "\r\n" + place.Address + "\r\n" + place.Category;
                        this.poiOverlay.Markers.Add(marker);
                        this.listBoxAddress.Items.Add(place.Address);
                    }
                }
            }
        }

        #endregion

        #region 地址解析与逆解析

        //地址解析
        private void buttonAddressSearch_Click(object sender, EventArgs e)
        {
            string address = this.textBoxAddress.Text.Trim();
            if (!string.IsNullOrEmpty(address))
            {
                this.routeOverlay.Markers.Clear();
                Placemark placemark = new Placemark(address);
                placemark.CityName = currentCenterCityName;
                if (currentAreaPolygon != null)
                {
                    placemark.CityName = currentAreaPolygon.Name;
                }
                List<PointLatLng> points = new List<PointLatLng>();
                //GeoCoderStatusCode statusCode = SoSoMapProvider.Instance.GetPoints(placemark, out points);
                GeoCoderStatusCode statusCode = AMapProvider.Instance.GetPoints(placemark, out points);
                if (statusCode == GeoCoderStatusCode.G_GEO_SUCCESS)
                {
                    foreach (PointLatLng p in points)
                    {
                        GMarkerGoogle marker = new GMarkerGoogle(p, GMarkerGoogleType.blue_dot);
                        marker.ToolTipText = placemark.Address;
                        this.routeOverlay.Markers.Add(marker);
                        this.mapControl.Position = p;
                    }
                }
            }
        }

        private void 搜索该点的地址ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PointLatLng p = this.mapControl.FromLocalToLatLng((int)leftClickPoint.X, (int)leftClickPoint.Y);
            GeoCoderStatusCode statusCode;
            //Placemark? place = SoSoMapProvider.Instance.GetPlacemark(p, out statusCode);
            Placemark? place = AMapProvider.Instance.GetPlacemark(p, out statusCode);
            if (place.HasValue)
            {
                GMapImageMarker placeMarker = new GMapImageMarker(p, Properties.Resources.MapMarker_Bubble_Azure, place.Value.Address);
                this.routeOverlay.Markers.Add(placeMarker);
                //CommonTools.PromptingMessage.PromptMessage(this, place.Value.Address);
            }
        }

        #endregion 

        #region 导入数据

        private List<RouteMapData> GetRouteMapData(DataTable dt)
        {
            if (dt == null) return null;
            List<RouteMapData> signalMapdatas = new List<RouteMapData>();
            foreach (DataRow dr in dt.Rows)
            {
                RouteMapData data = new RouteMapData();
                double lng;
                bool ret1 = double.TryParse(dr["Longitude"].ToString(), out lng);
                if (ret1)
                {
                    if (lng > 180 || lng < -180) continue;
                    data.Lng = lng;
                }

                double lat;
                bool ret2 = double.TryParse(dr["Latitude"].ToString(), out lat);
                if (ret2)
                {
                    if (lat > 90 || lng < -90) continue;
                    data.Lat = lat;
                }

                if (!ret1 || !ret2) continue;

                data.Lng = double.Parse(dr["Longitude"].ToString());
                data.Lat = double.Parse(dr["Latitude"].ToString());
                //data.Datetime = DateTime.Parse(dr["Time"].ToString());
                data.Datetime = dr["Time"].ToString();

                Dictionary<string, double> kvdata = new Dictionary<string, double>();

                //跳过前3列得到后面的信号值
                for (int i = 3; i < dt.Columns.Count; ++i)
                {
                    string colName = dt.Columns[i].ColumnName;
                    if (!kvdata.ContainsKey(colName))
                    {
                        if (dr[colName] != null)
                        {
                            double d;
                            bool ret = double.TryParse(dr[colName].ToString(), out d);
                            if (ret)
                            {
                                kvdata.Add(colName, d);
                            }
                            else
                            {
                                kvdata.Add(colName, 0);
                            }
                        }
                        else
                        {
                            kvdata.Add(colName, 0);
                        }
                    }
                }

                data.SignalDataDictionary = kvdata;
                signalMapdatas.Add(data);
            }

            return signalMapdatas;
        }

        private void 导入路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openDlg = new OpenFileDialog();
                openDlg.Filter = "Excel File (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                openDlg.FilterIndex = 1;
                openDlg.RestoreDirectory = true;
                if (openDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DataTable dt = ExcelHelper.ExcelToDataTable(openDlg.FileName, null, true);
                    List<RouteMapData> signalMapdatas = GetRouteMapData(dt);
                    if (signalMapdatas != null && signalMapdatas.Count > 0)
                    {
                        GMapOverlay overlay = new GMapOverlay(openDlg.SafeFileName);
                        foreach (RouteMapData signalMapdata in signalMapdatas)
                        {
                            double x;
                            double y;
                            GMapPositionFix.CoordinateTransform.ConvertWgs84ToGcj02(signalMapdata.Lng, signalMapdata.Lat, out x, out y);
                            signalMapdata.Lng = x;
                            signalMapdata.Lat = y;
                            PointLatLng p = new PointLatLng(signalMapdata.Lat, signalMapdata.Lng);
                            RouteDataMarker marker = new RouteDataMarker(p, signalMapdata);
                            //marker.RouteDataColorList = routeDataColorList;
                            //marker.CheckedKey = checkedKey;
                            overlay.Markers.Add(marker);
                        }
                        this.mapControl.Overlays.Add(overlay);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region 路径导航

        private void 以此为起点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            routeStartPoint = this.mapControl.FromLocalToLatLng((int)leftClickPoint.X, (int)leftClickPoint.Y);
            GMapImageMarker marker = new GMapImageMarker(routeStartPoint, Properties.Resources.MapMarker_Bubble_Pink);
            this.routeOverlay.Markers.Add(marker);
        }

        private void 以此为终点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            routeEndPoint = this.mapControl.FromLocalToLatLng((int)leftClickPoint.X, (int)leftClickPoint.Y);
            GMapImageMarker marker = new GMapImageMarker(routeEndPoint, Properties.Resources.MapMarker_Bubble_Chartreuse);
            this.routeOverlay.Markers.Add(marker);

            if (routeStartPoint != PointLatLng.Empty)
            {
                MapRoute route = GMapProvidersExt.AMap.AMapProvider.Instance.GetRoute(routeStartPoint, routeEndPoint, currentCenterCityName);
                GMapRoute mapRoute = new GMapRoute(route.Points, "");
                if (mapRoute != null)
                {
                    this.routeOverlay.Routes.Add(mapRoute);
                    this.mapControl.ZoomAndCenterRoute(mapRoute);
                }
            }
        }

        #endregion

        #region 清除图层

        private void 清除画图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.polygonsOverlay.Clear();
            currentDrawPolygon = null;
        }

        private void 清楚边界ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.regionOverlay.Polygons.Clear();
            currentAreaPolygon = null;
        }

        private void 清楚路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.routeOverlay.Clear();
        }
        
        #endregion

        
    }
}
