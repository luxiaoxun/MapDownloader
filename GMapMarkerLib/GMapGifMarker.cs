using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Drawing;
using GMap.NET.WindowsForms.ToolTips;

namespace GMapMarkerLib
{
    public class GMapGifMarker : GMapMarker
    {
        // Fields
        private GifImage gifImg;

        private System.Windows.Forms.Timer flashTimer = new System.Windows.Forms.Timer();

        // Methods
        public GMapGifMarker(PointLatLng p, GifImage image)
            : base(p)
        {
            this.gifImg = image;
            Size = image.GetFrame(0).Size;
            Offset = new Point(-Size.Width / 2, -Size.Height / 2);
            flashTimer.Tick += new EventHandler(flashTimer_Tick);
            flashTimer.Interval = 100;
            flashTimer.Start();
        }

        Image nextFrame;

        void flashTimer_Tick(object sender, EventArgs e)
        {
            nextFrame = this.gifImg.GetNextFrame();
            this.Overlay.Control.Refresh();
        }

        public override void OnRender(Graphics g)
        {
            if (nextFrame == null) return;
            //Image nextFrame = this.gifImg.GetNextFrame();
            g.DrawImage(nextFrame, new Rectangle(LocalPosition.X, LocalPosition.Y, nextFrame.Width, nextFrame.Height));
        }

        public override void Dispose()
        {
            if (flashTimer != null)
            {
                flashTimer.Stop();
                flashTimer.Dispose();
            }

            base.Dispose();
        }

    }
}
