using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;

namespace GMapMarkerLib
{
    [Serializable]
    public class GMapMarkerEllipse : GMapMarker, ISerializable
    {
        // Fields
        [NonSerialized]
        public Brush Fill;
        [NonSerialized]
        public GMapMarker InnerMarker;
        [NonSerialized]
        public Pen Pen;

        // Methods
        public GMapMarkerEllipse(PointLatLng p)
            : base(p)
        {
            this.Fill = new SolidBrush(Color.FromArgb(0xff, Color.Blue));
            this.Pen = new Pen(Brushes.Blue, 2f);
            base.Size = new Size(10, 10);
            base.Offset = new Point((0 - base.Size.Width) / 2, (0 - base.Size.Height) / 2);
        }

        protected GMapMarkerEllipse(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Fill = new SolidBrush(Color.FromArgb(0xff, Color.Blue));
        }

        public override void Dispose()
        {
            if (this.Pen != null)
            {
                this.Pen.Dispose();
                this.Pen = null;
            }
            if (this.InnerMarker != null)
            {
                this.InnerMarker.Dispose();
                this.InnerMarker = null;
            }
            base.Dispose();
        }

        public override void OnRender(Graphics g)
        {
            Rectangle rect = new Rectangle(base.LocalPosition.X, base.LocalPosition.Y, base.Size.Width, base.Size.Height);
            g.DrawEllipse(this.Pen, rect);
            g.FillEllipse(this.Fill, rect);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
}
