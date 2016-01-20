using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using GMap.NET;
using GMap.NET.MapProviders;
using log4net;

namespace GMapDownload
{
    public class TileDownloader
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TileDownloader));

        public event EventHandler<TileDownloadEventArgs> PrefetchTileStart;
        public event EventHandler<TileDownloadEventArgs> PrefetchTileComplete;
        public event EventHandler<TileDownloadEventArgs> PrefetchTileProgress;

        private int retry = 3;
        public int Retry
        {
            get { return retry; }
            set { retry = value; }
        }

        private string tilePath;
        public string TilePath
        {
            get { return tilePath; }
            set { tilePath = value; }
        }

        private GMapProvider provider;
        private int threadNum = 5;
        private bool isComplete = false;
        private Thread[] thread;

        private object locker = new object();
        private volatile int downloadSize = 0;
        private volatile int threadCompleteNum = 0;
        private int allDownloadTileSize;
        private ConcurrentQueue<DownloadLevelTile> downloadFailedTiles = new ConcurrentQueue<DownloadLevelTile>();

        public TileDownloader(int threadNum)
        {
            this.threadNum = threadNum;
            this.thread = new Thread[threadNum];
        }

        public void StartDownload(TileDownloaderArgs tileDownloaderArgs)
        {
            GMaps.Instance.UseMemoryCache = false;
            GMaps.Instance.CacheOnIdleRead = false;

            provider = tileDownloaderArgs.MapProvider;
            List<DownloadLevelTile> downloadTiles = tileDownloaderArgs.DownloadTiles;
            
            allDownloadTileSize = downloadTiles.Count;
            int threadNum = 5;
            int singelNum = (int)(allDownloadTileSize / threadNum);
            int remainder = (int)(allDownloadTileSize % threadNum);

            if (PrefetchTileStart != null)
            {
                PrefetchTileStart(this, new TileDownloadEventArgs(0));
            }

            if (singelNum == 0)
            {
                threadNum = 1;
            }
            for (int i = 0; i < threadNum; i++)
            {
                int startIndex = i*singelNum;
                int endIndex = startIndex + singelNum - 1;
                if (remainder != 0 && (threadNum - 1) == i)
                {
                    endIndex = downloadTiles.Count-1;
                }
                DownloadThreadArgs args = new DownloadThreadArgs(downloadTiles.GetRange(startIndex,endIndex-startIndex+1));
                thread[i] = new Thread(new ParameterizedThreadStart(Download));
                thread[i].Start(args);
            }
        }

        private void ReportProress()
        {
            if (PrefetchTileProgress != null)
            {
                PrefetchTileProgress(null, new TileDownloadEventArgs(allDownloadTileSize,downloadSize));
            }
        }

        private void ReportComplete()
        {
            if (PrefetchTileComplete != null)
            {
                PrefetchTileComplete(null, new TileDownloadEventArgs(100));
            }
        }

        private void Download(object obj)
        {
            try
            {
                DownloadThreadArgs args = obj as DownloadThreadArgs;
                List<DownloadLevelTile> threadDownloadLevelTiles = args.DownloadLevelTiles;
                int retryCount = 0;
                for(int i=0; i<threadDownloadLevelTiles.Count; ++i)
                {
                    GPoint p = threadDownloadLevelTiles[i].TilePoint;
                    int zoom = threadDownloadLevelTiles[i].TileZoom;
                    try
                    {
                        if (CacheTiles(zoom, p, provider))
                        {
                            retryCount = 0;
                            lock (locker)
                            {
                                ++downloadSize;
                            }
                        }
                        else
                        {
                            if (++retryCount <= retry)
                            {
                                --i;
                                continue;
                            }
                            else
                            {
                                retryCount = 0;
                                downloadFailedTiles.Enqueue(threadDownloadLevelTiles[i]);
                            }
                        }

                        ReportProress();
                    }
                    catch (Exception exception)
                    {
                        log.Error(exception);
                    }
                    
                }

                lock (locker)
                {
                    ++threadCompleteNum;
                }
                if (threadCompleteNum == threadNum)
                {
                    DownloadFailedTiles();
                    isComplete = true;
                    ReportComplete();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void DownloadFailedTiles()
        {
            while (!downloadFailedTiles.IsEmpty)
            {
                DownloadLevelTile tile = null;
                downloadFailedTiles.TryDequeue(out tile);
                if (tile != null)
                {
                    GPoint p = tile.TilePoint;
                    int zoom = tile.TileZoom;
                    try
                    {
                        if (CacheTiles(zoom, p, provider))
                        {
                            lock (locker)
                            {
                                ++downloadSize;
                            }
                            ReportProress();
                        }
                        else
                        {
                            downloadFailedTiles.Enqueue(tile);
                        }
                    }
                    catch (Exception exception)
                    {
                        log.Error(exception);
                    }
                }
            }
        }

        private bool CacheTiles(int zoom, GPoint p, GMapProvider provider)
        {
            foreach (var pr in provider.Overlays)
            {
                PureImage img;
                try
                {
                    img = pr.GetTileImage(p, zoom);
                    if (img != null)
                    {
                        // if the tile path is not null, write the tile to disk
                        if (tilePath != null)
                        {
                            WriteTileToDisk(img, zoom, p);
                        }
                        else
                        {
                            GMaps.Instance.PrimaryCache.PutImageToCache(img.Data.ToArray(), pr.DbId, p, zoom);
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
    }
}
