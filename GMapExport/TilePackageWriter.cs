using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GMap.NET.Internals;
using GMap.NET.MapProviders;
using ICSharpCode.SharpZipLib.Zip;
using GMapProvidersExt.Baidu;

namespace GMapExport
{
    public class TilePackageWriter : IDisposable
    {
        // Fields
        private ArcGISTileSchema tillingScheme;
        private ZipOutputStream zipFileStream;

        // Methods
        public TilePackageWriter(string path, ArcGISTileSchema scheme)
        {
            this.tillingScheme = scheme;
            if (File.Exists(path))
            {
                throw new Exception(string.Format("文件（{0}）已存在！", path));
            }
            this.Init(path);
        }

        public void Dispose()
        {
            this.zipFileStream.Flush();
            this.zipFileStream.Finish();
            this.zipFileStream.Close();
        }

        public static string GetCDIFileContent(ArcGISTileSchema scheme, GMapProvider mapProvider = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            builder.Append("<EnvelopeN xsi:type='typens:EnvelopeN' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:typens='http://www.esri.com/schemas/ArcGIS/10.1'>");
            builder.Append("<XMin>" + scheme.FullExtent.LowerCorner.X + "</XMin>");
            if ((mapProvider != null) && (((mapProvider is BaiduMapProvider) || (mapProvider is BaiduSatelliteMapProvider)) || (mapProvider is BaiduHybridMapProvider)))
            {
                builder.Append("<YMin>" + (scheme.FullExtent.LowerCorner.Y - 30000.0) + "</YMin>");
            }
            else
            {
                builder.Append("<YMin>" + scheme.FullExtent.LowerCorner.Y + "</YMin>");
            }
            builder.Append("<XMax>" + scheme.FullExtent.UpperCorner.X + "</XMax>");
            builder.Append("<YMax>" + scheme.FullExtent.UpperCorner.Y + "</YMax>");
            builder.Append("</EnvelopeN>");
            return builder.ToString();
        }

        public static string GetConfigFileContent(ArcGISTileSchema scheme)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            builder.Append("<CacheInfo xsi:type=\"typens:CacheInfo\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" xmlns:typens=\"http://www.esri.com/schemas/ArcGIS/10.1\">");
            builder.Append("<TileCacheInfo xsi:type=\"typens:TileCacheInfo\">");
            builder.Append(ArcGISPrjTool.GetSpatialReferenceXML(scheme.WKID));
            builder.Append(string.Format("<TileOrigin xsi:type=\"typens:PointN\"><X>{0}</X><Y>{1}</Y></TileOrigin>", scheme.TileOrigin.Lng, scheme.TileOrigin.Lat));
            builder.Append(string.Format("<TileCols>{0}</TileCols>", scheme.TileCols));
            builder.Append(string.Format("<TileRows>{0}</TileRows>", scheme.TileRows));
            builder.Append(string.Format("<DPI>{0}</DPI>", scheme.DPI));
            builder.Append("<LODInfos xsi:type=\"typens:ArrayOfLODInfo\">");
            foreach (LODInfo info in scheme.LODs)
            {
                builder.Append(string.Concat(new object[] { "<LODInfo xsi:type=\"typens:LODInfo\"><LevelID>", info.LevelID, "</LevelID><Scale>", info.Scale, "</Scale><Resolution>", info.Resolution, "</Resolution></LODInfo>" }));
            }
            builder.Append("</LODInfos>");
            builder.Append("</TileCacheInfo>");
            builder.Append("<TileImageInfo xsi:type=\"typens:TileImageInfo\">");
            builder.Append(string.Format("<CacheTileFormat>{0}</CacheTileFormat><CompressionQuality>0</CompressionQuality><Antialiasing>false</Antialiasing>", scheme.CacheTileFormat.ToString().ToUpper()));
            builder.Append("</TileImageInfo>");
            builder.Append(string.Format("<CacheStorageInfo xsi:type=\"typens:CacheStorageInfo\"><StorageFormat>{0}</StorageFormat><PacketSize>{1}</PacketSize></CacheStorageInfo></CacheInfo>", scheme.StorageFormat, scheme.PacketSize));
            return builder.ToString();
        }

        private void Init(string path)
        {
            this.zipFileStream = new ZipOutputStream(File.Create(path));
            this.WriteMetadataFiles();
        }

        private void WriteEsriInfo()
        {
        }

        private void WriteMetadataFiles()
        {
        }

        public void WriteTile(Tile tile)
        {
        }

        // Properties
        private string ImageSuffix
        {
            get
            {
                switch (this.tillingScheme.CacheTileFormat)
                {
                    case ImageFormat.PNG32:
                    case ImageFormat.PNG24:
                    case ImageFormat.PNG8:
                        return "png";
                }
                return this.tillingScheme.CacheTileFormat.ToString().ToLower();
            }
        }
    }


}
