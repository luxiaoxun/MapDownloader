using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GMap.NET.WindowsForms;

namespace GMapWinFormDemo
{
    public partial class MapControl : GMapControl
    {
        //#region 比例尺变量

        ///// <summary>
        ///// The font for the m/km markers
        ///// </summary>
        //private Font fontCustomScale = new Font("Arial", 6);

        ///// <summary>
        ///// The font for the scale header 
        ///// </summary>
        //private Font fontCustomScaleBold = new Font("Arial", 10, FontStyle.Bold);

        ///// <summary>
        ///// The brush for the scale's background
        ///// </summary>
        //private Brush brushCustomScaleBackColor = new SolidBrush(Color.FromArgb(180, 185, 215, 255));

        ///// <summary>
        ///// The Textcolor for the scale's fonts
        ///// </summary>
        //private Color colorCustomScaleText = Color.FromArgb(20, 65, 140);

        ///// <summary>
        ///// The width of the scale-rectangle
        ///// </summary>
        //private int intScaleRectWidth = 300;

        ///// <summary>
        ///// The height of the scale-rectangle
        ///// </summary>
        //private int intScaleRectHeight = 50;

        ///// <summary>
        ///// The height of the scale bar
        ///// </summary>
        //private int intScaleBarHeight = 10;

        ///// <summary>
        ///// The padding of the scale
        ///// </summary>
        //private int intScaleLeftPadding = 10;

        //#endregion

        public MapControl()
        {
            InitializeComponent();
        }

        //protected override void OnPaintOverlays(System.Drawing.Graphics g)
        //{
        //    base.OnPaintOverlays(g);

        //    if (ShowScale)
        //    {
        //        double resolution = this.MapProvider.Projection.GetGroundResolution((int)this.Zoom, Position.Lat);

        //        int px10 = (int)(10.0 / resolution);            // 10 meters
        //        int px100 = (int)(100.0 / resolution);          // 100 meters
        //        int px1000 = (int)(1000.0 / resolution);        // 1km   
        //        int px10000 = (int)(10000.0 / resolution);      // 10km  
        //        int px100000 = (int)(100000.0 / resolution);    // 100km  
        //        int px1000000 = (int)(1000000.0 / resolution);  // 1000km
        //        int px5000000 = (int)(5000000.0 / resolution);  // 5000km

        //        //Check how much width we have and set the scale accordingly
        //        int availableWidth = (intScaleRectWidth - 2 * intScaleLeftPadding);

        //        //5000 kilometers:
        //        if (availableWidth >= px5000000)
        //            DrawScale(g, px5000000, availableWidth, 5000, "km");
        //        //1000 kilometers:
        //        else if (availableWidth >= px1000000)
        //            DrawScale(g, px1000000, availableWidth, 1000, "km");
        //        //100 kilometers:
        //        else if (availableWidth >= px100000)
        //            DrawScale(g, px100000, availableWidth, 100, "km");
        //        //10 kilometers:
        //        else if (availableWidth >= px10000)
        //            DrawScale(g, px10000, availableWidth, 10, "km");
        //        //1 kilometers:
        //        else if (availableWidth >= px1000)
        //            DrawScale(g, px1000, availableWidth, 1, "km");
        //        //100 meters:
        //        else if (availableWidth >= px100)
        //            DrawScale(g, px100, availableWidth, 100, "m");
        //        //10 meters:
        //        else if (availableWidth >= px10)
        //            DrawScale(g, px10, availableWidth, 10, "m");
        //    }
        //}

        //private void DrawScale(System.Drawing.Graphics g, int resLength, int availableWidth, int totalDimenson, String unit)
        //{
        //    Point p = new System.Drawing.Point(this.Width - (intScaleRectWidth + 10), this.Height - (intScaleRectHeight + 10));
        //    Rectangle rect = new Rectangle(p, new Size(intScaleRectWidth, intScaleRectHeight));
        //    g.FillRectangle(brushCustomScaleBackColor, rect);
        //    Pen pen = new Pen(colorCustomScaleText, 1);
        //    g.DrawRectangle(pen, rect);
        //    SizeF stringSize = new SizeF();
        //    Point pos = new Point();

        //    //Header:
        //    String scaleString = @"比例尺";
        //    stringSize = g.MeasureString(scaleString, fontCustomScaleBold);
        //    pos = new Point(p.X + (rect.Width - (int)stringSize.Width) / 2, p.Y + 3);
        //    g.DrawString(scaleString, fontCustomScaleBold, pen.Brush, pos);

        //    pos = new Point(p.X + intScaleLeftPadding, pos.Y + 30);

        //    //How many rectangles fit?
        //    int numRects = availableWidth / resLength;
        //    Size rectSize = new Size(resLength, intScaleBarHeight);
        //    //Center rectangle
        //    pos.X += (availableWidth - resLength * numRects) / 2;
        //    //Draw rectangles:
        //    for (int i = 0; i < numRects; i++)
        //    {
        //        Rectangle r = new Rectangle(pos, rectSize);
        //        if (i % 2 == 0)
        //            g.FillRectangle(pen.Brush, r);
        //        else
        //            g.DrawRectangle(pen, r);
        //        //Draw little vertical lines
        //        g.DrawLine(pen, pos, new Point(pos.X, pos.Y - 5));
        //        //Draw labels:
        //        int dist = i * totalDimenson;
        //        stringSize = g.MeasureString(dist + " " + unit, fontCustomScale);
        //        g.DrawString(dist + " " + unit, fontCustomScale, pen.Brush, new Point(pos.X - (int)stringSize.Width / 2, pos.Y - (7 + (int)stringSize.Height)));
        //        //Finally set new point
        //        pos = new Point(pos.X + resLength, pos.Y);
        //    }
        //    //Draw last line:
        //    g.DrawLine(pen, pos, new Point(pos.X, pos.Y - 5));
        //    //Draw last label
        //    int m = numRects * totalDimenson;
        //    stringSize = g.MeasureString(m + " " + unit, fontCustomScale);
        //    g.DrawString(m + " " + unit, fontCustomScale, pen.Brush, new Point(pos.X - (int)stringSize.Width / 2, pos.Y - (7 + (int)stringSize.Height)));
        //}
    }
}
