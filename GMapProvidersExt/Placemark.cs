using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using GMap.NET;

namespace GMapProvidersExt
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Placemark
    {
        private string address;
        public string Name;
        public string CountryCode;
        public string AdministrativeAreaName;
        public string CityName;
        public string ThoroughfareName;
        public string PostalCodeNumber;
        public string Tel;
        public string Category;
        public int Accuracy;
        public RectLatLng LatLonBox;
        public PointLatLng Point;
        public string LocalityName;
        public string CountryName;
        public string DistrictName;
        public string SubAdministrativeAreaName;
        public string Neighborhood;
        public string StreetNumber;
        public string CountryNameCode;
        public string HouseNo;

        public string Address
        {
            get
            {
                return this.address;
            }
            internal set
            {
                this.address = value;
            }
        }

        public Placemark(string address)
        {
            this.address = address;
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
    }
}
