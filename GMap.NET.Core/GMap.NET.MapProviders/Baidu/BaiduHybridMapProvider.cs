using System;
using System.Collections.Generic;
using System.Text;

namespace GMap.NET.MapProviders
{
   public class BaiduHybridMapProvider : BaiduMapProviderBase
   {
      public static readonly BaiduHybridMapProvider Instance;

      readonly Guid id = new Guid("AF522C29-9F94-4E9B-BDB3-346CD058AE7B");
      public override Guid Id
      {
         get { return id; }
      }

      readonly string name = "BaiduHybridMap";
      public override string Name
      {
         get { return name; }
      }

      GMapProvider[] overlays;
      public override GMapProvider[] Overlays
      {
         get
         {
            if (overlays == null)
            {
               overlays = new GMapProvider[] { BaiduSateliteMapProvider.Instance, this };
            }
            return overlays;
         }
      }

      static BaiduHybridMapProvider()
      {
         Instance = new BaiduHybridMapProvider();
      }

      public override PureImage GetTileImage(GPoint pos, int zoom)
      {
         string url = MakeTileImageUrl(pos, zoom, LanguageStr);

         return GetTileImageUsingHttp(url);
      }

      private string MakeTileImageUrl(GPoint pos, int zoom, string language)
      {
         zoom = zoom - 1;
         var offsetX = Math.Pow(2, zoom);
         var offsetY = offsetX - 1;

         var numX = pos.X - offsetX;
         var numY = -pos.Y + offsetY;

         zoom = zoom + 1;
         var num = (pos.X + pos.Y) % 8 + 1;
         var x = numX.ToString().Replace("-", "M");
         var y = numY.ToString().Replace("-", "M");

         string url = string.Format(UrlFormat, num, x, y, zoom, "009", "sate", "46");
         Console.WriteLine("url:" + url);
         return url;
      }

      static readonly string UrlFormat = "http://online{0}.map.bdimg.com/tile/?qt=tile&x={1}&y={2}&z={3}&styles=sl&v={4}";
   }
}
