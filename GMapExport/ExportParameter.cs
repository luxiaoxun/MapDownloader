using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapExport
{
    public class ExportParameter
    {
        public RectLatLng ExportRect { set; get; }

        public ExportType ExportType { set; get; }

        public GMapProvider MapProvider { set; get; }

        public int MaxZoom { set; get; }

        public int MinZoom { set; get; }

        public string ExportPath { set; get; }

    }
}
