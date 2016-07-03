using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu
{
    public class TiandituMapProvider : TiandituProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("FB3EFBB1-2709-41EA-8684-600DFD3E4E94");
        public static readonly TiandituMapProvider Instance;
        private readonly string name;

        // Methods
        static TiandituMapProvider()
        {
            Instance = new TiandituMapProvider();
        }

        public TiandituMapProvider()
        {
            this.name = "TiandituMapNoAnno3857";
            this.cnName = "天地图街道地图(无注记-球面墨卡托)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "vec_w";
                int serverIndex = GMapProvider.GetServerNum(pos, TiandituProviderBase.maxServer);
                //string url = string.Format(TiandituProviderBase.UrlFormat, new object[] { serverIndex, str, str.Substring(0, str.Length - 2), str.Substring(str.Length - 1), zoom, pos.Y, pos.X });
                string url = string.Format(TiandituProviderBase.UrlFormat, new object[] { serverIndex, str, pos.X, pos.Y, zoom });
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
