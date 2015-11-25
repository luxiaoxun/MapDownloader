using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

namespace gheat
{
    public class Tools
    {
        /// <summary>
        ///Take From
        ///http://www.gutgames.com/post/Adjusting-Brightness-of-an-Image-in-C.aspx
        /// </summary>
        /// <param name="Image"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static Bitmap AdjustBrightness(Bitmap Image, int Value)
        {
            System.Drawing.Bitmap TempBitmap = Image;
            float FinalValue = (float)Value / 255.0f;
            System.Drawing.Bitmap NewBitmap = new System.Drawing.Bitmap(TempBitmap.Width, TempBitmap.Height);
            System.Drawing.Graphics NewGraphics = System.Drawing.Graphics.FromImage(NewBitmap);
            float[][] FloatColorMatrix ={
			                        new float[] {1, 0, 0, 0, 0},
			                        new float[] {0, 1, 0, 0, 0},
			                        new float[] {0, 0, 1, 0, 0},
			                        new float[] {0, 0, 0, 1, 0},
			                        new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
		                            };

            ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);
            ImageAttributes Attributes = new ImageAttributes();
            Attributes.SetColorMatrix(NewColorMatrix);
            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, System.Drawing.GraphicsUnit.Pixel, Attributes);
            Attributes.Dispose();
            NewGraphics.Dispose();
            return NewBitmap;
        } 
    }
}
