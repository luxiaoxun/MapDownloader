using System;
using System.Collections.Generic;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;

namespace GMapProvidersExt
{
    public class BaiduMapSateliteProvider : BaiduMapProviderBase
    {
        public static readonly BaiduMapSateliteProvider Instance;

        readonly Guid id = new Guid("89A10DFA-2557-431a-9656-20064E8D1342");
        public override Guid Id
        {
            get { return id; }
        }

        readonly string name = "BaiduSateliteMap";
        public override string Name
        {
            get { return name; }
        }


        static BaiduMapSateliteProvider()
        {
            Instance = new BaiduMapSateliteProvider();
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

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            zoom = zoom - 1;
            var offsetX = Math.Pow(2, zoom);
            var offsetY = offsetX - 1;

            var numX = pos.X - offsetX;
            var numY = -pos.Y + offsetY;

            zoom = zoom + 1;
            var num = (pos.X + pos.Y) % 4 + 1;
            var x = numX.ToString().Replace("-", "M");
            var y = numY.ToString().Replace("-", "M");

            //return "http://q3.baidu.com/it/u=x=721;y=209;z=12;v=014;type=web&fm=44";
            //http://q3.baidu.com/it/u=x=721;y=209;z=12;v=014;type=web&fm=44
            string url = string.Format(UrlFormat, pos.X, pos.Y, zoom);
            //string url = string.Format(UrlFormat, x, y, zoom);
            return url;
        }

        //static readonly string UrlFormat = "http://q{0}.baidu.com/it/u=x={1};y={2};z={3};v={4};type={5}&fm={6}";
        //static readonly string UrlFormat = "http://online{0}.map.bdimg.com/tile/?qt=tile&x={1}&y={2}&z={3}&styles=pl&udt=20140314";
        static readonly string UrlFormat = "http://shangetu2.map.bdimg.com/it/u=x={0};y={1};z={2};v=009;type=sate&fm=46&udt=20140117";
    }
}
