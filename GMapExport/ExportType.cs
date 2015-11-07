using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMapExport
{
    public enum ExportType
    {
        DEFAULTSPLICE,
        GOOGLETILE,
        ARCGISTILE,
        TMSTILE,
        MBTILES,
        SQLITEDB,
        ORUXMAPS,
        BAIDUTILE,
        ARCGISTPK_EXPOLDED,
        GEOPACKAGE,
        AZDB
    }


    public enum ResampleFormat
    {
        Cubic,
        CubicSpline,
        Bilinear,
        NearestNeighbour
    }


}
