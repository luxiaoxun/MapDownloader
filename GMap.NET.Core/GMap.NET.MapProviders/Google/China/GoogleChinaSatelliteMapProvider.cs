
namespace GMap.NET.MapProviders
{
   using System;

   /// <summary>
   /// GoogleChinaSatelliteMap provider
   /// </summary>
   public class GoogleChinaSatelliteMapProvider : GoogleMapProviderBase
   {
      public static readonly GoogleChinaSatelliteMapProvider Instance;

      GoogleChinaSatelliteMapProvider()
      {
         RefererUrl = string.Format("http://ditu.{0}/", ServerChina);
      }

      static GoogleChinaSatelliteMapProvider()
      {
         Instance = new GoogleChinaSatelliteMapProvider();
      }

      public string Version = "s@130";

      #region GMapProvider Members

      readonly Guid id = new Guid("543009AC-3379-4893-B580-DBE6372B1753");
      public override Guid Id
      {
         get
         {
            return id;
         }
      }

      private readonly string cnName = "Google中国卫星地图";
      public string CnName
      {
          get
          {
              return this.cnName;
          }
      }

      readonly string name = "GoogleChinaSatelliteMap";
      public override string Name
      {
         get
         {
            return name;
         }
      }

      public override PureImage GetTileImage(GPoint pos, int zoom)
      {
          try
          {
              string url = MakeTileImageUrl(pos, zoom, LanguageStr);

              return GetTileImageUsingHttp(url);
          }
          catch (Exception ex)
          {
              return null;
          }
      }

      #endregion

      string MakeTileImageUrl(GPoint pos, int zoom, string language)
      {
         string sec1 = string.Empty; // after &x=...
         string sec2 = string.Empty; // after &zoom=...
         GetSecureWords(pos, out sec1, out sec2);

         //return string.Format(UrlFormat, UrlFormatServer, GetServerNum(pos, 4), UrlFormatRequest, Version, pos.X, sec1, pos.Y, zoom, sec2, ServerChina);
         return string.Format(UrlFormat, pos.X, pos.Y, zoom);
      }

      static readonly string UrlFormatServer = "mt";
      static readonly string UrlFormatRequest = "vt";
      //static readonly string UrlFormat = "http://{0}{1}.{9}/{2}/lyrs={3}&gl=cn&x={4}{5}&y={6}&z={7}&s={8}";
      //http://www.google.cn/maps/vt?lyrs=s@167&gl=cn&x=54390&y=26610&z=16
      static readonly string UrlFormat = "http://www.google.cn/maps/vt?lyrs=s@167&gl=cn&x={0}&y={1}&z={2}";
   }
}