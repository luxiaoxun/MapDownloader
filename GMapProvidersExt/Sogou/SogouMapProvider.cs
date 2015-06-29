using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;

namespace GMapProvidersExt.Sogou
{
    public class SogouMapProvider : SogouMapProviderBase
    {
        // Fields
        private readonly string cnName;
        private readonly Guid id = new Guid("D0CEB471-F10A-4412-A2C1-DF617D6674A8");
        public static readonly SogouMapProvider Instance;
        private readonly string name;
        private readonly string[] sogouUrls;

        // Methods
        static SogouMapProvider()
        {
            Instance = new SogouMapProvider();
        }

        private SogouMapProvider()
        {
            this.sogouUrls = new string[] { "http://p0.go2map.com/seamless1/0/174/", "http://p1.go2map.com/seamless1/0/174/", "http://p2.go2map.com/seamless1/0/174/", "http://p3.go2map.com/seamless1/0/174/" };
            this.name = "SogouMap";
            this.cnName = "搜狗普通地图";
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = this.GetTileUrl(pos, zoom);
                return base.GetTileImageUsingHttp(url);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string GetTileUrl(GPoint pos, int zoom)
        {
            string str;
            string str2;
            long num = (long)Math.Pow(2.0, (double)(zoom - 1));
            long num2 = num - 1;
            long x = pos.X;
            long y = pos.Y;
            long xPos = x - num;
            long yPos = -y + num2;
            int num7 = 0x2d9 - zoom;
            if (num7 == 710)
            {
                num7 = 0x318;
            }
            long num9 = (long)Math.Floor((double)(((double)xPos) / 200.0));
            long num8 = (long)Math.Floor((double)(((double)yPos) / 200.0));
            if (num9 < 0)
            {
                str2 = "M" + -num9;
            }
            else
            {
                str2 = num9.ToString();
            }
            if (num8 < 0)
            {
                str = "M" + -num8;
            }
            else
            {
                str = num8.ToString();
            }
            string xPosStr = xPos.ToString().Replace("-", "M");
            string yPosStr = yPos.ToString().Replace("-", "M");
            int index = int.Parse(((x + y) % ((long)this.sogouUrls.Length)).ToString());
            return string.Concat(new object[] { this.sogouUrls[index], num7, "/", str2, "/", str, "/", xPosStr, "_", yPosStr, ".GIF" });
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
                return new GMapProvider[] { this };
            }
        }

        public override PureProjection Projection
        {
            get
            {
                return SphericalMercatorProjection.Instance;
            }
        }
    }


}
