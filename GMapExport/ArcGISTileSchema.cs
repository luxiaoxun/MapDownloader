using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMapCommonType;

namespace GMapExport
{
    public class ArcGISTileSchema
    {
        // Properties
        public ImageFormat CacheTileFormat { get; set; }

        public int CompressionQuality { get; set; }

        public int DPI { get; set; }

        public BoundingBox FullExtent { get; set; }

        public BoundingBox InitialExtent { get; set; }

        public LODInfo[] LODs { get; set; }

        public string LODsJson { get; set; }

        public int PacketSize { get; set; }

        public string Path { get; set; }

        public string RestResponseArcGISJson { get; set; }

        public string RestResponseArcGISPJson { get; set; }

        public EsriStorageFormat StorageFormat { get; set; }

        public int TileCols { get; set; }

        public PointLatLng TileOrigin { get; set; }

        public int TileRows { get; set; }

        public string TPKName { get; set; }

        public string TPKPath { get; set; }

        public int WKID { get; set; }

        public string WKT { get; set; }
    }


}
