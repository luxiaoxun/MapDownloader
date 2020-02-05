using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Net;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using GMap.NET.CacheProviders;
using NetUtil;
using GMapUtil;
using log4net;
using GMapChinaRegion;
using GMapPolygonLib;
using GMapMarkerLib;
using GMapDrawTools;
using GMapCommonType;
using GMapProvidersExt;
using GMapProvidersExt.Tencent;
using GMapProvidersExt.AMap;
using GMapProvidersExt.Baidu;
using GMapExport;
using GMapHeat;
using GMapDownload;
using GMapPOI;
using GMapTools;

namespace MapDownloader
{
    public partial class MainForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

        //private string conString = @"Server=127.0.0.1;Port=3306;Database=mapcache;Uid=root;Pwd=admin;";
        private string conStringFormat = "Server={0};Port={1};Database={2};Uid={3};Pwd={4};";
        private string conString;

        private Draw draw;
        // polygons overlay
        private GMapOverlay polygonsOverlay = new GMapOverlay("polygonsOverlay");
        // POI overlay
        private GMapOverlay poiOverlay = new GMapOverlay("poiOverlay");

        // China boundry
        private Country china;
        private bool isCountryLoad = false;
        private GMapOverlay regionOverlay = new GMapOverlay("region");

        private int retryNum = 3;
        private string tilePath = "D:\\GisMap";
        private SQLitePureImageCache sqliteCache = new SQLitePureImageCache();
        private MySQLPureImageCacheMulti mysqlCache = new MySQLPureImageCacheMulti();

        private bool isLeftButtonDown = false;
        // Current dragable node when editing "current area polygon"
        private GMapMarkerEllipse currentDragableNode = null;
        private List<GMapMarkerEllipse> currentDragableNodes;
        // Current area polygon for downloading
        private GMapAreaPolygon currentAreaPolygon;

        private DrawDistance drawDistance;  // Draw distane tool

        private bool allowRouting = false;
        private PointLatLng routeStartPoint = PointLatLng.Empty;
        private PointLatLng routeEndPoint = PointLatLng.Empty;
        private GPoint leftClickPoint = GPoint.Empty;
        private GMapOverlay routeOverlay = new GMapOverlay("routeOverlay");
        private string currentCenterCityName = "南京市";

        // Tile Downloader, init 5 threads
        private TileDownloader tileDownloader = new TileDownloader(5);

        public MainForm()
        {
            InitializeComponent();

            InitMap();

            InitUI();

            InitPOISearch();

            InitMySQLConString();
        }

        // Init App, load configurations
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

        // Init map
        private void InitMap()
        {
            mapControl.ShowCenter = false;
            mapControl.DragButton = System.Windows.Forms.MouseButtons.Left;
            mapControl.CacheLocation = Environment.CurrentDirectory + "\\MapCache\\"; // Map cache location
            //mapControl.MapProvider = GMapProviders.GoogleChinaMap;
            //mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduMapProvider.Instance;
            mapControl.MapProvider = GMapProvidersExt.AMap.AMapProvider.Instance;
            mapControl.Position = new PointLatLng(32.043, 118.773);
            mapControl.MinZoom = 1;
            mapControl.MaxZoom = 18;
            mapControl.Zoom = 9;

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
            this.mapControl.OnMarkerClick += new MarkerClick(mapControl_OnMarkerClick);
            this.mapControl.OnPolygonClick += new PolygonClick(mapControl_OnPolygonClick);
            this.mapControl.OnPolygonEnter += new PolygonEnter(mapControl_OnPolygonEnter);
            this.mapControl.OnPolygonLeave += new PolygonLeave(mapControl_OnPolygonLeave);
            this.mapControl.OnPositionChanged += new PositionChanged(mapControl_OnPositionChanged);
            this.mapControl.OnMapZoomChanged += new MapZoomChanged(mapControl_OnMapZoomChanged);
            this.mapControl.OnPolygonDoubleClick += new PolygonDoubleClick(mapControl_OnPolygonDoubleClick);

            draw = new Draw(this.mapControl);
            draw.DrawComplete += new EventHandler<DrawEventArgs>(draw_DrawComplete);

            drawDistance = new DrawDistance(this.mapControl);
            drawDistance.DrawComplete += new EventHandler<DrawDistanceEventArgs>(drawDistance_DrawComplete);
        }

        // Double click to download the map
        void mapControl_OnPolygonDoubleClick(GMapPolygon item, MouseEventArgs e)
        {
            if (item is GMapAreaPolygon)
            {
                if (currentAreaPolygon != null)
                {
                    DownloadMap(currentAreaPolygon);
                }
                else
                {
                    CommonTools.MessageBox.ShowTipMessage("请先用画图工具画下载的区域多边形或选择省市区域！");
                }
            }
        }

        #region Map event

        void mapControl_OnMapZoomChanged()
        {
            if (this.mapControl.Zoom >= 14)
            {
                //Allow routing on map
                allowRouting = true;
            }
            else
            {
                allowRouting = false;
            }

            if (heatMarker != null)
            {
                var tl = mapControl.FromLatLngToLocal(heatRect.LocationTopLeft);
                var br = mapControl.FromLatLngToLocal(heatRect.LocationRightBottom);

                heatMarker.Position = heatRect.LocationTopLeft;
                heatMarker.Size = new System.Drawing.Size((int)(br.X - tl.X), (int)(br.Y - tl.Y));
            }
        }

        void mapControl_OnPositionChanged(PointLatLng point)
        {
            BackgroundWorker centerPositionWorker = new BackgroundWorker();
            centerPositionWorker.DoWork += new DoWorkEventHandler(centerPositionWorker_DoWork);
            centerPositionWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(centerPositionWorker_RunWorkerCompleted);
            centerPositionWorker.RunWorkerAsync(point);
        }

        void centerPositionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                Placemark place = (Placemark)e.Result;
                if (!place.Equals(Placemark.Empty))
                {
                    this.toolStripStatusCenter.Text = "地图中心:" + place.ProvinceName + "," + place.CityName + "," + place.DistrictName;
                    currentCenterCityName = place.CityName;
                }
            }
            catch (Exception ex)
            {
                log.Error("Locate the map center error: " + ex);
            }
        }

        void centerPositionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            PointLatLng p = (PointLatLng)e.Argument;
            //Placemark centerPosPlace = SoSoMapProvider.Instance.GetCenterNameByLocation(p);
            Placemark centerPosPlace = AMapProvider.Instance.GetCenterNameByLocation(p);
            e.Result = centerPosPlace;
        }

        void mapControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                leftClickPoint = new GPoint(e.X, e.Y);
                if (allowRouting)
                {
                    this.contextMenuStripLocation.Show(Cursor.Position);
                }
            }
        }

        void mapControl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (item is DrawDeleteMarker)
                {
                    currentAreaPolygon = null;

                    GMapOverlay overlay = item.Overlay;
                    if (overlay.Markers.Contains(item))
                    {
                        overlay.Markers.Remove(item);
                    }

                    if (this.mapControl.Overlays.Contains(overlay))
                    {
                        this.mapControl.Overlays.Remove(overlay);
                    }
                }
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
        }

        // Mouse move evnet
        void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                PointLatLng p = this.mapControl.FromLocalToLatLng(e.X, e.Y);

                int zoom = (int)this.mapControl.Zoom;
                double resolution = this.mapControl.MapProvider.Projection.GetLevelResolution(zoom);
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
            }
        }

        #endregion

        // Init UI
        private void InitUI()
        {
            ShowDownloadTip(false);
            this.toolStripStatusPOIDownload.Visible = false;
            this.toolStripStatusExport.Visible = false;

            this.serverAndCacheToolStripMenuItem.Checked = true;
            this.xPanderPanel2.ExpandClick += new EventHandler<EventArgs>(xPanderPanel2_ExpandClick);

            this.comboBoxStore.SelectedIndex = 0;
            this.comboBoxStore.SelectedIndexChanged += new EventHandler(comboBoxStore_SelectedIndexChanged);

            this.buttonMapImage.Click += new EventHandler(buttonMapImage_Click);

            this.dataGridViewPOI.AutoSize = true;
            this.dataGridViewPOI.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridViewPOI_RowPostPaint);
            this.comboBoxPoiSave.SelectedIndex = 0;
        }

        void dataGridViewPOI_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridViewPOI.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridViewPOI.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridViewPOI.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

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
            TreeNode rootNode = new TreeNode("中国");
            this.advTreeChina.Nodes.Add(rootNode);
            rootNode.Expand();

            //异步加载中国省市边界
            BackgroundWorker loadChinaWorker = new BackgroundWorker();
            loadChinaWorker.DoWork += new DoWorkEventHandler(loadChinaWorker_DoWork);
            loadChinaWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadChinaWorker_RunWorkerCompleted);
            loadChinaWorker.RunWorkerAsync();
        }

        private void InitCountryTree()
        {
            try
            {
                if (china.Province != null)
                {
                    foreach (var provice in china.Province)
                    {
                        TreeNode pNode = new TreeNode(provice.name);
                        pNode.Tag = provice;
                        if (provice.City != null)
                        {
                            foreach (var city in provice.City)
                            {
                                TreeNode cNode = new TreeNode(city.name);
                                cNode.Tag = city;
                                if (city.Piecearea != null)
                                {
                                    foreach (var piecearea in city.Piecearea)
                                    {
                                        TreeNode areaNode = new TreeNode(piecearea.name);
                                        areaNode.Tag = piecearea;
                                        cNode.Nodes.Add(areaNode);
                                    }
                                }
                                pNode.Nodes.Add(cNode);
                            }
                        }
                        TreeNode rootNode = this.advTreeChina.Nodes[0];
                        rootNode.Nodes.Add(pNode);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            this.advTreeChina.NodeMouseClick += new TreeNodeMouseClickEventHandler(advTreeChina_NodeMouseClick);
        }

        void advTreeChina_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.advTreeChina.SelectedNode = sender as TreeNode;
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
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
                        RectLatLng rect = GMapUtil.PolygonUtils.GetRegionMaxRect(polygon);
                        GMapTextMarker textMarker = new GMapTextMarker(rect.LocationMiddle, "双击下载");
                        regionOverlay.Clear();
                        regionOverlay.Polygons.Add(areaPolygon);
                        regionOverlay.Markers.Add(textMarker);
                        this.mapControl.SetZoomToFitRect(rect);
                    }
                }
            }
        }

        void loadChinaWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (china == null)
            {
                log.Error("加载中国省市边界失败！");
                return;
            }

            InitPOICountrySearchCondition();

            InitCountryTree();
        }

        void loadChinaWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //byte[] buffer = Properties.Resources.ChinaBoundary_Province_City;
                byte[] buffer = Properties.Resources.ChinaBoundary;
                china = GMapChinaRegion.ChinaMapRegion.GetChinaRegionFromJsonBinaryBytes(buffer);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion 

        #region 存储方式

        void comboBoxStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            int storeType = this.comboBoxStore.SelectedIndex;
            switch (storeType)
            {
                case 0:
                    mapControl.Manager.PrimaryCache = sqliteCache;
                    break;
                case 1:
                    mapControl.Manager.PrimaryCache = mysqlCache;
                    break;
                default:
                    mapControl.Manager.PrimaryCache = sqliteCache;
                    break;
            }
        }

        #endregion

        #region 地图下载

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
                if (!tileDownloader.IsComplete)
                {
                    CommonTools.MessageBox.ShowWarningMessage("正在下载地图，等待下载完成！");
                }
                else
                {
                    RectLatLng area = GMapUtil.PolygonUtils.GetRegionMaxRect(polygon);
                    try
                    {
                        DownloadCfgForm downloadCfgForm = new DownloadCfgForm(area, this.mapControl.MapProvider);
                        if (downloadCfgForm.ShowDialog() == DialogResult.OK)
                        {
                            TileDownloaderArgs downloaderArgs = downloadCfgForm.GetDownloadTileGPoints();
                            ResetToServerAndCacheMode();

                            if (this.comboBoxStore.SelectedIndex == 2)
                            {
                                tileDownloader.TilePath = this.tilePath;
                            }
                            tileDownloader.Retry = retryNum;
                            tileDownloader.PrefetchTileStart += new EventHandler<TileDownloadEventArgs>(tileDownloader_PrefetchTileStart);
                            tileDownloader.PrefetchTileProgress += new EventHandler<TileDownloadEventArgs>(tileDownloader_PrefetchTileProgress);
                            tileDownloader.PrefetchTileComplete += new EventHandler<TileDownloadEventArgs>(tileDownloader_PrefetchTileComplete);
                            tileDownloader.StartDownload(downloaderArgs);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        log.Error(ex);
                    }
                }
            }
            else
            {
                CommonTools.MessageBox.ShowTipMessage("请先用画图工具画下载的区域多边形或选择省市区域！");
            }
        }

        private void ShowDownloadTip(bool isVisible)
        {
            this.toolStripProgressBarDownload.Visible = isVisible;
            this.toolStripStatusDownload.Visible = isVisible;
        }

        void tileDownloader_PrefetchTileComplete(object sender, TileDownloadEventArgs e)
        {
            MessageBox.Show("地图下载完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            ShowDownloadTip(false);
        }

        private delegate void UpdateDownloadProress(int completedCount, int totalCount);

        private void UpdateDownloadBar(int completedCount, int totalCount)
        {
            if (this.toolStripProgressBarDownload.Visible)
            {
                int value = completedCount * 100 / totalCount;
                this.toolStripStatusDownload.Text = string.Format("下载进度：{0}/{1}", completedCount, totalCount);
                this.toolStripProgressBarDownload.Value = value;
            }
        }

        void tileDownloader_PrefetchTileProgress(object sender, TileDownloadEventArgs e)
        {
            if (e != null)
            {
                if (this.IsDisposed || !this.IsHandleCreated) return;
                this.Invoke(new UpdateDownloadProress(UpdateDownloadBar), e.TileCompleteNum, e.TileAllNum);
            }
        }

        void tileDownloader_PrefetchTileStart(object sender, TileDownloadEventArgs e)
        {
            ShowDownloadTip(true);
        }

        private void 下载地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DownloadMap(currentAreaPolygon);
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
            if (currentAreaPolygon != null)
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
                CommonTools.MessageBox.ShowTipMessage("请先用“矩形”画图工具选择区域");
            }
        }

        void tileImage_ImageTileComplete(object sender, EventArgs e)
        {
            MessageBox.Show("拼接图生成完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        #endregion

        #region 地图切换

        private void UpdateMainFormText(string mapName)
        {
            this.Text = "地图下载器" + "--" + mapName;
        }

        private void 福建街道地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.Fujian.TiandituFujianMapProviderWithAnno.Instance;
            UpdateMainFormText(GMapProvidersExt.TianDitu.Fujian.TiandituFujianMapProviderWithAnno.Instance.CnName);
            this.mapControl.Position = new PointLatLng(26.0651, 119.2786);
        }

        private void 福建卫星地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.Fujian.TiandituFujianSatelliteMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.TianDitu.Fujian.TiandituFujianSatelliteMapProvider.Instance.CnName);
            this.mapControl.Position = new PointLatLng(26.0651, 119.2786);
        }

        private void 福建混合地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.Fujian.TiandituFujianSatelliteMapProviderWithAnno.Instance;
            UpdateMainFormText(GMapProvidersExt.TianDitu.Fujian.TiandituFujianSatelliteMapProviderWithAnno.Instance.CnName);
            this.mapControl.Position = new PointLatLng(26.0651, 119.2786);
        }

        //船舶地图
        private void 船舶ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Ship.ShipMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Ship.ShipMapProvider.Instance.CnName);
        }

        //Google地图
        private void 普通地图ToolStripMenuItem6_Click_1(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProviders.GoogleMap;
            UpdateMainFormText("Google普通地图");
        }

        private void 卫星地图ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProviders.GoogleSatelliteMap;
            UpdateMainFormText("Google卫星地图");
        }

        private void 混合地图ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProviders.GoogleHybridMap;
            UpdateMainFormText("Google混合地图");
        }

        //Google中国地图
        private void 普通地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProviders.GoogleChinaMap;
            UpdateMainFormText("Google中国普通地图");
        }

        private void 卫星地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProviders.GoogleChinaSatelliteMap;
            UpdateMainFormText("Google中国卫星地图");
        }

        private void 混合地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProviders.GoogleChinaHybridMap;
            UpdateMainFormText("Google中国混合地图");
        }

        private void 地形图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProviders.GoogleChinaTerrainMap;
            UpdateMainFormText("Google中国地形地图");
        }

        //Baidu地图
        private void 普通地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Baidu.BaiduMapProvider.Instance.CnName);
        }

        private void 卫星地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduSatelliteMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Baidu.BaiduSatelliteMapProvider.Instance.CnName);
        }

        private void 混合地图ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Baidu.BaiduHybridMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Baidu.BaiduHybridMapProvider.Instance.CnName);
        }

        //高德地图
        private void 普通地图ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.AMap.AMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.AMap.AMapProvider.Instance.CnName);
        }

        private void 卫星地图ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.AMap.AMapSateliteProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.AMap.AMapSateliteProvider.Instance.CnName);
        }

        private void 混合地图ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.AMap.AMapHybirdProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.AMap.AMapHybirdProvider.Instance.CnName);
        }

        //SoSo地图
        private void 普通地图ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Tencent.TencentMapProvider.Instance;
            //this.mapControl.MapProvider = GMapProvidersExt.SoSo.SosoMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Tencent.TencentMapProvider.Instance.CnName);
        }

        private void 卫星地图ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Tencent.TencentMapSateliteProvider.Instance;
            //this.mapControl.MapProvider = GMapProvidersExt.SoSo.SosoMapSateliteProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Tencent.TencentMapSateliteProvider.Instance.CnName);
        }

        private void 混合地图ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Tencent.TencentMapHybridProvider.Instance;
            //this.mapControl.MapProvider = GMapProvidersExt.SoSo.SosoMapHybridProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Tencent.TencentMapHybridProvider.Instance.CnName);
        }

        private void 地形地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = TencentTerrainMapAnnoProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Tencent.TencentTerrainMapAnnoProvider.Instance.CnName);
        }

        //Here地图
        private void 普通地图ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProvidersExt.Here.NokiaMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Here.NokiaMapProvider.Instance.CnName);
        }

        private void 卫星地图ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProvidersExt.Here.NokiaSatelliteMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Here.NokiaSatelliteMapProvider.Instance.CnName);
        }

        private void 混合地图ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            mapControl.MapProvider = GMapProvidersExt.Here.NokiaHybridMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Here.NokiaHybridMapProvider.Instance.CnName);
        }

        //Bing地图
        private void 普通地图ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProviders.BingMap;
            UpdateMainFormText("Bing普通地图");
        }

        private void 卫星地图ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProviders.BingSatelliteMap;
            UpdateMainFormText("Bing卫星地图");
        }

        private void 混合地图ToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProviders.BingHybridMap;
            UpdateMainFormText("Bing混合地图");
        }

        private void 普通地图中文ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Bing.BingChinaMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Bing.BingChinaMapProvider.Instance.CnName);
        }

        //搜狗地图
        private void 普通地图ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.Sogou.SogouMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.Sogou.SogouMapProvider.Instance.CnName);
        }

        //天地图
        private void 街道地图球面墨卡托ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituMapProviderWithAnno.Instance;
            UpdateMainFormText(GMapProvidersExt.TianDitu.TiandituMapProviderWithAnno.Instance.CnName);
        }

        private void 卫星地图球面墨卡托ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituSatelliteMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.TianDitu.TiandituSatelliteMapProvider.Instance.CnName);
        }

        private void 混合地图球面墨卡托ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituSatelliteMapProviderWithAnno.Instance;
            UpdateMainFormText(GMapProvidersExt.TianDitu.TiandituSatelliteMapProviderWithAnno.Instance.CnName);
        }

        private void 街道地图WGS84ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituMapProviderWithAnno4326.Instance;
            UpdateMainFormText(GMapProvidersExt.TianDitu.TiandituMapProviderWithAnno4326.Instance.CnName);
        }

        private void 卫星地图WGS84ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituSatelliteMapProvider4326.Instance;
            UpdateMainFormText(GMapProvidersExt.TianDitu.TiandituSatelliteMapProvider4326.Instance.CnName);
        }

        private void 混合地图WGS84ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.TianDitu.TiandituSatelliteMapProviderWithAnno4326.Instance;
            UpdateMainFormText(GMapProvidersExt.TianDitu.TiandituSatelliteMapProviderWithAnno4326.Instance.CnName);
        }

        //ArcGIS地图
        private void arcGIS街道地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.ArcGIS.ArcGISMapProvider.Instance.CnName);
        }

        private void arcGIS街道地图无POIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISMapProviderNoPoi.Instance;
            UpdateMainFormText(GMapProvidersExt.ArcGIS.ArcGISMapProviderNoPoi.Instance.CnName);
        }

        private void arcGIS街道地图冷色版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISColdMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.ArcGIS.ArcGISColdMapProvider.Instance.CnName);
        }

        private void arcGIS街道地图灰色版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISGrayMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.ArcGIS.ArcGISGrayMapProvider.Instance.CnName);
        }

        private void arcGIS街道地图暖色版ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISWarmMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.ArcGIS.ArcGISWarmMapProvider.Instance.CnName);
        }

        private void arcGIS卫星地图无偏移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = GMapProvidersExt.ArcGIS.ArcGISSatelliteMapProvider.Instance;
            UpdateMainFormText(GMapProvidersExt.ArcGIS.ArcGISSatelliteMapProvider.Instance.CnName);
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
                if (e != null && (e.Polygon != null || e.Rectangle != null || e.Circle != null || e.Line != null || e.Route != null))
                {
                    GMapPolygon drawPolygon = null;
                    switch (e.DrawingMode)
                    {
                        case DrawingMode.Polygon:
                            drawPolygon = e.Polygon;
                            break;
                        case DrawingMode.Rectangle:
                            drawPolygon = e.Rectangle;
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

                    if (drawPolygon != null)
                    {
                        GMapAreaPolygon areaPolygon = new GMapAreaPolygon(drawPolygon.Points, "下载区域");
                        currentAreaPolygon = areaPolygon;
                        RectLatLng rect = GMapUtil.PolygonUtils.GetRegionMaxRect(currentAreaPolygon);
                        GMapTextMarker textMarker = new GMapTextMarker(rect.LocationMiddle, "双击下载");
                        regionOverlay.Clear();
                        regionOverlay.Polygons.Add(areaPolygon);
                        regionOverlay.Markers.Add(textMarker);
                        this.mapControl.SetZoomToFitRect(rect);
                    }
                }
            }
            finally
            {
                draw.IsEnable = false;
            }
        }

        #endregion

        #region 测距

        private void 测距ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            drawDistance.IsEnable = true;
        }

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

        private void 显示网格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.显示网格ToolStripMenuItem.Checked = !this.显示网格ToolStripMenuItem.Checked;
            if (this.显示网格ToolStripMenuItem.Checked)
            {
                this.mapControl.ShowTileGridLines = true;
            }
            else
            {
                this.mapControl.ShowTileGridLines = false;
            }
        }

        #endregion

        #region KML GPX 操作

        private void 读取GPXToolStripMenuItem_Click(object sender, EventArgs e)
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
                        log.Error("Read GPX file error: " + exception);
                        CommonTools.MessageBox.ShowTipMessage("读取GPX文件错误");
                    }
                }
            }
        }

        private void 读取KMLToolStripMenuItem_Click(object sender, EventArgs e)
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
                log.Error("Read KML file error: " + exception);
                CommonTools.MessageBox.ShowTipMessage("读取KML文件时出现异常");
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

        #endregion

        #region POI查询

        private List<PoiData> poiDataList = new List<PoiData>();
        private List<Placemark> poisQueryResult = new List<Placemark>();
        private int poiQueryCount = 0;
        private string searchProvince;
        private string searchCity;

        private void queryProgressEvent(long completedCount, long total)
        {
            this.toolStripStatusPOIDownload.Text = string.Format("已找到{0}条POI，还在查询中...", completedCount);
        }

        void poiWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (poisQueryResult != null && poisQueryResult.Count > 0)
            {
                foreach (Placemark place in poisQueryResult)
                {
                    GMarkerGoogle marker = new GMarkerGoogle(place.Point, GMarkerGoogleType.blue_dot);
                    marker.ToolTipText = place.Name + "\r\n" + place.Address + "\r\n" + place.Category;
                    this.poiOverlay.Markers.Add(marker);
                    PoiData poiData = new PoiData();
                    poiData.Name = place.Name;
                    poiData.Address = place.Address;
                    poiData.Province = searchProvince;
                    poiData.City = searchCity;
                    poiData.Lat = place.Point.Lat;
                    poiData.Lng = place.Point.Lng;
                    this.poiDataList.Add(poiData);
                }

                this.dataGridViewPOI.DataSource = poiDataList;
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
                int mapIndex = argument.MapIndex;
                this.poiDataList.Clear();
                this.poisQueryResult.Clear();
                this.poiQueryCount = 0;
                switch (mapIndex)
                {
                    //百度
                    case 0:
                        BaiduMapProvider.Instance.GetPlacemarksByKeywords(keyWords, regionName, poiQueryRectangleStr, this.queryProgressEvent, out this.poisQueryResult, ref this.poiQueryCount);
                        break;
                    //腾讯
                    case 1:
                        TencentMapProvider.Instance.GetPlacemarksByKeywords(keyWords, regionName, poiQueryRectangleStr,
                        "", this.queryProgressEvent, out this.poisQueryResult, ref this.poiQueryCount);
                        break;
                    //高德
                    case 2:
                        AMapProvider.Instance.GetPlacemarksByKeywords(keyWords, regionName, poiQueryRectangleStr, this.queryProgressEvent, out this.poisQueryResult, ref this.poiQueryCount);
                        break;
                }
            }
        }

        //关键字POI查询
        private void buttonPOISearch_Click(object sender, EventArgs e)
        {
            Province province = this.comboBoxProvince.SelectedItem as Province;
            if (province == null)
            {
                CommonTools.MessageBox.ShowTipMessage("请选择POI查询的省份！");
                return;
            }
            searchProvince = province.name;

            City city = this.comboBoxCity.SelectedItem as City;
            if (city == null)
            {
                CommonTools.MessageBox.ShowTipMessage("请选择POI查询的城市！");
                return;
            }
            searchCity = city.name;

            string keywords = this.textBoxPOIkeyword.Text.Trim();
            if (string.IsNullOrEmpty(keywords))
            {
                CommonTools.MessageBox.ShowTipMessage("请输入POI查询的关键字！");
                return;
            }

            int selectMapIndex = this.comboBoxPOIMap.SelectedIndex;
            GetPOIFromMap(searchCity, keywords, selectMapIndex);
        }

        private void GetPOIFromMap(string cityName, string keywords, int mapIndex)
        {
            this.poiOverlay.Markers.Clear();
            this.dataGridViewPOI.DataSource = null;
            this.dataGridViewPOI.Update();
            this.poiDataList.Clear();
            POISearchArgument argument = new POISearchArgument();
            argument.KeyWord = keywords;
            argument.Region = cityName;
            argument.MapIndex = mapIndex;
            if (currentAreaPolygon != null)
            {
                RectLatLng rect = GMapUtil.PolygonUtils.GetRegionMaxRect(currentAreaPolygon);
                argument.Rectangle = string.Format("{0},{1},{2},{3}",
                    new object[] { rect.LocationRightBottom.Lat, rect.LocationTopLeft.Lng, rect.LocationTopLeft.Lat, rect.LocationRightBottom.Lng });
            }

            toolStripStatusPOIDownload.Visible = true;
            BackgroundWorker poiWorker = new BackgroundWorker();
            poiWorker.DoWork += new DoWorkEventHandler(poiWorker_DoWork);
            poiWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(poiWorker_RunWorkerCompleted);
            poiWorker.RunWorkerAsync(argument);
        }

        private void InitPOISearch()
        {
            if (!isCountryLoad)
            {
                InitChinaRegion();
                isCountryLoad = true;
            }
            this.comboBoxPOIMap.SelectedIndex = 0;
        }

        private void InitPOICountrySearchCondition()
        {
            if (china != null)
            {
                foreach (var provice in china.Province)
                {
                    this.comboBoxProvince.Items.Add(provice);
                }
                this.comboBoxProvince.DisplayMember = "name";
                //this.comboBoxProvince.SelectedIndex = 0;
                this.comboBoxProvince.SelectedValueChanged += ComboBoxProvince_SelectedValueChanged;
            }
        }

        private void ComboBoxProvince_SelectedValueChanged(object sender, EventArgs e)
        {
            Province province = this.comboBoxProvince.SelectedItem as Province;
            if (province != null)
            {
                this.comboBoxCity.Items.Clear();
                foreach (var city in province.City)
                {
                    this.comboBoxCity.Items.Add(city);
                }
                this.comboBoxCity.DisplayMember = "name";
                this.comboBoxCity.SelectedIndex = 0;
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
        }

        private void 清楚边界ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.regionOverlay.Clear();
            currentAreaPolygon = null;
        }

        private void 清楚路径ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.routeOverlay.Clear();
        }

        private void 清除POIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.poiOverlay.Clear();
        }

        #endregion

        #region 离线Web服务

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

        private void 离线Web服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.离线Web服务ToolStripMenuItem.Checked = !this.离线Web服务ToolStripMenuItem.Checked;
            if (离线Web服务ToolStripMenuItem.Checked)
            {
                try
                {
                    mapControl.Manager.EnableTileHost(8899);
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

        #region 导出切片

        private void 导出地图切片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArcGISLayerConfigForm form = new ArcGISLayerConfigForm();
            DialogResult res = form.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                ExportParameter exportParameter = form.GetExportParameter();
                TileExport tileExport = new TileExport();
                tileExport.TileExportStart += new EventHandler<TileExportEventArgs>(tileExport_TileExportStart);
                tileExport.TileExportComplete += new EventHandler<TileExportEventArgs>(tileExport_TileExportComplete);
                tileExport.TileExportProgress += new EventHandler<TileExportEventArgs>(tileExport_TileExportProgress);
                tileExport.Start(exportParameter, this.conString);
            }
        }

        void tileExport_TileExportProgress(object sender, TileExportEventArgs e)
        {
            if (e != null)
            {
                this.toolStripStatusExport.Text = string.Format("正在导出第{0}级的切片...", e.CurrentExportZoom);
            }
        }

        void tileExport_TileExportComplete(object sender, TileExportEventArgs e)
        {
            MessageBox.Show("切片导出完成！");
            this.toolStripStatusExport.Visible = false;
        }

        void tileExport_TileExportStart(object sender, TileExportEventArgs e)
        {
            this.toolStripStatusExport.Visible = true;
        }

        #endregion

        #region POI导出

        private void buttonPoiSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (poiDataList.Count <= 0)
                {
                    CommonTools.MessageBox.ShowTipMessage("POI数据为空，无法保存！");
                    return;
                }
                BackgroundWorker poiExportWorker = new BackgroundWorker();
                poiExportWorker.DoWork += new DoWorkEventHandler(poiExportWorker_DoWork);
                poiExportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(poiExportWorker_RunWorkerCompleted);

                int selectIndex = this.comboBoxPoiSave.SelectedIndex;
                if (selectIndex == 0)
                {
                    SaveFileDialog saveDlg = new SaveFileDialog();
                    saveDlg.Filter = "Excel File (*.xls)|*.xls|(*.xlsx)|*.xlsx";
                    saveDlg.FilterIndex = 1;
                    saveDlg.RestoreDirectory = true;
                    if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string file = saveDlg.FileName;

                        DataTable dt = new DataTable();
                        dt.Columns.Add("名称", typeof(string));
                        dt.Columns.Add("地址", typeof(string));
                        dt.Columns.Add("省份", typeof(string));
                        dt.Columns.Add("城市", typeof(string));
                        dt.Columns.Add("经度", typeof(double));
                        dt.Columns.Add("纬度", typeof(double));

                        foreach (PoiData data in poiDataList)
                        {
                            DataRow dr = dt.NewRow();
                            dr["名称"] = data.Name;
                            dr["地址"] = data.Address;
                            dr["省份"] = data.Province;
                            dr["城市"] = data.City;
                            dr["经度"] = data.Lng;
                            dr["纬度"] = data.Lat;
                            dt.Rows.Add(dr);
                        }
                        PoiExportParameter para = new PoiExportParameter();
                        para.Path = file;
                        para.Data = dt;
                        para.ExportType = selectIndex;
                        poiExportWorker.RunWorkerAsync(para);
                    }
                }
                else if (selectIndex == 1)
                {
                    PoiExportParameter para = new PoiExportParameter();
                    para.ExportType = selectIndex;
                    poiExportWorker.RunWorkerAsync(para);
                }
            }
            catch (Exception ex)
            {
                log.Error("Save POI data error: " + ex);
                MessageBox.Show("POI保存失败！");
            }
        }

        void poiExportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("POI保存完成！");
        }

        void poiExportWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e != null)
            {
                PoiExportParameter para = e.Argument as PoiExportParameter;
                if (para.ExportType == 0)
                {
                    DataTable dt = para.Data;
                    string file = para.Path;
                    ExcelHelper.DataTableToExcel(dt, file, null, true);
                }
                else if (para.ExportType == 1)
                {
                    MySQLPoiCache mysqlPoiCache = new MySQLPoiCache(this.conString);
                    bool isInitialized = mysqlPoiCache.Initialize();
                    if (!isInitialized)
                    {
                        MessageBox.Show("数据库初始化失败！");
                        return;
                    }
                    //Export data into database
                    foreach (var data in poiDataList)
                    {
                        mysqlPoiCache.PutPoiDataToCache(data);
                    }
                }
            }
        }

        #endregion 

        private List<PointLatLng> GetRandomPoint()
        {
            Random rand = new Random();
            List<PointLatLng> points = new List<PointLatLng>();
            int pointNum = 500;
            for (int i = 0; i < pointNum; ++i)
            {
                double x = 118 + rand.NextDouble() * 0.1 + rand.NextDouble() * 0.1 * 0.1 + rand.NextDouble();
                double y = 31.5 + rand.NextDouble() * 0.1 + rand.NextDouble() * 0.1 * 0.1 + rand.NextDouble();
                points.Add(new PointLatLng(y, x));
            }

            return points;
        }

        GMapHeatImage heatMarker = null;
        RectLatLng heatRect = RectLatLng.Empty;

        private void 热力图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int zoom = (int)this.mapControl.Zoom;

            List<PointLatLng> ps = GetRandomPoint();
            foreach (var p in ps)
            {
                GMapPointMarker pointmarker = new GMapPointMarker(p);
                //this.poiOverlay.Markers.Add(pointmarker);
            }

            //热力图范围
            heatRect = GMapUtils.GetPointsMaxRect(ps);
            GPoint plt = this.mapControl.MapProvider.Projection.FromLatLngToPixel(heatRect.LocationTopLeft, zoom);
            GPoint prb = this.mapControl.MapProvider.Projection.FromLatLngToPixel(heatRect.LocationRightBottom, zoom);

            List<HeatPoint> hps = new List<HeatPoint>();
            foreach (var p in ps)
            {
                GPoint gp = this.mapControl.MapProvider.Projection.FromLatLngToPixel(p, zoom);
                HeatPoint hp = new HeatPoint();
                hp.X = gp.X - plt.X;
                hp.Y = gp.Y - plt.Y;
                hp.W = 1.0f;
                hps.Add(hp);
            }

            int width = (int)(prb.X - plt.X);
            int height = (int)(prb.Y - plt.Y);

            var hmMaker = new HeatMapMaker
            {
                Width = width,
                Height = height,
                Radius = 10,
                ColorRamp = ColorRamp.RAINBOW,
                HeatPoints = hps,
                Opacity = 111
            };
            Bitmap bitmap = hmMaker.MakeHeatMap();
            heatMarker = new GMapHeatImage(heatRect.LocationTopLeft, bitmap);
            this.poiOverlay.Markers.Add(heatMarker);
        }

        private void 地图截屏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image img = this.mapControl.ToImage();
            SaveFileDialog openDialog = new SaveFileDialog();
            openDialog.Filter = "(*.png)|*.png|(*.jpg)|*.jpg|(*.bmp)|*.bmp";
            if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openDialog.FileName;
                img.Save(fileName);
            }
        }

        private void arcGISTileToBundleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArcGISTileToBundleForm tileToBundleForm = new ArcGISTileToBundleForm();
            tileToBundleForm.ShowDialog();
        }

        private void 代理设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProxyForm proxyForm = new ProxyForm();
            DialogResult diaResult = proxyForm.ShowDialog();
            if (diaResult == System.Windows.Forms.DialogResult.OK)
            {
                bool isProxyOn = proxyForm.CheckProxyOn();
                if (isProxyOn)
                {
                    string ip = proxyForm.GetProxyIp();
                    int port = proxyForm.GetProxyPort();
                    // set your proxy here if need
                    GMapProvider.IsSocksProxy = true;
                    GMapProvider.WebProxy = new WebProxy(ip, port);
                }
                else
                {
                    GMapProvider.IsSocksProxy = false;
                }
            }
        }


    }
}
