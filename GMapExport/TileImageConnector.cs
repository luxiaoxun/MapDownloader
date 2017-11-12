using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System.Windows.Forms;
using System.Drawing;
using log4net;

namespace GMapExport
{
    public class TileImageConnector
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TileImageConnector));
        private BackgroundWorker imageGenWorker = new BackgroundWorker();
        private GMapProvider provider;
        
        private int retry = 3;
        public int Retry
        {
            get { return retry; }
            set { retry = value; }
        }
        
        public event EventHandler ImageTileComplete;

        public TileImageConnector()
        {
            imageGenWorker.DoWork += new DoWorkEventHandler(imageGenWorker_DoWork);
            imageGenWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(imageGenWorker_RunWorkerCompleted);
        }

        public void Start(GMapProvider provider, RectLatLng area, int zoom)
        {
            this.provider = provider;
            if(!imageGenWorker.IsBusy)
            {
                ImageGenArgs args = new ImageGenArgs();
                args.Area = area;
                args.Zoom = zoom;
                imageGenWorker.RunWorkerAsync(args);
            }
        }

        void imageGenWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ImageTileComplete != null)
            {
                ImageTileComplete(this, new EventArgs());
            }
        }

        private bool CacheTiles(int zoom, GPoint p, GMapProvider provider, Graphics gfx, GPoint topLeftPx)
        {
            foreach (var pr in provider.Overlays)
            {
                Exception ex;
                GMapImage tile = GMaps.Instance.GetImageFrom(pr, p, zoom, out ex) as GMapImage;
                if (tile != null)
                {
                    using (tile)
                    {
                        long x = p.X * this.provider.Projection.TileSize.Width - topLeftPx.X;
                        long y = p.Y * this.provider.Projection.TileSize.Width - topLeftPx.Y;
                        gfx.DrawImage(tile.Img, x, y, this.provider.Projection.TileSize.Width, this.provider.Projection.TileSize.Height);
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        void imageGenWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ImageGenArgs args = (ImageGenArgs)e.Argument;
                RectLatLng area = args.Area;
                int zoom = args.Zoom;
                List<GPoint> tileArea = this.provider.Projection.GetAreaTileList(area, zoom, 0);
                string bigImage = zoom + "-" + Guid.NewGuid().ToString() + ".jpg";

                // current area
                GPoint topLeftPx = this.provider.Projection.FromLatLngToPixel(area.LocationTopLeft, zoom);
                GPoint rightButtomPx = this.provider.Projection.FromLatLngToPixel(area.Bottom, area.Right, zoom);
                GPoint pxDelta = new GPoint(rightButtomPx.X - topLeftPx.X, rightButtomPx.Y - topLeftPx.Y);

                using (Bitmap bmpDestination = new Bitmap((int)(pxDelta.X), (int)(pxDelta.Y)))
                {
                    using (Graphics gfx = Graphics.FromImage(bmpDestination))
                    {
                        gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                        // get tiles & combine into one
                        int all = tileArea.Count;
                        int retryCount = 0;
                        for (int i = 0; i < all; ++i)
                        {
                            GPoint pos = tileArea[i];
                            if (CacheTiles(zoom, pos, provider,gfx,topLeftPx))
                                retryCount = 0;
                            else
                            {
                                if (++retryCount <= retry)
                                {
                                    i--;
                                    continue;
                                }
                                else
                                    retryCount = 0;
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
                log.Warn(ex.Message);
                MessageBox.Show("拼接图生成错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    #region Internal Helper Classes

    internal class ImageGenArgs
    {
        public RectLatLng Area;
        public int Zoom;
    }

    #endregion
}
