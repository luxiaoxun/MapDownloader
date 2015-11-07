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
    public class ArcGISLayerConfig
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ArcGISLayerConfig));

        private string rootPath = "D://GisMap";
        private GMapProvider mapProvider;
        private int maxZoom;
        private int minZoom;
        private RectLatLng exportArea;

        private double DPI = 96.0;

        public ArcGISLayerConfig(RectLatLng area, int minZoom, int maxZoom, GMapProvider provider, string path)
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

        public List<int> GetExportLevels()
        {
            List<int> list = new List<int>();
            //foreach (int num in this.exportParam.Levels)
            //{
            //    if (this.downloadTask.Levels.Contains(num))
            //    {
            //        list.Add(num);
            //    }
            //}
            for(int z=this.minZoom; z<=this.maxZoom; ++z)
            {
                list.Add(z);
            }
            return list;
        }

        //private RectifyTool rectifyTool;

        private ArcGISTileSchema GetArcGISTileSchema(EsriStorageFormat storageFormat)
        {
            List<int> zoomLevelList = this.GetExportLevels();

            ArcGISTileSchema arcGisTileSchema = new ArcGISTileSchema
            {
                //CacheTileFormat = (ImageFormat)Enum.Parse(typeof(ImageFormat), this.GetCSharpImageFormat().ToString(), true),
                CacheTileFormat = ImageFormat.PNG,
                CompressionQuality = 0,
                DPI = (int)this.DPI,
                //FullExtent = BoundingBox.FromRectLngLat(this.downloadTask.ProjectedViewBounds),
                //InitialExtent = BoundingBox.FromRectLngLat(this.downloadTask.ProjectedViewBounds),
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
            arcGisTileSchema.StorageFormat = storageFormat;
            arcGisTileSchema.TileOrigin = this.mapProvider.Projection.GetProjectedPoint(this.mapProvider.Projection.TileOrigin);
            //arcGisTileSchema.TileOrigin = this.mapProvider.Projection.TileOrigin;
            //if (this.exportParam.Rectify)
            //{
            //    string prjWkt = this.downloadTask.PrjWkt;
            //    Point2D pointd = null;
            //    if (this.exportParam.RectifyType == RectifyType.Center)
            //    {
            //        pointd = this.rectifyTool.MarsToPrjOffset(this.downloadTask.ViewBounds.LocationMiddle, this.exportParam.RecX, this.exportParam.RecY, prjWkt);
            //    }
            //    else if (this.exportParam.RectifyType == RectifyType.LeftTop)
            //    {
            //        pointd = this.rectifyTool.MarsToPrjOffset(this.downloadTask.ViewBounds.LocationTopLeft, this.exportParam.RecX, this.exportParam.RecY, prjWkt);
            //    }
            //    else if (this.exportParam.RectifyType == RectifyType.RightBottom)
            //    {
            //        pointd = this.rectifyTool.MarsToPrjOffset(this.downloadTask.ViewBounds.LocationRightBottom, this.exportParam.RecX, this.exportParam.RecY, prjWkt);
            //    }
            //    if (pointd != null)
            //    {
            //        Point2D tileOrigin = arcGisTileSchema.TileOrigin;
            //        tileOrigin.X += pointd.X;
            //        Point2D pointd2 = arcGisTileSchema.TileOrigin;
            //        pointd2.Y += pointd.Y;
            //    }
            //}
            arcGisTileSchema.TileCols = (int)this.mapProvider.Projection.TileSize.Width;
            arcGisTileSchema.TileRows = (int)this.mapProvider.Projection.TileSize.Height;
            arcGisTileSchema.WKID = this.mapProvider.Projection.EpsgCode;
            //arcGisTileSchema.WKID = 4326;
            //arcGisTileSchema.WKID = 3857;
            //arcGisTileSchema.TPKName = this.downloadTask.Name;
            arcGisTileSchema.TPKName = new Guid().ToString();
            return arcGisTileSchema;
        }

        private RectLatLng GetProjectedView()
        {
            PointLatLng point1 = this.mapProvider.Projection.GetProjectedPoint(new PointLatLng(this.exportArea.Top, this.exportArea.Left));
            PointLatLng point2 = this.mapProvider.Projection.GetProjectedPoint(new PointLatLng(this.exportArea.Bottom, this.exportArea.Right));
            return RectLatLng.FromLTRB(point1.Lng, point1.Lat, point2.Lng, point2.Lat);
        }

        //private ExportParam exportParam;

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
