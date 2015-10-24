using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.Internals;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Baidu
{
    public abstract class BaiduMapProviderBaseJS : GMapProvider
    {
        private string ClientKey = "1308e84a0e8a1fc2115263a4b3cf87f1";

        public BaiduMapProviderBaseJS()
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
            get { return BaiduProjectionJS.Instance; }
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
}
