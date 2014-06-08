using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GMapChinaRegion
{
    public class Piecearea
    {
        [XmlAttribute]
        public string code { set; get; }

        [XmlAttribute]
        public string name { set; get; }

        [XmlAttribute]
        public string rings { set; get; }
    }

    public class City
    {
        [XmlAttribute]
        public string code { set; get; }

        [XmlAttribute]
        public string name { set; get; }

        [XmlAttribute]
        public string rings { set; get; }

        [XmlElement]
        public List<Piecearea> Piecearea { set; get; }
    }

    public class Province
    {
        [XmlAttribute]
        public string ID { set; get; }

        [XmlAttribute]
        public string code { set; get; }

        [XmlAttribute]
        public string name { set; get; }

        [XmlAttribute]
        public string rings { set; get; }

        [XmlElement]
        public List<City> City { set; get; }
    }

    public class Country
    {
        [XmlAttribute]
        public string ID { set; get; }

        [XmlAttribute]
        public string code { set; get; }

        [XmlAttribute]
        public string name { set; get; }

        [XmlElement]
        public List<Province> Province { set; get; } 
    }
}
