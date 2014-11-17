using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Drawing;
using System.Windows.Forms;

namespace GMapMarkerLib
{
    public class GMapFlashMarker : GMapMarker
    {
        private Image image;

        public bool IsHighlight = true;
        public Pen HighlightPen { set; get; }

        public Pen FlashPen { set; get; }
        private Timer flashTimer = new Timer();

        private int radius;
        private int flashRadius;
        private Random rand;

        public GMapFlashMarker(PointLatLng p, Image image)
            : base(p)
        {
            Size = new System.Drawing.Size(image.Width, image.Height);
            Offset = new System.Drawing.Point(-Size.Width / 2, -Size.Height / 2);
            this.image = image;
            HighlightPen = new System.Drawing.Pen(Brushes.Red,2);
            radius = Size.Width >= Size.Height ? Size.Width : Size.Height;
            flashTimer.Interval = 10;
            flashTimer.Tick += new EventHandler(flashTimer_Tick);
            rand = new Random();
        }

        public void StartFlash()
        {
            flashTimer.Start();
        }

        void flashTimer_Tick(object sender, EventArgs e)
        {
            if (FlashPen == null)
            {
                FlashPen = new Pen(Brushes.Red, 3);
                flashRadius = radius;
            }
            else
            {
                flashRadius += radius/4;
                if (flashRadius >= 2 * radius)
                {
                    flashRadius = radius;
                    FlashPen.Color = Color.FromArgb(255, Color.Red);
                }
                else
                {
                    int alpha = rand.Next(255);
                    FlashPen.Color = Color.FromArgb(alpha, Color.Red);
                }
            }

            this.Overlay.Control.Invalidate(
                new Rectangle(LocalPosition.X - flashRadius / 2 + Size.Width / 2, LocalPosition.Y - flashRadius / 2 + Size.Height / 2, flashRadius, flashRadius));
            this.Overlay.Control.Invalidate();

            //this.Overlay.Control.Refresh();
        }

        public void StopFlash()
        {
            flashTimer.Stop();
            if (FlashPen != null)
            {
                FlashPen.Dispose();
                FlashPen = null;
            }
            this.Overlay.Control.Refresh();
        }

        public override void OnRender(Graphics g)
        {
            if (image == null) return;

            Rectangle rect = new Rectangle(LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height);
            g.DrawImage(image, rect);

            if (IsMouseOver && IsHighlight)
            {
                g.DrawRectangle(HighlightPen,rect);
            }

            if (FlashPen != null)
            {
                g.DrawEllipse(FlashPen,
                    new Rectangle(LocalPosition.X - flashRadius / 2 + Size.Width / 2, LocalPosition.Y - flashRadius / 2 + Size.Height / 2, flashRadius, flashRadius));
            }
        }

        public override void Dispose()
        {
            if (HighlightPen != null)
            {
                HighlightPen.Dispose();
                HighlightPen = null;
            }

            if (FlashPen != null)
            {
                FlashPen.Dispose();
                FlashPen = null;
            }

            base.Dispose();
        }
    }
}
