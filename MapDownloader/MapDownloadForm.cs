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
using NetUtilityLib;

namespace MapDownloader
{
    public partial class MapDownloadForm : Form
    {
        //private string conString = @"Server=127.0.0.1;Port=3306;Database=mapcache;Uid=root;Pwd=admin;";

        private static string conStringFormat = "Server={0};Port={1};Database={2};Uid={3};Pwd={4};";
        private static string conString;

        public MapDownloadForm()
        {
            InitializeComponent();

            InitMap();

            InitMySQLConString();
        }

        private void InitMap()
        {
            mapControl.ShowCenter = false;
            mapControl.DragButton = System.Windows.Forms.MouseButtons.Left;
            mapControl.CacheLocation = Environment.CurrentDirectory + "\\MapCache\\"; //缓存位置

            mapControl.MapProvider = GMapProviders.GoogleChinaMap; ;
            mapControl.Position = new PointLatLng(32.043, 118.773);
            mapControl.MinZoom = 1;
            mapControl.MaxZoom = 18;
            mapControl.Zoom = 9;

            #region Add maps

            comboBoxMapType.ValueMember = "CnName";
            //comboBoxMapType.DataSource = GMapProviders.List;
            
            //foreach (var mapProvider in GMapProviders.List)
            //{
            //    comboBoxMapType.Items.Add(mapProvider);
            //}
            comboBoxMapType.Items.Add(GMapProviders.GoogleChinaMap);
            comboBoxMapType.Items.Add(GMapProviders.GoogleChinaSatelliteMap);
            comboBoxMapType.Items.Add(GMapProviders.GoogleChinaHybridMap);

            comboBoxMapType.Items.Add(GMapProvidersExt.AMap.AMapProvider.Instance);
            comboBoxMapType.Items.Add(GMapProvidersExt.AMap.AMapSateliteProvider.Instance);
            comboBoxMapType.Items.Add(GMapProvidersExt.AMap.AMapHybirdProvider.Instance);
            comboBoxMapType.Items.Add(GMapProvidersExt.SoSo.SosoMapProvider.Instance);
            comboBoxMapType.Items.Add(GMapProvidersExt.SoSo.SosoMapSateliteProvider.Instance);
            comboBoxMapType.Items.Add(GMapProvidersExt.SoSo.SosoMapHybridProvider.Instance);
            comboBoxMapType.Items.Add(GMapProvidersExt.Baidu.BaiduMapProvider.Instance);
            comboBoxMapType.Items.Add(GMapProvidersExt.Baidu.BaiduSatelliteMapProvider.Instance);
            comboBoxMapType.Items.Add(GMapProvidersExt.Baidu.BaiduHybridMapProvider.Instance);


            comboBoxMapType.SelectedItem = mapControl.MapProvider;

            #endregion

            this.comboBoxMapType.SelectedValueChanged += new EventHandler(comboBoxMapType_SelectedValueChanged);
            this.radioButtonMySQL.CheckedChanged += new EventHandler(radioButtonMySQL_CheckedChanged);
            this.radioButtonSQLite.CheckedChanged += new EventHandler(radioButtonSQLite_CheckedChanged);
            this.buttonDownload.Click += new EventHandler(buttonDownload_Click);
            this.buttonMapImage.Click += new EventHandler(buttonMapImage_Click);
        }

        private void InitMySQLConString()
        {
            string ip = ConfigHelper.GetAppConfig("MySQLServerIP");
            string port = ConfigHelper.GetAppConfig("MySQLServerPort");
            string dbName = ConfigHelper.GetAppConfig("Database");
            string userID = ConfigHelper.GetAppConfig("UserID");
            string password = ConfigHelper.GetAppConfig("Password");

            conString = string.Format(conStringFormat, ip, port, dbName, userID, password);
        }

        void buttonMapImage_Click(object sender, EventArgs e)
        {
            RectLatLng area = mapControl.SelectedArea;
            if (!area.IsEmpty)
            {
                try
                {
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
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select map area with Right button holding ALT", "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

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

        void comboBoxMapType_SelectedValueChanged(object sender, EventArgs e)
        {
            this.mapControl.MapProvider = this.comboBoxMapType.SelectedItem as GMapProvider;
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();

            RectLatLng area = mapControl.SelectedArea;
            if (!area.IsEmpty)
            {
                try
                {
                    int minZ = int.Parse(this.textBoxMinZoom.Text);
                    int maxZ = int.Parse(this.textBoxMaxZoom.Text);
                    minZ = minZ <= 0 ? 1 : minZ;
                    maxZ = maxZ >= mapControl.MaxZoom ? mapControl.MaxZoom : maxZ;

                    for (int i = minZ; i <= maxZ; i++)
                    {
                        BackgroundWorker worker = new BackgroundWorker();
                        worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                        worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                        MapAreaInfo mapAreaInfo = new MapAreaInfo(i,area);
                        worker.RunWorkerAsync(mapAreaInfo);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select map area with Right button holding ALT", "Tip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int zoom = (int)e.Result;
            string text = "Zoom为" + zoom + "的地图下载完成";
            this.listBox1.Items.Add(text);
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
    }
}
