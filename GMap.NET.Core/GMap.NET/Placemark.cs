
namespace GMap.NET
{
   /// <summary>
   /// represents place info
   /// </summary>
   public struct Placemark
   {
       public static readonly Placemark Empty = new Placemark();

      /// <summary>
      /// the address
      /// </summary>
       public string Address;

      /// <summary>
      /// the accuracy of address
      /// </summary>
      public int Accuracy;

      // parsed values from address      
      public string ThoroughfareName;
      public string LocalityName;
      public string PostalCodeNumber;
      public string CountryName;
      public string AdministrativeAreaName;
      public string DistrictName;
      public string SubAdministrativeAreaName;
      public string Neighborhood;
      public string StreetNumber;

      public string CountryNameCode;
      public string HouseNo;

       //Added for Map
      public PointLatLng Point;
      public string Name;
      public string CountryCode;
      public string ProvinceName;
      public string CityName;
      public string Tel;
      public string Category;
      public RectLatLng LatLonBox;

      public Placemark(string address)
      {
         Address = address;

         Accuracy = 0;
         HouseNo = string.Empty;
         ThoroughfareName = string.Empty;
         DistrictName = string.Empty;
         LocalityName = string.Empty;
         PostalCodeNumber = string.Empty;
         CountryName = string.Empty;
         CountryNameCode = string.Empty;
         AdministrativeAreaName = string.Empty;
         SubAdministrativeAreaName = string.Empty;
         Neighborhood = string.Empty;
         StreetNumber = string.Empty;

         Point = PointLatLng.Empty;
         LatLonBox = RectLatLng.Empty;
         Name = string.Empty;
         CountryCode = string.Empty;
         ProvinceName = string.Empty;
         CityName = string.Empty;
         Tel = string.Empty;
         Category = string.Empty;
      }

      public Placemark(Placemark oth)
      {
          this.Address = oth.Address;
          this.Category = oth.Category;
          this.ProvinceName = oth.ProvinceName;
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
