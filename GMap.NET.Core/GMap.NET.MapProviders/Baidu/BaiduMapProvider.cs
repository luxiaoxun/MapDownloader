using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Xml;
using GMap.NET.Internals;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMap.NET.MapProviders
{
   /// <summary>
   /// Baidu 地图 DoDo
   /// </summary>
   public abstract class BaiduMapProviderBase : GMapProvider
   {
      private string ClientKey = "1308e84a0e8a1fc2115263a4b3cf87f1";
      public BaiduMapProviderBase()
      {
         MinZoom = 3;
         MaxZoom = 19;
         RefererUrl = "http://map.baidu.com";
         Copyright = string.Format("©{0} Baidu Corporation, ©{0} NAVTEQ, ©{0} Image courtesy of NASA", DateTime.Today.Year);
      }

      /// <summary>
      /// 地图坐标投影 这个必须要修改
      /// </summary>
      public override PureProjection Projection
      {
         get { return BaiduProjection.Instance; }
      }

      private GMapProvider[] overlays;
      public override GMapProvider[] Overlays
      {
         get
         {
            if (overlays == null)
            {
               overlays = new GMapProvider[] { this };
            }
            return overlays;
         }
      }
   }

   public class BaiduMapProvider : BaiduMapProviderBase
   {
      public static readonly BaiduMapProvider Instance;

      readonly Guid id = new Guid("608748FC-5FDD-4d3a-9027-356F24A755E5");
      public override Guid Id
      {
         get { return id; }
      }

      readonly string name = "BaiduMap";
      public override string Name
      {
         get { return name; }
      }
      static BaiduMapProvider()
      {
         Instance = new BaiduMapProvider();
      }

      public override PureImage GetTileImage(GPoint pos, int zoom)
      {
         string url = MakeTileImageUrl(pos, zoom, LanguageStr);
         return GetTileImageUsingHttp(url);
      }

      private string MakeTileImageUrl(GPoint pos, int zoom, string language)
      {
         var offsetX = Math.Pow(2, zoom - 1);
         var offsetY = offsetX - 1;

         var numX = pos.X - offsetX;
         var numY = -pos.Y + offsetY;

         var x = numX.ToString().Replace("-", "M");
         var y = numY.ToString().Replace("-", "M");

         //原来：http://q3.baidu.com/it/u=x=721;y=209;z=12;v=014;type=web&fm=44
         //更新：http://online1.map.bdimg.com/tile/?qt=tile&x=23144&y=6686&z=17&styles=pl
         string url = string.Format(UrlFormat, x, y, zoom);
         //url = string.Format("http://placehold.it/256x256&text=({0}, {1}, {2})", x, y, zoom);
         return url;
      }

      //http://placehold.it/256x256&text=text
      static readonly string UrlFormat = "http://online1.map.bdimg.com/tile/?qt=tile&x={0}&y={1}&z={2}&styles=pl";
   }
}