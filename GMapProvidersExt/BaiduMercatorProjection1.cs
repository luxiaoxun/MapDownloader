using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt
{
    public class BaiduMercatorProjection1 : PureProjection
    {
        // Fields
        private static readonly double aelAsTkQaQ;
        private static readonly double double_2;
        private static readonly double double_3;
        private static readonly double double_4;
        private static double[] double_5;
        private static double[] double_6;
        private static double[] double_7;
        private static double[] double_8;
        private readonly GSize gsize_0;
        public static readonly BaiduMercatorProjection1 Instance;
        private int int_0;
        private int int_1;
        private static List<double[]> list_2;
        private static List<double[]> list_3;

        // Methods
        static BaiduMercatorProjection1()
        {
            //Class5.cO9iOAkzhndjY();
            Instance = new BaiduMercatorProjection1();
            double_2 = -74.0;
            double_3 = 74.0;
            aelAsTkQaQ = -180.0;
            double_4 = 180.0;
            double_5 = new double[] { 
            -0.0015702102444, 111320.7020616939, 1704480524535203, -10338987376042340, 26112667856603880, -3.51496691766537E+16, 26595700718403920, -10725012454188240, 1800819912950474, 82.5, 0.0008277824516172526, 111320.70204635779, 647795574.66716075, -4082003173.6413159, 10774905663.511419, -15171875531.515591, 
            12053065338.62167, -5124939663.5774717, 913311935.95120323, 67.5, 0.00337398766765, 111320.70202021619, 4481351.0458903648, -23393751.199316621, 79682215.471864551, -115964993.2797253, 97236711.156021446, -43661946.337528206, 8477230.5011352338, 52.5, 0.00220636496208, 111320.70202091279, 
            51751.861128411307, 3796837.7494702451, 992013.73977910134, -1221952.21711287, 1340652.697009075, -620943.69909843116, 144416.92938062409, 37.5, -0.00034419635043683922, 111320.7020576856, 278.23539807727519, 2485758.6900353939, 6070.7509632433776, 54821.183453521182, 9540.6066333042363, -2710.55326746645, 
            1405.483844121726, 22.5, -0.00032181358786131318, 111320.7020701615, 0.00369383431289, 823725.64027957176, 0.46104986909093, 2351.3431413312919, 1.58060784298199, 8.77738589078284, 0.37238884252424, 7.45
         };
            double_6 = new double[] { 75.0, 60.0, 45.0, 30.0, 15.0, 0.0 };
            double_7 = new double[] { 12890594.86, 8362377.87, 5591021.0, 3481989.83, 1678043.12, 0.0 };
            double_8 = new double[] { 
            1.4105261721162549E-08, 8.98305509648872E-06, -1.9939833816331, 200.98243831067961, -187.2403703815547, 91.6087516669843, -23.38765649603339, 2.57121317296198, -0.03801003308653, 17337981.2, -7.4358563895655369E-09, 8.9830550977262389E-06, -0.78625201886289, 96.326875997598464, -1.85204757529826, -59.369359054858769, 
            47.400335492967372, -16.50741931063887, 2.28786674699375, 10260144.86, -3.0308834608988258E-08, 8.98305509983578E-06, 0.30071316287616, 59.742936184422767, 7.357984074871, -25.383710026647449, 13.45380521110908, -3.29883767235584, 0.32710905363475, 6856817.37, -1.9819813049305521E-08, 8.9830550997795349E-06, 
            0.03278182852591, 40.316785277057441, 0.65659298677277, -4.44255534477492, 0.85341911805263, 0.12923347998204, -0.04625736007561, 4482777.06, 3.09191371068437E-09, 8.9830550968121549E-06, 6.995724062E-05, 23.109343041449009, -0.00023663490511, -0.6321817810242, -0.00663494467273, 0.03430082397953, 
            -0.00466043876332, 2555164.4, 2.890871144776878E-09, 8.9830550958054071E-06, -3.068298E-08, 7.47137025468032, -3.53937994E-06, -0.02145144861037, -1.234426596E-05, 0.00010322952773, -3.23890364E-06, 826088.5
         };
            list_2 = new List<double[]>();
            list_3 = new List<double[]>();
            for (int i = 0; i < 6; i++)
            {
                double[] item = new double[10];
                double[] numArray2 = new double[10];
                for (int j = 0; j < 10; j++)
                {
                    item[j] = double_5[(i * 10) + j];
                    numArray2[j] = double_8[(i * 10) + j];
                }
                list_2.Add(item);
                list_3.Add(numArray2);
            }
        }

        public BaiduMercatorProjection1()
        {
            //Class5.cO9iOAkzhndjY();
            this.gsize_0 = new GSize(0x100, 0x100);
            this.int_0 = 0x12;
            this.int_1 = 0x100;
        }

        public override GPoint FromLatLngToPixel(double lat, double lng, int zoom)
        {
            double num;
            double num2;
            PointLatLng lng2 = new PointLatLng(lat, lng);
            this.method_1(lng2, out num, out num2);
            return new GPoint((long)(num / Math.Pow(2.0, (double)(this.int_0 - zoom))), (long)(num2 / Math.Pow(2.0, (double)(this.int_0 - zoom))));
        }

        public override PointLatLng FromPixelToLatLng(long x, long y, int zoom)
        {
            PointLatLng lng;
            this.method_2(x * Math.Pow(2.0, (double)(this.int_0 - zoom)), y * Math.Pow(2.0, (double)(this.int_0 - zoom)), out lng);
            return lng;
        }

        public int GetBounds(PointLatLng center, int width, int height, int zoom)
        {
            return 0;
        }

        public override GSize GetTileMatrixMaxXY(int zoom)
        {
            long num = ((int)1) << zoom;
            return new GSize(num - 1, num - 1);
        }

        public override GSize GetTileMatrixMinXY(int zoom)
        {
            return new GSize(0, 0);
        }

        private GPoint method_0(GPoint gpoint_0, int int_2)
        {
            int num = this.int_1 * ((int)Math.Pow(2.0, (double)(this.int_0 - int_2)));
            long num2 = gpoint_0.X * num;
            long num3 = gpoint_0.Y * num;
            double num4 = this.int_1 * Math.Pow(2.0, (double)(this.int_0 - int_2));
            double num5 = (((double)num2) / num4) - 0;
            double num6 = (((double)num3) / num4) - 0;
            return new GPoint((long)num5, (long)num6);
        }

        private void method_1(PointLatLng pointLatLng_0, out double double_9, out double double_10)
        {
            double[] numArray = null;
            for (int i = 0; i < double_6.Length; i++)
            {
                if (pointLatLng_0.Lat >= double_6[i])
                {
                    numArray = list_2[i];
                    break;
                }
            }
            if (numArray == null)
            {
                for (int j = double_6.Length - 1; 0 <= j; j--)
                {
                    if (pointLatLng_0.Lat <= -double_6[j])
                    {
                        numArray = list_2[j];
                        break;
                    }
                }
            }
            double num4 = numArray[0] + (numArray[1] * Math.Abs(pointLatLng_0.Lng));
            double num3 = Math.Abs(pointLatLng_0.Lat) / numArray[9];
            num3 = (((((numArray[2] + (numArray[3] * num3)) + ((numArray[4] * num3) * num3)) + (((numArray[5] * num3) * num3) * num3)) + ((((numArray[6] * num3) * num3) * num3) * num3)) + (((((numArray[7] * num3) * num3) * num3) * num3) * num3)) + ((((((numArray[8] * num3) * num3) * num3) * num3) * num3) * num3);
            num4 *= (0.0 > pointLatLng_0.Lng) ? ((double)(-1)) : ((double)1);
            num3 *= (0.0 > pointLatLng_0.Lat) ? ((double)(-1)) : ((double)1);
            double_9 = num4;
            double_10 = num3;
        }

        private void method_2(double double_9, double double_10, out PointLatLng pointLatLng_0)
        {
            double[] numArray = null;
            for (int i = 0; i < double_7.Length; i++)
            {
                if (Math.Abs(double_10) >= double_7[i])
                {
                    numArray = list_3[i];
                    break;
                }
            }
            double lng = numArray[0] + (numArray[1] * Math.Abs(double_9));
            double lat = Math.Abs(double_10) / numArray[9];
            lat = (((((numArray[2] + (numArray[3] * lat)) + ((numArray[4] * lat) * lat)) + (((numArray[5] * lat) * lat) * lat)) + ((((numArray[6] * lat) * lat) * lat) * lat)) + (((((numArray[7] * lat) * lat) * lat) * lat) * lat)) + ((((((numArray[8] * lat) * lat) * lat) * lat) * lat) * lat);
            lng *= (0.0 > double_9) ? ((double)(-1)) : ((double)1);
            lat *= (0.0 > double_10) ? ((double)(-1)) : ((double)1);
            pointLatLng_0 = new PointLatLng(lat, lng);
        }

        // Properties
        public override double Axis
        {
            get
            {
                return 6378137.0;
            }
        }

        public override RectLatLng Bounds
        {
            get
            {
                return RectLatLng.FromLTRB(aelAsTkQaQ, double_3, double_4, double_2);
            }
        }

        public override double Flattening
        {
            get
            {
                return 0.0033528106647474805;
            }
        }

        public override GSize TileSize
        {
            get
            {
                return this.gsize_0;
            }
        }
    }
}
