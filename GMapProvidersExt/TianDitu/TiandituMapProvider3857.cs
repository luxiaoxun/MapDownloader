using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu
{
    public class TiandituMapProvider3857 : TiandituProviderBase3857
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("847D01A4-21ED-4302-8732-69C6849CDCE9");
        public static readonly TiandituMapProvider3857 Instance;
        private readonly string name;

        // Methods
        static TiandituMapProvider3857()
        {
            Instance = new TiandituMapProvider3857();
        }

        public TiandituMapProvider3857()
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
                string url = string.Format(TiandituProviderBase3857.UrlFormat, new object[] { GMapProvider.GetServerNum(pos, TiandituProviderBase3857.maxServer), str, str.Substring(0, str.Length - 2), str.Substring(str.Length - 1), zoom, pos.Y, pos.X });
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
