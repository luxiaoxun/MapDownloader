using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMapDownload
{
    public class DownloadThreadArgs
    {
        public List<DownloadLevelTile> DownloadLevelTiles { set; get; }

        public DownloadThreadArgs(List<DownloadLevelTile> downloadLevelTiles)
        {
            this.DownloadLevelTiles = downloadLevelTiles;
        }
    }
}
