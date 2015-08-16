using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.TianDitu
{
    public class TiandituMapProviderWithAnno3857 : TiandituProviderBase3857
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("127f0a15-356f-4b0d-a41f-3dff713c5a36");
        public static readonly TiandituMapProviderWithAnno3857 Instance;
        private readonly string name;
        private GMapProvider[] overlays;

        // Methods
        static TiandituMapProviderWithAnno3857()
        {
            Instance = new TiandituMapProviderWithAnno3857();
        }

        public TiandituMapProviderWithAnno3857()
        {
            this.name = "TiandituMap3857";
            this.cnName = "天地图街道地图(球面墨卡托)";
        }

        protected override bool CheckTileImageHttpResponse(System.Net.WebResponse response)
        {
            return true;
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string str = "cva_w";
                string url = string.Format(TiandituProviderBase3857.UrlFormat, new object[] { GMapProvider.GetServerNum(pos, TiandituProviderBase3857.maxServer), str, str.Substring(0, str.Length - 2), str.Substring(str.Length - 1), zoom, pos.Y, pos.X });
                PureImage ret = base.GetTileImageUsingHttp(url);
                return ret;
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

        public override GMapProvider[] Overlays
        {
            get
            {
                if (this.overlays == null)
                {
                    this.overlays = new GMapProvider[] { TiandituMapProvider3857.Instance, this };
                }
                return this.overlays;
            }
        }
    }
}
