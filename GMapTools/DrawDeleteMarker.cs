using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapTools
{
    public class DrawDeleteMarker : GMapMarker
    {
        private Image image;

        public DrawDeleteMarker(PointLatLng p) : base(p)
        {
            image = GMapTools.Properties.Resource.delete;
            Size = new System.Drawing.Size(image.Width, image.Height);
            Offset = new System.Drawing.Point(Size.Width, -Size.Height/2);

            this.ToolTip = new DrawDeleteMarkerToolTip(this);
            this.ToolTipText = "清除本次测距";
            this.ToolTipMode = MarkerTooltipMode.OnMouseOver;
        }

        public override void OnRender(Graphics g)
        {
            Rectangle imageRect = new Rectangle(LocalPosition.X, LocalPosition.Y, Size.Width, Size.Height);
            g.DrawImage(image, imageRect);
        }
    }
}
