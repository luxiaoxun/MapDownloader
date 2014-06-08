using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GMapChinaRegion;
using NetUtilityLib;

namespace NetUtilityLibTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Country china = GetCountryDataFromFile(@"F:\GMap\china.xml");
            //for (int i = 0; i < china.Province.Count; ++i)
            //{
            //    string ps = china.Province[i].rings;
            //    china.Province[i].rings = GetPositionFromRings(ref ps, china.Province[i].name);
            //    for (int j = 0; j < china.Province[i].City.Count; ++j)
            //    {
            //        string cs = china.Province[i].City[j].rings;
            //        china.Province[i].City[j].rings = GetPositionFromRings(ref cs, china.Province[i].City[j].name);
            //        for (int k = 0; k < china.Province[i].City[j].Piecearea.Count; ++k)
            //        {
            //            string pps = china.Province[i].City[j].Piecearea[k].rings;
            //            china.Province[i].City[j].Piecearea[k].rings = GetPositionFromRings(ref pps,
            //                china.Province[i].City[j].Piecearea[k].name);
            //        }
            //    }
            //}

            //XmlHelper.XmlSerializeToFile(china, @"F:\GMap\china2.xml", Encoding.UTF8);
            //JsonHelper.JsonSerializeToFile(china, @"F:\GMap\china",Encoding.UTF8);
            
            Console.WriteLine("Complete!");
            Console.ReadKey();
        }

        private static Country GetCountryDataFromFile(string file)
        {
            Country country = XmlHelper.XmlDeserializeFromFile<Country>(file, Encoding.UTF8);
            return country;
        }

        private static string GetPositionFromRings(ref string rings,string name)
        {
            if (rings == null)
            {
                Console.WriteLine(name);
                return "";
            }
            string[] res = rings.Split(';');
            return res[0];
        }
    }
}
