using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace GMapMarkerLib
{
    public class GifImage
    {
        // Fields
        private int currentFrame = -1;
        private FrameDimension dimension;
        private int frameCount;
        private Image gifImage;
        private Hashtable imgMap = new Hashtable();
        private bool autoReverse;
        private int step = 1;

        public Size Size { get; set; }

        // Methods
        public GifImage(string path)
        {
            this.gifImage = Image.FromFile(path);
            this.dimension = new FrameDimension(this.gifImage.FrameDimensionsList[0]);
            this.frameCount = this.gifImage.GetFrameCount(this.dimension);
            this.Size = this.gifImage.Size;
        }

        public GifImage(Image gifimage)
        {
            this.gifImage = gifimage;
            this.dimension = new FrameDimension(this.gifImage.FrameDimensionsList[0]);
            this.frameCount = this.gifImage.GetFrameCount(this.dimension);
            this.Size = this.gifImage.Size;
        }

        public Image GetFrame(int index)
        {
            Image image = (Image)this.imgMap[index];
            if (image == null)
            {
                this.gifImage.SelectActiveFrame(this.dimension, index);
                image = (Image)this.gifImage.Clone();
                this.imgMap[index] = image;
            }
            return image;
        }

        public Image GetNextFrame()
        {
            this.currentFrame += this.step;
            if ((this.currentFrame >= this.frameCount) || (this.currentFrame < 1))
            {
                if (this.autoReverse)
                {
                    this.step *= -1;
                    this.currentFrame += this.step;
                }
                else
                {
                    this.currentFrame = 0;
                }
            }
            return this.GetFrame(this.currentFrame);
        }

        // Properties
        public bool ReverseAtEnd
        {
            get
            {
                return this.autoReverse;
            }
            set
            {
                this.autoReverse = value;
            }
        }
    }
}
