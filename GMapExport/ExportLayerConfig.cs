using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMapCommonType;
using log4net;
using GMapProvidersExt.Baidu;

namespace GMapExport
{
    public class ExportLayerConfig
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExportLayerConfig));

        private string rootPath = "D://GisMap";
        private GMapProvider mapProvider;
        private int maxZoom;
        private int minZoom;
        private RectLatLng exportArea;

        private double DPI = 96.0;

        public ExportLayerConfig(RectLatLng area, int minZoom, int maxZoom, GMapProvider provider, string path)
        {
            this.rootPath = path;
            this.mapProvider = provider;
            this.minZoom = minZoom;
            this.maxZoom = maxZoom;
            this.exportArea = area;
        }

        public bool CreateArcGISMetaFile()
        {
            try
            {
                string cdiFile = this.rootPath + "/Layer/conf.cdi";
                string xmlFile = this.rootPath + "/Layer/conf.xml";
                if (!Directory.Exists(cdiFile.Substring(0, cdiFile.LastIndexOf('/'))) && !Directory.Exists(cdiFile.Substring(0, cdiFile.LastIndexOf('/'))))
                {
                    Directory.CreateDirectory(cdiFile.Substring(0, cdiFile.LastIndexOf('/')));
                }
                ArcGISTileSchema schema = GetArcGISTileSchema(EsriStorageFormat.esriMapCacheStorageModeExploded);

                this.GenerateArcGISCDIFile(cdiFile, schema);
                this.GenerateArcGISConfFile(xmlFile,schema);
                return true;
            }
            catch (Exception exception)
            {
                string msg = "创建ArcGIS切片元数据文件失败!";
                log.Error(msg+exception);
                return false;
            }
        }

        private void GenerateArcGISConfFile(string dstFile,ArcGISTileSchema schema)
        {
            string xmlFileContent = TilePackageWriter.GetConfigFileContent(schema);
            this.WriteXMLFile(dstFile, xmlFileContent);
        }

        private void GenerateArcGISCDIFile(string dstFile, ArcGISTileSchema schema)
        {
            string cdiFileContent = TilePackageWriter.GetCDIFileContent(schema, this.mapProvider);
            this.WriteXMLFile(dstFile, cdiFileContent);
        }

        public static string GetCDIFileContent(ArcGISTileSchema scheme, GMapProvider mapProvider = null)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            builder.Append("<EnvelopeN xsi:type='typens:EnvelopeN' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:typens='http://www.esri.com/schemas/ArcGIS/10.0'>");
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

        private List<int> GetExportLevels()
        {
            List<int> list = new List<int>();
            for(int z=this.minZoom; z<=this.maxZoom; ++z)
            {
                list.Add(z);
            }
            return list;
        }

        private ArcGISTileSchema GetArcGISTileSchema(EsriStorageFormat storageFormat)
        {
            List<int> zoomLevelList = this.GetExportLevels();

            ArcGISTileSchema arcGisTileSchema = new ArcGISTileSchema
            {
                CacheTileFormat = ImageFormat.PNG,
                CompressionQuality = 0,
                DPI = (int)this.DPI,
                FullExtent = BoundingBox.FromRectLngLat(this.GetProjectedView()),
                InitialExtent = BoundingBox.FromRectLngLat(this.GetProjectedView()),
                LODs = new LODInfo[zoomLevelList.Count]
            };
            int num = 0;
            foreach (int levelId in zoomLevelList)
            {
                LODInfo info = new LODInfo
                {
                    LevelID = levelId,
                    Resolution = this.mapProvider.Projection.GetLevelResolution(levelId),
                    Scale = this.mapProvider.Projection.GetLevelScale(levelId)
                };
                arcGisTileSchema.LODs[num++] = info;
            }
            if (storageFormat == EsriStorageFormat.esriMapCacheStorageModeExploded)
            {
                arcGisTileSchema.PacketSize = 0;
            }
            else if (storageFormat == EsriStorageFormat.esriMapCacheStorageModeCompact)
            {
                arcGisTileSchema.PacketSize = 128;
            }
            arcGisTileSchema.StorageFormat = storageFormat;
            arcGisTileSchema.TileOrigin = this.mapProvider.Projection.GetProjectedPoint(this.mapProvider.Projection.TileOrigin);
            arcGisTileSchema.TileCols = (int)this.mapProvider.Projection.TileSize.Width;
            arcGisTileSchema.TileRows = (int)this.mapProvider.Projection.TileSize.Height;
            arcGisTileSchema.WKID = this.mapProvider.Projection.EpsgCode;
            //arcGisTileSchema.WKID = 4326;
            //arcGisTileSchema.WKID = 3857;
            arcGisTileSchema.TPKName = new Guid().ToString();
            return arcGisTileSchema;
        }

        private RectLatLng GetProjectedView()
        {
            PointLatLng point1 = this.mapProvider.Projection.GetProjectedPoint(new PointLatLng(this.exportArea.Top, this.exportArea.Left));
            PointLatLng point2 = this.mapProvider.Projection.GetProjectedPoint(new PointLatLng(this.exportArea.Bottom, this.exportArea.Right));
            return RectLatLng.FromLTRB(point1.Lng, point1.Lat, point2.Lng, point2.Lat);
        }

        public bool CreateTmsMetaFile()
        {
            try
            {
                string dstFile = this.rootPath + "/tms.xml";
                string dirPath = dstFile.Substring(0, dstFile.LastIndexOf('/'));
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                GenerateTmsConfFile(dstFile);
                return true;
            }
            catch (Exception exception)
            {
                string msg = "创建TMS切片元数据文件失败!";
                log.Error(msg + exception);
                return false;
            }
        }

        private void GenerateTmsConfFile(string dstFile)
        {
            int epsgCode = this.mapProvider.Projection.EpsgCode;
            List<int> zoomLevelList = this.GetExportLevels();
            RectLatLng projectedViewBound = GetProjectedView();
            string imageFormat = "png";

            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            builder.Append("<tilemap tilemapservice=\"http://www.osgeo.org/services/tilemapservice.xml\" version=\"1.0.0\">");
            string title = "TMS Layer";
            builder.Append("<title>" + title + "</title>");
            builder.Append("<abstract></abstract>");
            if (epsgCode == 3857)
            {
                builder.Append("<srs>OSGEO:41001</srs>");
            }
            else
            {
                builder.Append("<srs>EPSG:" + epsgCode + "</srs>");
            }
            builder.Append("<vsrs></vsrs>");
            builder.Append(string.Concat(new object[] { "<boundingbox maxx=\"", projectedViewBound.LocationRightBottom.Lng, "\" maxy=\"", projectedViewBound.LocationTopLeft.Lat, "\" minx=\"", projectedViewBound.LocationTopLeft.Lng, "\" miny=\"", projectedViewBound.LocationRightBottom.Lat, "\" />" }));
            if (epsgCode == 3857)
            {
                builder.Append("<origin x=\"-20037508.3427890\" y=\"-20037508.3427890\" />");
            }
            else
            {
                builder.Append("<Origin x=\"-180\" y=\"-90\" /> ");
            }
            builder.Append("<tileformat extension=\"" + imageFormat + "\" height=\"256\" mime-type=\"image/" + imageFormat + "\" width=\"256\" />");
            if (epsgCode == 3857)
            {
                builder.Append("<tilesets profile=\"global-mercator\">");
            }
            else
            {
                builder.Append("<tilesets profile=\"global-geodetic\">");
            }
            foreach (int level in zoomLevelList)
            {
                if (epsgCode == 3857)
                {
                    builder.Append(string.Concat(new object[] { "<tileset href=\"\" order=\"", level, "\" units-per-pixel=\"", 78271.516 / Math.Pow(2.0, (double)level), "\" />" }));
                }
                else
                {
                    builder.Append(string.Concat(new object[] { "<tileset href=\"\" order=\"", level, "\" units-per-pixel=\"", 0.703125 / Math.Pow(2.0, (double)level), "\" />" }));
                }
            }
            builder.Append("</tilesets>");
            builder.Append("</tilemap>");
            this.WriteXMLFile(dstFile, builder.ToString());
        }

        //private ImageFormat GetCSharpImageFormat()
        //{
        //    switch (this.exportParam.GridFormat)
        //    {
        //        case GridFormat.Png:
        //            return ImageFormat.Png;

        //        case GridFormat.Jpeg:
        //            return ImageFormat.Jpeg;

        //        case GridFormat.GTiff:
        //            return ImageFormat.Tiff;

        //        case GridFormat.Img:
        //            return null;

        //        case GridFormat.Bmp:
        //            return ImageFormat.Bmp;

        //        case GridFormat.Gif:
        //            return ImageFormat.Gif;
        //    }
        //    return null;
        //}

        private void WriteXMLFile(string path, string xml)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(xml);
                    writer.Flush();
                    writer.Dispose();
                }
            }
            catch (Exception exception)
            {
                string msg = "配置文件：" + path + "创建失败！异常信息：" + exception.Message;
                log.Error(msg);
                throw;
            }
        }

    }
}
