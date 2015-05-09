using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;

namespace MapDownloader
{
    public class PrefetchTiles
    {
        BackgroundWorker worker = new BackgroundWorker();

        public event EventHandler<PrefetchTileEventArgs> PrefetchTileStart;
        public event EventHandler<PrefetchTileEventArgs> PrefetchTileComplete;
        public event EventHandler<PrefetchTileEventArgs> PrefetchTileProgress;

        private int retry=3;
        public int Retry 
        {
            get { return retry; }
            set { retry = value; }
        }

        private ulong TotalTiles = 0;
        private ulong OverallCompleted = 0;
        private int OverallProgress = 0;

        private string tilePath;

        public PrefetchTiles()
        {
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }

        public void Start(RectLatLng area, int minZoom, int maxZoom, GMapProvider[] providers, int sleep)
        {
            if (!worker.IsBusy)
            {
                WorkerArgs args = new WorkerArgs();
                args.MinZoom = minZoom;
                args.MaxZoom = maxZoom;
                args.Area = area;
                args.Providers = providers;
                args.TileSleeper = sleep;

                GMaps.Instance.UseMemoryCache = false;
                GMaps.Instance.CacheOnIdleRead = false;

                worker.RunWorkerAsync(args);
                if (PrefetchTileStart != null)
                {
                    PrefetchTileStart(this, new PrefetchTileEventArgs(0));
                }
            }
        }

        public void Start(RectLatLng area, int minZoom, int maxZoom, GMapProvider[] providers, int sleep, string path)
        {
            this.tilePath = path;

            if (!worker.IsBusy)
            {
                WorkerArgs args = new WorkerArgs();
                args.MinZoom = minZoom;
                args.MaxZoom = maxZoom;
                args.Area = area;
                args.Providers = providers;
                args.TileSleeper = sleep;

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
        }

        #region Background Worker
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            WorkerArgs args = (WorkerArgs)e.Argument;
            List<GPoint> list = new List<GPoint>();

            for (int z = args.MinZoom; z <= args.MaxZoom; z++)
            {
                foreach (GMapProvider provider in args.Providers)
                {
                    TotalTiles += (ulong)provider.Projection.GetAreaTileList(args.Area, z, 0).Count;
                }
            }

            for (int z = args.MinZoom; z <= args.MaxZoom; z++)
            {
                if (worker.CancellationPending)
                    break;

                foreach (GMapProvider provider in args.Providers)
                {
                    if (worker.CancellationPending)
                        break;

                    if (list != null)
                    {
                        list.Clear();
                        list = null;
                    }

                    // Get the tiles we need to download
                    list = provider.Projection.GetAreaTileList(args.Area, z, 0);
                    Shuffle<GPoint>(list);

                    int numfiles = list.Count;

                    // Download all tiles in the list
                    int retryCount = 0;
                    for (int i = 0; i < numfiles; i++)
                    {
                        if (worker.CancellationPending)
                            break;

                        // Download the tile
                        // Retry if there is a failure
                        if (CacheTiles(z, list[i], provider))
                            retryCount = 0;
                        else
                        {
                            if (++retryCount <= retry)
                            {
                                i--;
                                System.Threading.Thread.Sleep(1111);
                                continue;
                            }
                            else
                                retryCount = 0;
                        }
                        // Report progress
                        OverallCompleted++;
                        OverallProgress = Convert.ToInt32(OverallCompleted * 100 / TotalTiles);
                        worker.ReportProgress(OverallProgress);
                        System.Threading.Thread.Sleep(args.TileSleeper);
                    }
                }
            }

            if (worker.CancellationPending)
                e.Cancel = true;
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e != null)
            {
                if (PrefetchTileProgress != null)
                {
                    PrefetchTileProgress(this, new PrefetchTileEventArgs(e.ProgressPercentage));
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
                Exception ex;
                PureImage img;

                img = GMaps.Instance.GetImageFrom(pr, p, zoom, out ex);

                if (img != null)
                {
                    // if the tile path is not null, write the tile to disk
                    if (tilePath != null)
                    {
                        WriteTileToDisk(img,zoom,p);
                    }
                    img.Dispose();
                    img = null;
                }
                else
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

        public PrefetchTileEventArgs(int value)
        {
            ProgressValue = value;
        }
    }

    #region Internal Helper Classes

    internal class WorkerArgs
    {
        public GMapProvider[] Providers;
        public int TileSleeper;
        public RectLatLng Area;
        public int MinZoom;
        public int MaxZoom;
    }
    #endregion

}
