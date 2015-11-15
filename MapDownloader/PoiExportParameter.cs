using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MapDownloader
{
    public class PoiExportParameter
    {
        public DataTable Data { set; get; }

        public string Path { set; get;}

        public int ExportType { set; get; }

    }
}
