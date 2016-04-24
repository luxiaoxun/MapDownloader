using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using log4net;

namespace GMapDownload
{
    public partial class DownloadCfgForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DownloadCfgForm));

        private RectLatLng rectLatLng;

        private GMapProvider mapProvider;

        private List<DownloadLevelCfg> downloadLevelCfgs = new List<DownloadLevelCfg>();
        
        private TileDownloaderArgs tileDownloaderArgs = new TileDownloaderArgs();

        public DownloadCfgForm(RectLatLng rectLatLng, GMapProvider mapProvider)
        {
            InitializeComponent();

            this.rectLatLng = rectLatLng;
            this.mapProvider = mapProvider;
            tileDownloaderArgs.MapProvider = mapProvider;
            tileDownloaderArgs.DownloadArea = rectLatLng;

            InitUI();

            BackgroundWorker backgroundWorker =new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            //backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerAsync();
        }

        private void InitUI()
        {
            this.textBoxRightBottomLat.Text = this.rectLatLng.LocationRightBottom.Lat.ToString();
            this.textBoxRightBottomLng.Text = this.rectLatLng.LocationRightBottom.Lng.ToString();
            this.textBoxTopLeftLat.Text = this.rectLatLng.LocationTopLeft.Lat.ToString();
            this.textBoxTopLeftLng.Text = this.rectLatLng.LocationTopLeft.Lng.ToString();

            this.mapProvider.MinZoom = 1;
            this.mapProvider.MaxZoom = 18;

            this.buttonOK.Enabled = false;
        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e != null)
            {
                //this.progressBar1.Value = e.ProgressPercentage;
            }
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BindingList<DownloadLevelCfg> bindingList =
                    new System.ComponentModel.BindingList<DownloadLevelCfg>(downloadLevelCfgs);
            this.dataGridViewLevel.DataSource = bindingList;

            this.buttonOK.Enabled = true;
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                int minzoom = this.mapProvider.MinZoom;
                int maxzoom = this.mapProvider.MaxZoom.Value;

                for (int z = minzoom; z <= maxzoom; ++z)
                {
                    GPoint topLeft = mapProvider.Projection.FromPixelToTileXY(mapProvider.Projection.FromLatLngToPixel(rectLatLng.LocationTopLeft, z));
                    GPoint rightBottom = mapProvider.Projection.FromPixelToTileXY(mapProvider.Projection.FromLatLngToPixel(rectLatLng.LocationRightBottom, z));

                    long begin_x = topLeft.X;
                    long end_x = rightBottom.X;
                    long begin_y = topLeft.Y;
                    long end_y = rightBottom.Y;
                    long levelRow = (end_x - begin_x + 1);
                    long levelCol = (end_y - begin_y + 1);
                    long currentZoomTiles = levelRow * levelCol;

                    DownloadLevelCfg downloadLevelCfg = new DownloadLevelCfg();
                    downloadLevelCfg.TopLeft = topLeft;
                    downloadLevelCfg.RightBottom = rightBottom;
                    downloadLevelCfg.IsChecked = false;
                    downloadLevelCfg.ZoomLevel = z;
                    downloadLevelCfg.ZoomLevelCol = levelCol;
                    downloadLevelCfg.ZoomLevelRow = levelRow;
                    downloadLevelCfg.PreCount = currentZoomTiles;
                    long sizeMb = currentZoomTiles * 20 / 1024;
                    if (sizeMb > 0)
                    {
                        downloadLevelCfg.PreSize = string.Format("{0}MB", sizeMb);
                    }
                    else
                    {
                        downloadLevelCfg.PreSize = string.Format("{0}KB", currentZoomTiles * 20);
                    }

                    downloadLevelCfgs.Add(downloadLevelCfg);

                    //int progress = (z - minzoom + 1) * 100 / (maxzoom - minzoom + 1);
                    //worker.ReportProgress(progress);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            
        }

        #region 全选 反选 清除

        //全选
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < downloadLevelCfgs.Count; ++i)
            {
                downloadLevelCfgs[i].IsChecked = true;
            }
            BindingList<DownloadLevelCfg> bindingList =
                    new System.ComponentModel.BindingList<DownloadLevelCfg>(downloadLevelCfgs);
            this.dataGridViewLevel.DataSource = bindingList;
        }

        //反选
        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < downloadLevelCfgs.Count; ++i)
            {
                downloadLevelCfgs[i].IsChecked = !downloadLevelCfgs[i].IsChecked;
            }
            BindingList<DownloadLevelCfg> bindingList =
                    new System.ComponentModel.BindingList<DownloadLevelCfg>(downloadLevelCfgs);
            this.dataGridViewLevel.DataSource = bindingList;
        }

        //清除
        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < downloadLevelCfgs.Count; ++i)
            {
                downloadLevelCfgs[i].IsChecked = false;
            }
            BindingList<DownloadLevelCfg> bindingList =
                    new System.ComponentModel.BindingList<DownloadLevelCfg>(downloadLevelCfgs);
            this.dataGridViewLevel.DataSource = bindingList;
        }

        #endregion


        private List<DownloadLevelTile> GetDownloadLevelTiles(DownloadLevelCfg downloadLevelCfg)
        {
            int count = downloadLevelCfg.TileGPoints.Count;
            List<DownloadLevelTile> downloadLevelTiles = new List<DownloadLevelTile>(count);
            for (int i = 0; i < count; ++i)
            {
                DownloadLevelTile downloadLevel = new DownloadLevelTile(downloadLevelCfg.TileGPoints[i], downloadLevelCfg.ZoomLevel);
                downloadLevelTiles.Add(downloadLevel);
            }

            return downloadLevelTiles;
        }

        //确定
        private void button1_Click(object sender, EventArgs e)
        {
            tileDownloaderArgs.DownloadTiles = new List<DownloadLevelTile>();
            for (int i = 0; i < downloadLevelCfgs.Count; ++i)
            {
                if (downloadLevelCfgs[i].IsChecked)
                {
                    GPoint topLeft = downloadLevelCfgs[i].TopLeft;
                    GPoint rightBottom = downloadLevelCfgs[i].RightBottom;

                    long begin_x = topLeft.X;
                    long end_x = rightBottom.X;
                    long begin_y = topLeft.Y;
                    long end_y = rightBottom.Y;

                    List<GPoint> tileGPoints = new List<GPoint>();
                    try
                    {
                        for (long x = begin_x; x <= end_x; ++x)
                        {
                            for (long y = begin_y; y <= end_y; ++y)
                            {
                                if (x >= 0 && y >= 0)
                                {
                                    GPoint p = new GPoint(x, y);
                                    tileGPoints.Add(p);
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        //downloadLevelCfg.Tip = "无法下载";
                        string msg = string.Format("第{0}级数据量太大，会内存溢出，请分片下载！",downloadLevelCfgs[i].ZoomLevel);
                        MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        log.Error(exception);
                    }
                    downloadLevelCfgs[i].TileGPoints = tileGPoints;
                    List<DownloadLevelTile> downloadLevelTile = GetDownloadLevelTiles(downloadLevelCfgs[i]);
                    tileDownloaderArgs.DownloadTiles.AddRange(downloadLevelTile);
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        //取消
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public TileDownloaderArgs GetDownloadTileGPoints()
        {
            return this.tileDownloaderArgs;
        }


    }
}
