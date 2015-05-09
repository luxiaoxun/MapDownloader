using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;
using GMap.NET.CacheProviders;
using GMapDrawTools;
using NetUtilityLib;
using GMapChinaRegion;
using MySql.Data.MySqlClient;

namespace MapDownloader
{
    public partial class MapDownloadForm : DevComponents.DotNetBar.Office2007Form
    {
        //private string conString = @"Server=127.0.0.1;Port=3306;Database=mapcache;Uid=root;Pwd=admin;";

        #region 比例尺变量

        /// <summary>
        /// The font for the m/km markers
        /// </summary>
        private Font fontCustomScale = new Font("Arial", 6);

        /// <summary>
        /// The font for the scale header 
        /// </summary>
        private Font fontCustomScaleBold = new Font("Arial", 10, FontStyle.Bold);

        /// <summary>
        /// The brush for the scale's background
        /// </summary>
        private Brush brushCustomScaleBackColor = new SolidBrush(Color.FromArgb(180, 185, 215, 255));

        /// <summary>
        /// The Textcolor for the scale's fonts
        /// </summary>
        private Color colorCustomScaleText = Color.FromArgb(20, 65, 140);

        /// <summary>
        /// The width of the scale-rectangle
        /// </summary>
        private int intScaleRectWidth = 300;

        /// <summary>
        /// The height of the scale-rectangle
        /// </summary>
        private int intScaleRectHeight = 50;

        /// <summary>
        /// The height of the scale bar
        /// </summary>
        private int intScaleBarHeight = 10;

        /// <summary>
        /// The padding of the scale
        /// </summary>
        private int intScaleLeftPadding = 10;

        #endregion

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

        public MapDownloadForm()
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

            draw = new Draw(this.mapControl);
            draw.DrawComplete += new EventHandler<DrawEventArgs>(draw_DrawComplete);
        }

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
            if (china == null) return;
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
                        regionOverlay.Polygons.Clear();
                        regionOverlay.Polygons.Add(polygon);
                        RectLatLng rect = GMapChinaRegion.ChinaMapRegion.GetRegionMaxRect(polygon);
                        this.mapControl.SetZoomToFitRect(rect);
                        selectedRect = rect;
                    }
                }
            }
        }

        void loadChinaWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string file = System.Windows.Forms.Application.StartupPath + "\\chinaBoundry";
            if (System.IO.File.Exists(file))
            {
                china = GMapChinaRegion.ChinaMapRegion.GetChinaRegionFromJsonFile(file);
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
                retryNum = int.Parse(retryStr);

                conString = string.Format(conStringFormat, ip, port, dbName, userID, password);
                mysqlCache.ConnectionString = conString;

                tilePath = ConfigHelper.GetAppConfig("TilePath");
            }
            catch (Exception ignore)
            {

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
        private RectLatLng GetRectLatLngFromDrawing(GMapDrawRectangle rectangle)
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

        //Mouse move 事件
        void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                PointLatLng p = this.mapControl.FromLocalToLatLng(e.X, e.Y);

                int zoom = (int)this.mapControl.Zoom;
                double resolution = this.mapControl.MapProvider.Projection.GetGroundResolution(zoom, this.mapControl.Position.Lat);

                this.toolStripStatusTip.Text = string.Format("显示级别：{0} 分辨率：{1:F3}米/像素 坐标：{2:F4},{3:F4}",zoom,resolution,p.Lng,p.Lat );
            }
            catch (Exception ignore)
            {
            }
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
                UpdateDownloadBar(e.ProgressValue);
            }
        }

        void obj_PrefetchTileStart(object sender, PrefetchTileEventArgs e)
        {
            ShowDownloadTip(true);
        }

        private void ShowDownloadTip(bool isVisible)
        {
            this.toolStripStatusDownload.Visible = isVisible;
            this.toolStripProgressBarDownload.Visible = isVisible;
        }

        private void UpdateDownloadBar(int value)
        {
            this.toolStripProgressBarDownload.Value = value;
            if (this.toolStripProgressBarDownload.Maximum == value)
            {
                ShowDownloadTip(false);
                MessageBox.Show("地图下载完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void ResetToServerAndCacheMode()
        {
            this.mapControl.Manager.Mode = AccessMode.ServerAndCache;
            this.serverAndCacheToolStripMenuItem.Checked = true;
            this.本地缓存ToolStripMenuItem.Checked = false;
            this.在线服务ToolStripMenuItem.Checked = false;
        }

        //下载
        private void buttonDownload_Click(object sender, EventArgs e)
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
                        PrefetchTiles obj = new PrefetchTiles();
                        obj.Retry = retryNum;
                        obj.PrefetchTileStart += new EventHandler<PrefetchTileEventArgs>(obj_PrefetchTileStart);
                        obj.PrefetchTileProgress += new EventHandler<PrefetchTileEventArgs>(obj_PrefetchTileProgress);
                        if (this.radioButtonDisk.Checked)
                        {
                            //切片存在本地磁盘上
                            obj.Start(area, minZ, maxZ, new GMapProvider[] { mapControl.MapProvider }, 100, tilePath);
                        }
                        else
                        {
                            //切片存在数据库中
                            obj.Start(area, minZ, maxZ, new GMapProvider[] { mapControl.MapProvider }, 100);
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
                MessageBox.Show("请先用“矩形”画图工具选择区域", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
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
                MessageBox.Show("请先用“矩形”画图工具选择区域", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

    }
}
