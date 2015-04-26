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

        private string tilePath = "D:\\gismap";

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

            mapControl.MapProvider = GMapProviders.GoogleChinaMap;
            mapControl.Position = new PointLatLng(32.043, 118.773);
            mapControl.MinZoom = 1;
            mapControl.MaxZoom = 18;
            mapControl.Zoom = 9;

            mapControl.Overlays.Add(polygonsOverlay);
            mapControl.Overlays.Add(regionOverlay);

            this.mapControl.MouseMove += new MouseEventHandler(mapControl_MouseMove);

            draw = new Draw(this.mapControl);
            draw.DrawComplete += new EventHandler<DrawEventArgs>(draw_DrawComplete);

            this.radioButtonMySQL.CheckedChanged += new EventHandler(radioButtonMySQL_CheckedChanged);
            this.radioButtonSQLite.CheckedChanged += new EventHandler(radioButtonSQLite_CheckedChanged);
            this.buttonDownload.Click +=new EventHandler(buttonDownload_Click);
            this.buttonMapImage.Click += new EventHandler(buttonMapImage_Click);
         
            this.checkBoxItemShowGrid.CheckedChanged += new DevComponents.DotNetBar.CheckBoxChangeEventHandler(checkBoxItemShowGrid_CheckedChanged);
            this.buttonItemTileLocal.Click += new EventHandler(buttonItemTileLocal_Click);
        }

        //初始化UI
        private void InitUI()
        {
            ShowDownloadTip(false);

            this.xPanderPanel2.ExpandClick += new EventHandler<EventArgs>(xPanderPanel2_ExpandClick);
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
            string ip = ConfigHelper.GetAppConfig("MySQLServerIP");
            string port = ConfigHelper.GetAppConfig("MySQLServerPort");
            string dbName = ConfigHelper.GetAppConfig("Database");
            string userID = ConfigHelper.GetAppConfig("UserID");
            string password = ConfigHelper.GetAppConfig("Password");

            conString = string.Format(conStringFormat, ip, port, dbName, userID, password);

            tilePath = ConfigHelper.GetAppConfig("TilePath");
        }

        //存为本地切片底图图源
        void buttonItemTileLocal_Click(object sender, EventArgs e)
        {
            MySqlCommand cmdFetch;
            MySqlConnection cnGet;
            cnGet = new MySqlConnection(conString);
            cnGet.Open();
            cmdFetch = new MySqlCommand("SELECT * FROM `gmapnetcache` where Type=1818940751", cnGet);
            cmdFetch.Prepare();
            try
            {
                MySqlDataReader odata = null;
                lock (cmdFetch)
                {
                    cnGet.Ping();

                    if (cnGet.State != ConnectionState.Open)
                    {
                        cnGet.Open();
                    }

                    odata = cmdFetch.ExecuteReader();
                    DirectoryInfo di = new DirectoryInfo(tilePath);
                    while (odata.Read())
                    {
                        int type = odata.GetInt32("Type");
                        if (type == 1818940751) //只获取一种
                        {
                            int zoom = odata.GetInt32("Zoom");
                            string zoomStr = string.Format("{0:D2}", zoom);

                            long x = odata.GetInt64("X");
                            long y = odata.GetInt64("Y");
                            string col = string.Format("{0:x8}", x).ToLower();
                            string row = string.Format("{0:x8}", y).ToLower();
                            string dir = di.FullName + "//" + type + "//" + "L" + zoomStr + "//" + "R" + row + "//";
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }

                            FileStream fs1 = new FileStream(dir + "C" + col + ".png", FileMode.Create, FileAccess.Write);
                            BinaryWriter sw1 = new BinaryWriter(fs1);
                            //读出图片字节数组至byte[] 
                            byte[] imageByte = new byte[odata.GetBytes(odata.GetOrdinal("Tile"), 0, null, 0, int.MaxValue)];
                            odata.GetBytes(odata.GetOrdinal("Tile"), 0, imageByte, 0, imageByte.Length);
                            sw1.Write(imageByte);
                            sw1.Close();
                            fs1.Close();
                        }
                    }

                    MessageBox.Show("获取完成！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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

        #region 拼接大图

        void buttonMapImage_Click(object sender, EventArgs e)
        {
            RectLatLng area = selectedRect;
            if (!area.IsEmpty)
            {
                try
                {
                    BackgroundWorker imageGenWorker = new BackgroundWorker();
                    imageGenWorker.DoWork += new DoWorkEventHandler(imageGenWorker_DoWork);
                    imageGenWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(imageGenWorker_RunWorkerCompleted);
                    imageGenWorker.RunWorkerAsync(area);

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

        void imageGenWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("拼接图生成完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        void imageGenWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                RectLatLng area = (RectLatLng)e.Argument;
                int zoom = int.Parse(this.textBoxImageZoom.Text);
                List<GPoint> tileArea = mapControl.MapProvider.Projection.GetAreaTileList(area, zoom, 0);
                string bigImage = zoom + "-" + Guid.NewGuid().ToString() + "-.png";

                // current area
                GPoint topLeftPx = mapControl.MapProvider.Projection.FromLatLngToPixel(area.LocationTopLeft, zoom);
                GPoint rightButtomPx = mapControl.MapProvider.Projection.FromLatLngToPixel(area.Bottom, area.Right, zoom);
                GPoint pxDelta = new GPoint(rightButtomPx.X - topLeftPx.X, rightButtomPx.Y - topLeftPx.Y);

                int padding = 22;
                using (Bitmap bmpDestination = new Bitmap((int)(pxDelta.X + padding * 2), (int)(pxDelta.Y + padding * 2)))
                {
                    using (Graphics gfx = Graphics.FromImage(bmpDestination))
                    {
                        gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                        // get tiles & combine into one
                        foreach (var p in tileArea)
                        {
                            //Console.WriteLine("Downloading[" + p + "]: " + tileArea.IndexOf(p) + " of " + tileArea.Count);

                            foreach (var tp in mapControl.MapProvider.Overlays)
                            {
                                Exception ex;
                                GMapImage tile = GMaps.Instance.GetImageFrom(tp, p, zoom, out ex) as GMapImage;
                                if (tile != null)
                                {
                                    using (tile)
                                    {
                                        long x = p.X * mapControl.MapProvider.Projection.TileSize.Width - topLeftPx.X + padding;
                                        long y = p.Y * mapControl.MapProvider.Projection.TileSize.Width - topLeftPx.Y + padding;
                                        gfx.DrawImage(tile.Img, x, y, mapControl.MapProvider.Projection.TileSize.Width, mapControl.MapProvider.Projection.TileSize.Height);
                                    }
                                }
                            }
                        }
                    }

                    #region draw bounds & coordinates & scale
                    //System.Drawing.Rectangle rect = new System.Drawing.Rectangle();
                    //{
                    //    rect.Location = new System.Drawing.Point(padding, padding);
                    //    rect.Size = new System.Drawing.Size((int)pxDelta.X, (int)pxDelta.Y);
                    //}
                    //using (Font f = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold))
                    //using (Graphics gfx = Graphics.FromImage(bmpDestination))
                    //{
                    //// draw bounds & coordinates
                    //using (Pen p = new Pen(Brushes.Red, 3))
                    //{
                    //    p.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;

                    //    gfx.DrawRectangle(p, rect);

                    //    string topleft = area.LocationTopLeft.ToString();
                    //    SizeF s = gfx.MeasureString(topleft, f);

                    //    gfx.DrawString(topleft, f, p.Brush, rect.X + s.Height / 2, rect.Y + s.Height / 2);

                    //    string rightBottom = new PointLatLng(area.Bottom, area.Right).ToString();
                    //    SizeF s2 = gfx.MeasureString(rightBottom, f);

                    //    gfx.DrawString(rightBottom, f, p.Brush, rect.Right - s2.Width - s2.Height / 2, rect.Bottom - s2.Height - s2.Height / 2);
                    //}

                    //// draw scale
                    //using (Pen p = new Pen(Brushes.Blue, 1))
                    //{
                    //    double rez = mapControl.MapProvider.Projection.GetGroundResolution(zoom, area.Bottom);
                    //    int px100 = (int)(100.0 / rez); // 100 meters
                    //    int px1000 = (int)(1000.0 / rez); // 1km   

                    //    gfx.DrawRectangle(p, rect.X + 10, rect.Bottom - 20, px1000, 10);
                    //    gfx.DrawRectangle(p, rect.X + 10, rect.Bottom - 20, px100, 10);

                    //    string leftBottom = "scale: 100m | 1Km";
                    //    SizeF s = gfx.MeasureString(leftBottom, f);
                    //    gfx.DrawString(leftBottom, f, p.Brush, rect.X + 10, rect.Bottom - s.Height - 20);
                    //}
                    //}

                    #endregion

                    bmpDestination.Save(bigImage, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("拼接图生成错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        void radioButtonSQLite_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonSQLite.Checked)
            {
                GMap.NET.CacheProviders.SQLitePureImageCache cache = new GMap.NET.CacheProviders.SQLitePureImageCache();
                mapControl.Manager.PrimaryCache = cache;
            }
        }

        void radioButtonMySQL_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonMySQL.Checked)
            {
                GMap.NET.CacheProviders.MySQLPureImageCache cache = new GMap.NET.CacheProviders.MySQLPureImageCache();
                cache.ConnectionString = conString;
                mapControl.Manager.PrimaryCache = cache;
            }
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

        #region 地图下载

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            RectLatLng area = selectedRect;
            if (!area.IsEmpty)
            {
                try
                {
                    ShowDownloadTip(true);
                    int minZ = int.Parse(this.textBoxMinZoom.Text);
                    int maxZ = int.Parse(this.textBoxMaxZoom.Text);
                    minZ = minZ <= 0 ? 1 : minZ;
                    maxZ = maxZ >= mapControl.MaxZoom ? mapControl.MaxZoom : maxZ;
                    if (minZ <= maxZ)
                    {
                        this.toolStripProgressBarDownload.Maximum = maxZ;
                        this.toolStripProgressBarDownload.Minimum = minZ;
                        for (int i = minZ; i <= maxZ; i++)
                        {
                            BackgroundWorker worker = new BackgroundWorker();
                            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                            MapAreaInfo mapAreaInfo = new MapAreaInfo(i, area);
                            worker.RunWorkerAsync(mapAreaInfo);
                        }
                    }
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

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int zoom = (int)e.Result;
            string text = "Zoom为" + zoom + "的地图下载完成";
            UpdateDownloadBar(zoom);
            //this.listBox1.Items.Add(text);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            MapAreaInfo mapAreaInfo = e.Argument as MapAreaInfo;
            if (mapAreaInfo != null)
            {
                using (TilePrefetcher obj = new TilePrefetcher())
                {
                    obj.Shuffle = mapControl.Manager.Mode != AccessMode.CacheOnly;
                    obj.Owner = this;
                    obj.ShowCompleteMessage = false;
                    obj.ShowProgressMessage = false;
                    obj.Start(mapAreaInfo.Area, mapAreaInfo.Zoom, mapControl.MapProvider, mapControl.Manager.Mode == AccessMode.CacheOnly ? 0 : 100, mapControl.Manager.Mode == AccessMode.CacheOnly ? 0 : 1);
                    e.Result = mapAreaInfo.Zoom;
                }
            }
        }

        #endregion

        //protected override void OnPaintOverlays(System.Drawing.Graphics g)
        //{
        //    base.OnPaintOverlays(g);

        //    if (ShowScale)
        //    {
        //        double resolution = this.MapProvider.Projection.GetGroundResolution((int)this.Zoom, Position.Lat);

        //        int px10 = (int)(10.0 / resolution);            // 10 meters
        //        int px100 = (int)(100.0 / resolution);          // 100 meters
        //        int px1000 = (int)(1000.0 / resolution);        // 1km   
        //        int px10000 = (int)(10000.0 / resolution);      // 10km  
        //        int px100000 = (int)(100000.0 / resolution);    // 100km  
        //        int px1000000 = (int)(1000000.0 / resolution);  // 1000km
        //        int px5000000 = (int)(5000000.0 / resolution);  // 5000km

        //        //Check how much width we have and set the scale accordingly
        //        int availableWidth = (intScaleRectWidth - 2 * intScaleLeftPadding);

        //        //5000 kilometers:
        //        if (availableWidth >= px5000000)
        //            DrawScale(g, px5000000, availableWidth, 5000, "km");
        //        //1000 kilometers:
        //        else if (availableWidth >= px1000000)
        //            DrawScale(g, px1000000, availableWidth, 1000, "km");
        //        //100 kilometers:
        //        else if (availableWidth >= px100000)
        //            DrawScale(g, px100000, availableWidth, 100, "km");
        //        //10 kilometers:
        //        else if (availableWidth >= px10000)
        //            DrawScale(g, px10000, availableWidth, 10, "km");
        //        //1 kilometers:
        //        else if (availableWidth >= px1000)
        //            DrawScale(g, px1000, availableWidth, 1, "km");
        //        //100 meters:
        //        else if (availableWidth >= px100)
        //            DrawScale(g, px100, availableWidth, 100, "m");
        //        //10 meters:
        //        else if (availableWidth >= px10)
        //            DrawScale(g, px10, availableWidth, 10, "m");
        //    }
        //}

        //private void DrawScale(System.Drawing.Graphics g, int resLength, int availableWidth, int totalDimenson, String unit)
        //{
        //    Point p = new System.Drawing.Point(this.Width - (intScaleRectWidth + 10), this.Height - (intScaleRectHeight + 10));
        //    Rectangle rect = new Rectangle(p, new Size(intScaleRectWidth, intScaleRectHeight));
        //    g.FillRectangle(brushCustomScaleBackColor, rect);
        //    Pen pen = new Pen(colorCustomScaleText, 1);
        //    g.DrawRectangle(pen, rect);
        //    SizeF stringSize = new SizeF();
        //    Point pos = new Point();

        //    //Header:
        //    String scaleString = @"比例尺";
        //    stringSize = g.MeasureString(scaleString, fontCustomScaleBold);
        //    pos = new Point(p.X + (rect.Width - (int)stringSize.Width) / 2, p.Y + 3);
        //    g.DrawString(scaleString, fontCustomScaleBold, pen.Brush, pos);

        //    pos = new Point(p.X + intScaleLeftPadding, pos.Y + 30);

        //    //How many rectangles fit?
        //    int numRects = availableWidth / resLength;
        //    Size rectSize = new Size(resLength, intScaleBarHeight);
        //    //Center rectangle
        //    pos.X += (availableWidth - resLength * numRects) / 2;
        //    //Draw rectangles:
        //    for (int i = 0; i < numRects; i++)
        //    {
        //        Rectangle r = new Rectangle(pos, rectSize);
        //        if (i % 2 == 0)
        //            g.FillRectangle(pen.Brush, r);
        //        else
        //            g.DrawRectangle(pen, r);
        //        //Draw little vertical lines
        //        g.DrawLine(pen, pos, new Point(pos.X, pos.Y - 5));
        //        //Draw labels:
        //        int dist = i * totalDimenson;
        //        stringSize = g.MeasureString(dist + " " + unit, fontCustomScale);
        //        g.DrawString(dist + " " + unit, fontCustomScale, pen.Brush, new Point(pos.X - (int)stringSize.Width / 2, pos.Y - (7 + (int)stringSize.Height)));
        //        //Finally set new point
        //        pos = new Point(pos.X + resLength, pos.Y);
        //    }
        //    //Draw last line:
        //    g.DrawLine(pen, pos, new Point(pos.X, pos.Y - 5));
        //    //Draw last label
        //    int m = numRects * totalDimenson;
        //    stringSize = g.MeasureString(m + " " + unit, fontCustomScale);
        //    g.DrawString(m + " " + unit, fontCustomScale, pen.Brush, new Point(pos.X - (int)stringSize.Width / 2, pos.Y - (7 + (int)stringSize.Height)));
        //}

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
        }

        private void 在线服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.Manager.Mode = AccessMode.ServerOnly;
        }

        private void 本地缓存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mapControl.Manager.Mode = AccessMode.CacheOnly;
        }

        #endregion

    }
}
