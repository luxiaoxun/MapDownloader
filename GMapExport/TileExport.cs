using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using log4net;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;
using GMap.NET.WindowsForms;
using GMapProvidersExt.Baidu;

namespace GMapExport
{
    public class TileExport
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TileExport));

        private BackgroundWorker worker = new BackgroundWorker();

        public event EventHandler<TileExportEventArgs> TileExportStart;
        public event EventHandler<TileExportEventArgs> TileExportComplete;
        public event EventHandler<TileExportEventArgs> TileExportProgress;

        private string connString;
        private ExportParameter exportParameter;
        private GMapProvider mapProvider;

        public TileExport()
        {
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        public bool IsBusy
        {
            get { return worker.IsBusy; }
        }

        public void Start(ExportParameter exportParameter, string mysqlConnString)
        {
            if (exportParameter != null)
            {
                this.exportParameter = exportParameter;
                this.mapProvider = exportParameter.MapProvider;
                this.connString = mysqlConnString;

                ExportLayerConfig arcGisLayerConfig = new ExportLayerConfig(exportParameter.ExportRect, exportParameter.MinZoom,
                    exportParameter.MaxZoom, exportParameter.MapProvider, exportParameter.ExportPath);
                bool isConfigFileSuccess = true;
                if (exportParameter.ExportType == ExportType.ArcGISTile)
                {
                    if (!arcGisLayerConfig.CreateArcGISMetaFile())
                    {
                        isConfigFileSuccess = false;
                        MessageBox.Show("创建ArcGIS图层描述文件失败！");
                    }
                }
                else if (exportParameter.ExportType == ExportType.TMSTile)
                {
                    if (!arcGisLayerConfig.CreateTmsMetaFile())
                    {
                        isConfigFileSuccess = false;
                        MessageBox.Show("创建TMS图层描述文件失败！");
                    }
                }

                if (isConfigFileSuccess)
                {
                    if (this.TileExportStart != null)
                    {
                        TileExportStart(this, new TileExportEventArgs());
                    }

                    worker.RunWorkerAsync();
                }
            }
        }

        public void Stop()
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }

        #region Background Worker

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //string conn = @"Database=mapcache-841;Data Source=127.0.0.1;pooling=false;Charset=utf8;port=3306;User Id=root;password=luxx";
                using (MySqlConnection cnGet = new MySqlConnection(this.connString))
                {
                    cnGet.Open();
                    int mapType = this.exportParameter.MapProvider.DbId;
                    for (int mapZoom = exportParameter.MinZoom; mapZoom <= exportParameter.MaxZoom; ++mapZoom)
                    {
                        int progress = Convert.ToInt32((mapZoom-exportParameter.MinZoom+1)* 100 /( exportParameter.MaxZoom-exportParameter.MinZoom+1));
                        worker.ReportProgress(progress, new TileExportEventArgs(mapZoom));
                        using (MySqlCommand cmdFetch = cnGet.CreateCommand())
                        {
                            cmdFetch.CommandText = String.Format("SELECT * FROM `gmapnetcache` where Type={0} and Zoom={1}", mapType, mapZoom);
                            cmdFetch.Prepare();
                            using (MySqlDataReader odata = cmdFetch.ExecuteReader())
                            {
                                while (odata.Read())
                                {
                                    try
                                    {
                                        int type = odata.GetInt32("Type");
                                        if (type == mapType)
                                        {
                                            int zoom = odata.GetInt32("Zoom");
                                            if (zoom == mapZoom)
                                            {
                                                long x = odata.GetInt64("X");
                                                long y = odata.GetInt64("Y");
                                                GPoint point = new GPoint(x, y);

                                                byte[] imageByte = new byte[odata.GetBytes(odata.GetOrdinal("Tile"), 0, null, 0, int.MaxValue)];
                                                odata.GetBytes(odata.GetOrdinal("Tile"), 0, imageByte, 0, imageByte.Length);

                                                GMapImage image = new GMapImage();
                                                MemoryStream memoryStream = new MemoryStream(imageByte);
                                                image.Img = System.Drawing.Image.FromStream(memoryStream);
                                                image.Data = memoryStream;
                                                WriteTileToDisk(image, zoom, point);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        public void RectifyBaiduOrTMSTile(PureImage img, int zoom, GPoint p)
        {
            long y = this.mapProvider.Projection.FromLatLngToTileXY(this.mapProvider.Projection.Bounds.Top, this.mapProvider.Projection.Bounds.Left, zoom).Y;
            long y0 = this.mapProvider.Projection.FromLatLngToTileXY(this.mapProvider.Projection.Bounds.Bottom, this.mapProvider.Projection.Bounds.Right, zoom).Y;

            if (this.exportParameter.ExportType == ExportType.TMSTile)
            {
                long p_y = (y + y0) - p.Y;
                p.Y = p_y;
            }
            else if (this.exportParameter.ExportType == ExportType.BaiduTile)
            {
                long p_x;
                long p_y;
                long x = this.mapProvider.Projection.FromLatLngToTileXY(this.mapProvider.Projection.Bounds.LocationTopLeft, zoom).X;
                long x0 = this.mapProvider.Projection.FromLatLngToTileXY(this.mapProvider.Projection.Bounds.LocationRightBottom, zoom).X;
                if (this.mapProvider is BaiduMapProviderBase)
                {
                    int num = Convert.ToInt32(Math.Pow(2.0, (double)(zoom - 1)));
                    p_x = p.X - num;
                    p_y = (num - p.Y) - 1;
                }
                else
                {
                    long delta_x = (x0 - y) + 1;
                    long delta_y = (y0 - y) + 1;
                    p_x = p.X - (delta_x / 2);
                    p_y = (p.Y + 1) - (delta_y / 2);
                }
                p.X = p_x;
                p.Y = p_y;
            }
        }

        private string BuildTileExportPath(PureImage img, int zoom, GPoint p)
        {
            string tileSuffix = "png";
            if (this.exportParameter.ExportType == ExportType.ArcGISTile)
            {
                DirectoryInfo dir = new DirectoryInfo(exportParameter.ExportPath + "/Layer/_alllayers");
                string zoomStr = string.Format("{0:D2}", zoom);
                long x = p.X;
                long y = p.Y;
                string col = string.Format("{0:x8}", x).ToLower();
                string row = string.Format("{0:x8}", y).ToLower();
                string pathDir = dir.FullName + "/" + "L" + zoomStr + "/" + "R" + row + "/";
                string path = pathDir + "C" + col + "."+tileSuffix;
                return path;
            }
            if ((this.exportParameter.ExportType == ExportType.TMSTile) || (this.exportParameter.ExportType == ExportType.BaiduTile))
            {
                RectifyBaiduOrTMSTile(img, zoom, p);
            }
            if (exportParameter.ExportType == ExportType.DefaultYXTile)
            {
                return string.Format("{0}/{1}/{2}/{3}/{4}.{5}", new object[] { this.exportParameter.ExportPath, "tiles", zoom, p.Y, p.X, tileSuffix });
            }

            return string.Format("{0}/{1}/{2}/{3}/{4}.{5}", new object[] { this.exportParameter.ExportPath, "tiles", zoom, p.X, p.Y, tileSuffix });
        }

        private void WriteTileToDisk(PureImage img, int zoom, GPoint p)
        {
            string path = BuildTileExportPath(img, zoom, p);
            string pathDir = path.Substring(0, path.LastIndexOf('/'));
            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter sw = new BinaryWriter(fs);
            //读出图片字节数组至byte[]
            byte[] imageByte = img.Data.ToArray();
            sw.Write(imageByte);
            sw.Close();
            fs.Close();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e != null)
            {
                if (this.TileExportProgress != null)
                {
                    TileExportProgress(this, e.UserState as TileExportEventArgs);
                }
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.TileExportComplete != null)
            {
                TileExportComplete(this, new TileExportEventArgs());
            }
        }

        #endregion
    }
}
