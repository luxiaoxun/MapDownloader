using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt
{
    //[StructLayout(LayoutKind.Sequential)]
    public class Placemark
    {
        public string Address{set;get;}
        public string Name { set; get; }
        public string CountryCode { set; get; }
        public string AdministrativeAreaName { set; get; }
        public string CityName { set; get; }
        public string ThoroughfareName { set; get; }
        public string PostalCodeNumber { set; get; }
        public string Tel { set; get; }
        public string Category { set; get; }
        public int Accuracy { set; get; }
        public RectLatLng LatLonBox { set; get; }
        public PointLatLng Point { set; get; }
        public string LocalityName { set; get; }
        public string CountryName { set; get; }
        public string DistrictName { set; get; }
        public string SubAdministrativeAreaName { set; get; }
        public string Neighborhood { set; get; }
        public string StreetNumber { set; get; }
        public string CountryNameCode{set;get;}
        public string HouseNo { set; get; }

        public Placemark()
        {
        }

        public Placemark(string address)
        {
            this.Address = address;
            this.Category = string.Empty;
            this.CityName = string.Empty;
            this.CountryCode = string.Empty;
            this.LatLonBox = new RectLatLng();
            this.Name = string.Empty;
            this.Tel = string.Empty;
            this.Point = new PointLatLng();
            this.Accuracy = 0;
            this.HouseNo = string.Empty;
            this.ThoroughfareName = string.Empty;
            this.DistrictName = string.Empty;
            this.LocalityName = string.Empty;
            this.PostalCodeNumber = string.Empty;
            this.CountryName = string.Empty;
            this.CountryNameCode = string.Empty;
            this.AdministrativeAreaName = string.Empty;
            this.SubAdministrativeAreaName = string.Empty;
            this.Neighborhood = string.Empty;
            this.StreetNumber = string.Empty;
        }

        public Placemark(Placemark oth)
        {
            this.Address = oth.Address;
            this.Category = oth.Category;
            this.CityName = oth.CityName;
            this.CountryCode = oth.CountryCode;
            this.LatLonBox = oth.LatLonBox;
            this.Name = oth.Name;
            this.Tel = oth.Tel;
            this.Point = oth.Point;
            this.Accuracy = oth.Accuracy;
            this.HouseNo = oth.HouseNo;
            this.ThoroughfareName = oth.ThoroughfareName;
            this.DistrictName = oth.DistrictName;
            this.LocalityName = oth.LocalityName;
            this.PostalCodeNumber = oth.PostalCodeNumber;
            this.CountryName = oth.CountryName;
            this.CountryNameCode = oth.CountryNameCode;
            this.AdministrativeAreaName = oth.AdministrativeAreaName;
            this.SubAdministrativeAreaName = oth.SubAdministrativeAreaName;
            this.Neighborhood = oth.Neighborhood;
            this.StreetNumber = oth.StreetNumber;
        }
    }
}
