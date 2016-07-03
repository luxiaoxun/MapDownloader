using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu
{
    public class TiandituMapProvider4326 : TiandituProviderBase4326
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("d12187df-6c54-4c62-b43c-e957b2589797");
        public static readonly TiandituMapProvider4326 Instance;
        private readonly string name;

        // Methods
        static TiandituMapProvider4326()
        {
            Instance = new TiandituMapProvider4326();
        }

        public TiandituMapProvider4326()
        {
            //this.id = new Guid("d12187df-6c54-4c62-b43c-e957b2589797");
            this.name = "TiandituMapNoAnno4326";
            this.cnName = "天地图街道地图(无注记-WGS84)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "vec_c";
                int serverIndex = GMapProvider.GetServerNum(pos, TiandituProviderBase4326.maxServer);
                //string url = string.Format(TiandituProviderBase4326.UrlFormat, new object[] { serverIndex, str, str.Substring(0, str.Length - 2), str.Substring(str.Length - 1), zoom, pos.Y, pos.X });
                string url = string.Format(TiandituProviderBase4326.UrlFormat, new object[] { serverIndex, str, pos.X, pos.Y, zoom });
                return base.GetTileImageUsingHttp(url);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Properties
        public string CnName
        {
            get
            {
                return this.cnName;
            }
        }

        public override Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public bool IsEmptyAreaNotReturnData
        {
            get
            {
                return true;
            }
        }

        public override string Name
        {
            get
            {
                return this.name;
            }
        }
    }


}
