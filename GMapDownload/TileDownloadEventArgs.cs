using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMapDownload
{
    public class TileDownloadEventArgs : EventArgs
    {
        public int ProgressValue { set; get; }

        public int TileAllNum { set; get; }

        public int TileCompleteNum { set; get; }

        public int CurrentDownloadZoom { set; get; }

        public TileDownloadEventArgs(int value)
        {
            ProgressValue = value;
        }

        public TileDownloadEventArgs(int allNum, int comNum)
        {
            TileAllNum = allNum;
            TileCompleteNum = comNum;
            ProgressValue = TileCompleteNum * 100 / TileAllNum;
        }

        public TileDownloadEventArgs(int allNum, int comNum, int zoom)
        {
            TileAllNum = allNum;
            TileCompleteNum = comNum;
            CurrentDownloadZoom = zoom;
            ProgressValue = TileCompleteNum * 100 / TileAllNum;
        }
    }
}
