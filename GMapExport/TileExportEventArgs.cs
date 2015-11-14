using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMapExport
{
    public class TileExportEventArgs:EventArgs
    {
        public int CurrentExportZoom { set; get; }

        public TileExportEventArgs()
        {
        }

        public TileExportEventArgs(int exportZoom)
        {
            CurrentExportZoom = exportZoom;
        }
    }
}
