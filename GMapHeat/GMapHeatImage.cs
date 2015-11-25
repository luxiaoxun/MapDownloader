using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GMap.NET.WindowsForms;

namespace GMapHeat
{
    public class GMapHeatImage : GMapMarker
    {
        private Image image;
        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                if (image != null)
                {
                    this.Size = new Size(image.Width, image.Height);
                }
            }
        }

        public GMapHeatImage(GMap.NET.PointLatLng p, Image image)
            : base(p)
        {
            DisableRegionCheck = true;
            IsHitTestVisible = false;
            this.Image = image;
        }

        public override void OnRender(Graphics g)
        {
            if (image == null)
                return;

            g.DrawImage(image, LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height);
        }
    }
}
