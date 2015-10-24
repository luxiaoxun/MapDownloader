using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using log4net;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;

namespace GMapDownloader
{
    public class PrefetchTiles
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PrefetchTiles));

        private BackgroundWorker worker = new BackgroundWorker();

        public event EventHandler<PrefetchTileEventArgs> PrefetchTileStart;
        public event EventHandler<PrefetchTileEventArgs> PrefetchTileComplete;
        public event EventHandler<PrefetchTileEventArgs> PrefetchTileProgress;

        private int retry = 3;
        public int Retry
        {
            get { return retry; }
            set { retry = value; }
        }
        private Dictionary<int, List<GPoint>> zoomGPointDic = new Dictionary<int, List<GPoint>>();

        private ulong currentZoomTiles = 0;
        private ulong currentZoomCompleted = 0;
        private int overallProgress = 0;
        private int currentZoom;

        private string tilePath;

        public PrefetchTiles()
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

        GMapProvider provider;

        public void Start(RectLatLng area, int minZoom, int maxZoom, GMapProvider provider)
        {
            if (!worker.IsBusy)
            {
                WorkerArgs args = new WorkerArgs();
                args.MinZoom = minZoom;
                args.MaxZoom = maxZoom;
                args.Area = area;
                args.Provider = provider;

                GMaps.Instance.UseMemoryCache = false;
                GMaps.Instance.CacheOnIdleRead = false;

                worker.RunWorkerAsync(args);
                if (PrefetchTileStart != null)
                {
                    PrefetchTileStart(this, new PrefetchTileEventArgs(0));
                }
            }
        }

        public void Start(RectLatLng area, int minZoom, int maxZoom, GMapProvider provider, string path)
        {
            this.tilePath = path;

            if (!worker.IsBusy)
            {
                WorkerArgs args = new WorkerArgs();
                args.MinZoom = minZoom;
                args.MaxZoom = maxZoom;
                args.Area = area;
                args.Provider = provider;

                GMaps.Instance.UseMemoryCache = false;
                GMaps.Instance.CacheOnIdleRead = false;

                worker.RunWorkerAsync(args);
                if (PrefetchTileStart != null)
                {
                    PrefetchTileStart(this, new PrefetchTileEventArgs(0));
                }
            }
        }

        public void Stop()
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
            }

            currentZoomTiles = 0;
            currentZoomCompleted = 0;
            overallProgress = 0;
        }

        #region Background Worker

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = (BackgroundWorker)sender;
                WorkerArgs args = (WorkerArgs)e.Argument;
                //List<GPoint> list = new List<GPoint>();

                GMapProvider provider = args.Provider;
                for (int z = args.MinZoom; z <= args.MaxZoom; z++)
                {
                    if (worker.CancellationPending)
                        break;

                    //if (list != null)
                    //{
                    //    list.Clear();
                    //    list = null;
                    //}

                    currentZoom = z;
                    currentZoomCompleted = 0;
                    currentZoomTiles = 0;
                    RectLatLng rect = args.Area;
                    int retryCount = 0;

                    GPoint topLeft = provider.Projection.FromPixelToTileXY(provider.Projection.FromLatLngToPixel(rect.LocationTopLeft, z));
                    GPoint rightBottom = provider.Projection.FromPixelToTileXY(provider.Projection.FromLatLngToPixel(rect.LocationRightBottom, z));

                    long begin_x = topLeft.X;
                    long end_x = rightBottom.X;
                    long begin_y = topLeft.Y;
                    long end_y = rightBottom.Y;

                    currentZoomTiles = (ulong)((end_x - begin_x + 1) * (end_y - begin_y + 1));

                    for (long x = begin_x; x <= end_x; ++x)
                    {
                        for (long y = begin_y; y <= end_y; ++y)
                        {
                            if (worker.CancellationPending)
                                break;
                            if (x > 0 && y > 0)
                            {
                                GPoint p = new GPoint(x, y);
                                // Download the tile
                                // Retry if there is a failure
                                if (CacheTiles(z, p, provider))
                                    retryCount = 0;
                                else
                                {
                                    if (++retryCount <= retry)
                                    {
                                        y--;
                                        continue;
                                    }
                                    else
                                        retryCount = 0;
                                }
                            }
                            // Report progress
                            currentZoomCompleted++;
                            overallProgress = Convert.ToInt32(currentZoomCompleted * 100 / currentZoomTiles);
                            PrefetchTileEventArgs progressArgs = new PrefetchTileEventArgs(currentZoomTiles, currentZoomCompleted, currentZoom);
                            worker.ReportProgress(overallProgress, progressArgs);
                        }
                    }
                }

                if (worker.CancellationPending)
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e != null)
            {
                if (PrefetchTileProgress != null)
                {
                    //PrefetchTileProgress(this, new PrefetchTileEventArgs(totalTiles, overallCompleted,currentZoom));
                    PrefetchTileProgress(this, e.UserState as PrefetchTileEventArgs);
                }
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GMaps.Instance.UseMemoryCache = true;
            GMaps.Instance.CacheOnIdleRead = true;

            if (PrefetchTileComplete != null)
            {
                PrefetchTileComplete(this, new PrefetchTileEventArgs(100));
            }
        }

        #endregion

        #region Standalone Methods

        private void WriteTileToDisk(PureImage img, int zoom, GPoint p)
        {
            DirectoryInfo di = new DirectoryInfo(tilePath);
            string zoomStr = string.Format("{0:D2}", zoom);

            long x = p.X;
            long y = p.Y;
            string col = string.Format("{0:x8}", x).ToLower();
            string row = string.Format("{0:x8}", y).ToLower();

            string dir = di.FullName + "//" + "L" + zoomStr + "//" + "R" + row + "//";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            FileStream fs = new FileStream(dir + "C" + col + ".png", FileMode.Create, FileAccess.Write);
            BinaryWriter sw = new BinaryWriter(fs);
            //读出图片字节数组至byte[]
            byte[] imageByte = img.Data.ToArray();
            sw.Write(imageByte);
            sw.Close();
            fs.Close();
        }

        private bool CacheTiles(int zoom, GPoint p, GMapProvider provider)
        {
            foreach (var pr in provider.Overlays)
            {
                //Exception ex;
                PureImage img;
                try
                {
                    //img = GMaps.Instance.GetImageFrom(pr, p, zoom, out ex);
                    img = pr.GetTileImage(p, zoom);
                    if (img != null)
                    {
                        GMaps.Instance.PrimaryCache.PutImageToCache(img.Data.ToArray(), pr.DbId, p, zoom);
                        // if the tile path is not null, write the tile to disk
                        if (tilePath != null)
                        {
                            WriteTileToDisk(img, zoom, p);
                        }
                        img.Dispose();
                        img = null;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        void Shuffle<T>(List<T> deck)
        {
            int N = deck.Count;
            Random random = new System.Random(0);

            for (int i = 0; i < N; ++i)
            {
                int r = i + (int)(random.Next(N - i));
                T t = deck[r];
                deck[r] = deck[i];
                deck[i] = t;
            }
        }
        #endregion
    }

    public class PrefetchTileEventArgs : EventArgs
    {
        public int ProgressValue { set; get; }

        public ulong TileAllNum { set; get; }

        public ulong TileCompleteNum { set; get; }

        public int CurrentDownloadZoom { set; get; }

        public PrefetchTileEventArgs(int value)
        {
            ProgressValue = value;
        }

        public PrefetchTileEventArgs(ulong allNum, ulong comNum, int zoom)
        {
            TileAllNum = allNum;
            TileCompleteNum = comNum;
            CurrentDownloadZoom = zoom;
            ProgressValue = Convert.ToInt32(TileCompleteNum * 100 / TileAllNum); ;
        }
    }

    #region Internal Helper Classes

    internal class WorkerArgs
    {
        public GMapProvider Provider;
        public RectLatLng Area;
        public int MinZoom;
        public int MaxZoom;
    }
    #endregion

}
