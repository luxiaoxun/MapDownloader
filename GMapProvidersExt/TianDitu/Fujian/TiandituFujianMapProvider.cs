using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu.Fujian
{
    public class TiandituFujianMapProvider: TiandituFujianProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("06BE3B36-42BD-447C-A18E-CE85D9AB865F");
        public static readonly TiandituFujianMapProvider Instance;
        private readonly string name;

        // Methods
        static TiandituFujianMapProvider()
        {
            Instance = new TiandituFujianMapProvider();
        }

        public TiandituFujianMapProvider()
        {
            this.name = "TiandituFujianMapNoAnno";
            this.cnName = "天地图福建街道地图(无注记-WGS84)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "vec_fj";
                int serverIndex = GMapProvider.GetServerNum(pos, TiandituFujianProviderBase.maxServer);
                string url = string.Format(TiandituFujianProviderBase.UrlFormat, new object[] { str, zoom, pos.Y, pos.X, "png"});
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
