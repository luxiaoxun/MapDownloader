using System;
using System.Collections.Generic;
using System.Text;

namespace GMap.NET.Projections
{
   /// <summary>
   /// The Mercator projection for Baidu
   /// PROJCS["World_Mercator",GEOGCS["GCS_WGS_1984",DATUM["D_WGS_1984",SPHEROID["WGS_1984",6378137,298.257223563]],PRIMEM["Greenwich",0],UNIT["Degree",0.017453292519943295]],PROJECTION["Mercator"],PARAMETER["False_Easting",0],PARAMETER["False_Northing",0],PARAMETER["Central_Meridian",0],PARAMETER["standard_parallel_1",0],UNIT["Meter",1]]
   /// </summary>
   public class BaiduProjection : PureProjection
   {
      // 百度经纬度 -> 百度墨卡托 -> 像素坐标
      public static readonly BaiduProjection Instance = new BaiduProjection();

      static readonly double MinLatitude = -74;       // 最小纬度
      static readonly double MaxLatitude = 74;        // 最大纬度
      static readonly double MinLongitude = -180;     // 最小经度
      static readonly double MaxLongitude = 180;      // 最大经度
      readonly GSize tileSize = new GSize(256, 256);  // 瓦片大小

      // 别问这一堆奇葩的参数哪来的 我不会告诉你是从百度地图JavascriptAPI里一堆JS文件里翻出来的 ╭(╯^╰)╮
      static readonly double[] MCBAND = new double[] { 12890594.86, 8362377.87, 5591021, 3481989.83, 1678043.12, 0 };
      static readonly int[] LLBAND = new int[] { 75, 60, 45, 30, 15, 0 };
      static readonly double[][] MC2LL = new double[][] {
                                           new double[] { 1.410526172116255e-8, 0.00000898305509648872, -1.9939833816331, 200.9824383106796, -187.2403703815547, 91.6087516669843, -23.38765649603339, 2.57121317296198, -0.03801003308653, 17337981.2 },
                                           new double[] { -7.435856389565537e-9, 0.000008983055097726239, -0.78625201886289, 96.32687599759846, -1.85204757529826, -59.36935905485877, 47.40033549296737, -16.50741931063887, 2.28786674699375, 10260144.86 },
                                           new double[] { -3.030883460898826e-8, 0.00000898305509983578, 0.30071316287616, 59.74293618442277, 7.357984074871, -25.38371002664745, 13.45380521110908, -3.29883767235584, 0.32710905363475, 6856817.37 },
                                           new double[] { -1.981981304930552e-8, 0.000008983055099779535, 0.03278182852591, 40.31678527705744, 0.65659298677277, -4.44255534477492, 0.85341911805263, 0.12923347998204, -0.04625736007561, 4482777.06 },
                                           new double[] { 3.09191371068437e-9, 0.000008983055096812155, 0.00006995724062, 23.10934304144901, -0.00023663490511, -0.6321817810242, -0.00663494467273, 0.03430082397953, -0.00466043876332, 2555164.4 },
                                           new double[] { 2.890871144776878e-9, 0.000008983055095805407, -3.068298e-8, 7.47137025468032, -0.00000353937994, -0.02145144861037, -0.00001234426596, 0.00010322952773, -0.00000323890364, 826088.5 } };
      static readonly double[][] LL2MC = new double[][] {
                                           new double[] { -0.0015702102444, 111320.7020616939, 1704480524535203, -10338987376042340, 26112667856603880, -35149669176653700, 26595700718403920, -10725012454188240, 1800819912950474, 82.5 },
                                           new double[] { 0.0008277824516172526, 111320.7020463578, 647795574.6671607, -4082003173.641316, 10774905663.51142, -15171875531.51559, 12053065338.62167, -5124939663.577472, 913311935.9512032, 67.5 },
                                           new double[] { 0.00337398766765, 111320.7020202162, 4481351.045890365, -23393751.19931662, 79682215.47186455, -115964993.2797253, 97236711.15602145, -43661946.33752821, 8477230.501135234, 52.5 },
                                           new double[] { 0.00220636496208, 111320.7020209128, 51751.86112841131, 3796837.749470245, 992013.7397791013, -1221952.21711287, 1340652.697009075, -620943.6990984312, 144416.9293806241, 37.5 },
                                           new double[] { -0.0003441963504368392, 111320.7020576856, 278.2353980772752, 2485758.690035394, 6070.750963243378, 54821.18345352118, 9540.606633304236, -2710.55326746645, 1405.483844121726, 22.5 },
                                           new double[] { -0.0003218135878613132, 111320.7020701615, 0.00369383431289, 823725.6402795718, 0.46104986909093, 2351.343141331292, 1.58060784298199, 8.77738589078284, 0.37238884252424, 7.45 } };

      /// <summary>
      /// 瓦片尺寸
      /// </summary>
      public override GSize TileSize
      {
         get { return tileSize; }
      }
      /// <summary>
      /// 赤道半径墨卡托投影
      /// </summary>
      public override double Axis
      {
         get { return 6378137; }
      }
      /// <summary>
      /// 地球扁率
      /// </summary>
      public override double Flattening
      {
         get { return 1.0 / 298.257223563; }
      }
      /// <summary>
      /// 经纬坐标->像素 
      /// ☆百度的像素坐标有负值
      /// ☆但是像素这东东应该全是正值
      /// ☆所以要把像素值加上偏移量让他变正
      /// ☆反算时再把偏移量减掉
      /// </summary>
      /// <param name="lat"></param>
      /// <param name="lng"></param>
      /// <param name="zoom"></param>
      /// <returns></returns>
      public override GPoint FromLatLngToPixel(double lat, double lng, int zoom)
      {
         // 经纬度 -> 墨卡托平面坐标 -> 像素坐标
         double[] mcpoint = LngLatToMercator(lat, lng);
         double mcX = mcpoint[0];
         double mcY = mcpoint[1];
         double[] pixelpoint = MercatorToPixel(mcX, mcY, zoom);
         double piX = pixelpoint[0];
         double piY = pixelpoint[1];

         // DoDo 像素坐标加上相应的偏移量
         double offsetX = Math.Pow(2, zoom - 1) * tileSize.Width;
         double offsetY = Math.Pow(2, zoom - 1) * tileSize.Height;
         piX += offsetX;
         piY += offsetY;

         return new GPoint((long)Math.Round(piX), (long)Math.Round(piY));
      }
      /// <summary>
      /// 像素点到瓦片索引块坐标
      /// </summary>
      /// <param name="pixel"></param>
      /// <returns></returns>
      public override GPoint FromPixelToTileXY(GPoint pixel)
      {
         long tileX = (long)Math.Floor(pixel.X / (double)tileSize.Width);
         long tileY = (long)Math.Floor(pixel.Y / (double)tileSize.Height);
         return new GPoint(tileX, tileY);
      }
      /// <summary>
      /// 瓦片左上角对应的墨卡托像素点坐标
      /// </summary>
      /// <param name="tile"></param>
      /// <returns></returns>
      public override GPoint FromTileXYToPixel(GPoint tile)
      {
         long pixelX = tile.X * tileSize.Width;
         long pixelY = tile.Y * tileSize.Height;
         return new GPoint(pixelX, pixelY);
      }
      /// <summary>
      /// 像素坐标到经纬度
      /// ☆百度的像素坐标有负值
      /// ☆但是像素这东东应该全是正值
      /// ☆所以要把像素值加上偏移量让他变正
      /// ☆反算时再把偏移量减掉
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="zoom"></param>
      /// <returns></returns>
      public override PointLatLng FromPixelToLatLng(long x, long y, int zoom)
      {
         // DoDo 像素坐标加上相应的偏移量
         double offsetX = Math.Pow(2, zoom - 1) * tileSize.Width;
         double offsetY = Math.Pow(2, zoom - 1) * tileSize.Height;
         double newx = x - offsetX;
         double newy = y - offsetY;

         double zoomUnits = GetZoomUnits(zoom);        // 分辨率
         double mercatorx = zoomUnits * newx;
         double mercatory = zoomUnits * newy;
         return MercatorToLngLat((long)Math.Round(mercatorx), (long)Math.Round(mercatory));
      }
      /// <summary>
      /// 不知道 - -;
      /// </summary>
      /// <param name="zoom"></param>
      /// <returns></returns>
      public override GSize GetTileMatrixMinXY(int zoom)
      {
         return new GSize(0, 0);
      }
      /// <summary>
      /// 也不知道 - -;
      /// </summary>
      /// <param name="zoom"></param>
      /// <returns></returns>
      public override GSize GetTileMatrixMaxXY(int zoom)
      {
         long xy = (1 << zoom);
         return new GSize(xy - 1, xy - 1);
      }
      /// <summary>
      /// 墨卡托坐标转像素坐标
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <param name="zoom"></param>
      /// <returns></returns>
      private double[] MercatorToPixel(double x, double y, int zoom)
      {
         double pixelX = Math.Floor(x * Math.Pow(2, zoom - 18));
         double pixelY = Math.Floor(y * Math.Pow(2, zoom - 18));
         return new double[] { pixelX, pixelY };
      }
      /// <summary>
      /// 像素坐标转瓦片XY 图块坐标
      /// </summary>
      /// <param name="x"></param>
      /// <param name="y"></param>
      /// <returns></returns>
      private GPoint PixelToTile(long x, long y)
      {
         int tileX = (int)(Math.Floor((double)(x / tileSize.Width)));
         int tileY = (int)(Math.Floor((double)(y / tileSize.Height)));
         return new GPoint(tileX, tileY);
      }
      /// <summary>
      /// MercatorToLngLat
      /// </summary>
      /// <param name="x">X坐标 (经度)</param>
      /// <param name="y">Y坐标 (纬度)</param>
      /// <returns>经度, 纬度</returns>
      private PointLatLng MercatorToLngLat(long x, long y)
      {
         double[] mc = null;
         long absx = Math.Abs(x);
         long absy = Math.Abs(y);
         for (int i = 0; i < MCBAND.Length; i++)
         {
            if (absy >= MCBAND[i])
            {
               mc = MC2LL[i];
               break;
            }
         }
         double[] location = Convertor(x, y, mc);
         return new PointLatLng(location[1], location[0]);
      }
      /// <summary>
      /// 经纬度转墨卡托坐标
      /// </summary>
      /// <param name="lat">纬度</param>
      /// <param name="lng">经度</param>
      /// <returns>墨卡托平面坐标XY</returns>
      private double[] LngLatToMercator(double lat, double lng)
      {
         double[] mc = null;
         //lng = GetLoop(lng, MinLongitude, MaxLongitude);
         //lat = GetRange(lat, MinLatitude, MaxLatitude);
         lat = Clip(lat, MinLatitude, MaxLatitude);
         lng = Clip(lng, MinLongitude, MaxLongitude);

         for (int i = 0; i < LLBAND.Length; i++)
         {
            if (lat > LLBAND[i])
            {
               mc = LL2MC[i];
               break;
            }
         }
         // 
         if (mc == null)
         {
            for (int i = LLBAND.Length - 1; i >= 0; i--)
            {
               if (lat <= -LLBAND[i])
               {
                  mc = LL2MC[i];
                  break;
               }
            }
         }
         // 
         double[] location = Convertor(lng, lat, mc);
         return location;
      }
      /// <summary>
      /// 经度范围 左右转一圈
      /// </summary>
      /// <param name="lng"></param>
      /// <param name="a">-180度</param>
      /// <param name="b">180度</param>
      /// <returns></returns>
      private double GetLoop(double lng, double a, double b)
      {
         while (lng > b) lng -= b - a;
         while (lng < a) lng += b - a;
         return lng;
      }
      /// <summary>
      /// 纬度范围 上下不能超过
      /// </summary>
      /// <param name="lat"></param>
      /// <param name="a">-74</param>
      /// <param name="b">74</param>
      /// <returns></returns>
      private double GetRange(double lat, double a, double b)
      {
         lat = Math.Max(lat, a);
         lat = Math.Min(lat, b);
         return lat;
      }
      /// <summary>
      /// 获取分辨率 Fix: DoDo 修正了地图缩放到19级出错的BUG
      /// </summary>
      /// <param name="zoom"></param>
      /// <returns></returns>
      private double GetZoomUnits(int zoom)
      {
         return Math.Pow(2, (18 - zoom));
      }
      /// <summary>
      /// 数据转换
      /// </summary>
      /// <param name="xlng">墨卡托X 或者 经度</param>
      /// <param name="ylat">墨卡托Y 或者 纬度</param>
      /// <param name="mc">转换对照表</param>
      /// <returns>经纬度 或 墨卡托XY</returns>
      private double[] Convertor(double xlng, double ylat, double[] mc)
      {
         double newxlng = mc[0] + mc[1] * Math.Abs(xlng);
         double c = Math.Abs(ylat) / mc[9];
         double newylat = mc[2] + mc[3] * c + mc[4] * c * c + mc[5] * c * c * c + mc[6] * c * c * c * c + mc[7] * c * c * c * c * c + mc[8] * c * c * c * c * c * c;
         if (xlng < 0) newxlng *= -1;
         if (ylat > 0) newylat *= -1;
         return new double[] { newxlng, newylat };
      }
   }
}
