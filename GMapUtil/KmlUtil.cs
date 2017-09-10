using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMapCommonType;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using Geometry = SharpKml.Dom.Geometry;
using Placemark = SharpKml.Dom.Placemark;

namespace GMapUtil
{
    public class KmlUtil
    {
        private static List<Point2D> CoordinatesToPoints(CoordinateCollection coordinates)
        {
            List<Point2D> list = new List<Point2D>();
            foreach (Vector vector in coordinates)
            {
                list.Add(new Point2D(vector.Longitude, vector.Latitude));
            }
            return list;
        }

        private static void FillPlacemarkWithGeometry(ref KmlPlaceMark placeMark, ref SharpKml.Dom.Geometry geo)
        {
            if (geo is SharpKml.Dom.Polygon)
            {
                SharpKml.Dom.Polygon polygon = geo as SharpKml.Dom.Polygon;
                if (polygon.OuterBoundary != null)
                {
                    placeMark.Geometry = new GMapCommonType.Polygon(CoordinatesToPoints(polygon.OuterBoundary.LinearRing.Coordinates));
                }
            }
            if (geo is SharpKml.Dom.Point)
            {
                SharpKml.Dom.Point point = geo as SharpKml.Dom.Point;
                placeMark.Geometry = new Point2D(point.Coordinate.Longitude, point.Coordinate.Latitude);
            }
            if (geo is SharpKml.Dom.LineString)
            {
                SharpKml.Dom.LineString str = geo as SharpKml.Dom.LineString;
                placeMark.Geometry = new Polyline(CoordinatesToPoints(str.Coordinates));
            }
        }

        private static string GetCompatibleXml(string xmlContent)
        {
            string str = xmlContent.Substring(0, (xmlContent.Length > 0x7d0) ? 0x7d0 : xmlContent.Length);
            string str3 = xmlContent.Substring(str.Length);
            if (str.Contains("<kml xmlns=\"http://earth.google.com/kml/2.3\">"))
            {
                str = str.Replace("<kml xmlns=\"http://earth.google.com/kml/2.3\">",
                    "<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
            }
            else if (str.Contains("<kml xmlns=\"http://earth.google.com/kml/2.2\">"))
            {
                str = str.Replace("<kml xmlns=\"http://earth.google.com/kml/2.2\">",
                    "<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
            }
            else if (str.Contains("<kml xmlns=\"http://earth.google.com/kml/2.1\">"))
            {
                str = str.Replace("<kml xmlns=\"http://earth.google.com/kml/2.1\">",
                    "<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
            }
            string str2 =
                "xsi:schemaLocation=\"http://www.opengis.net/kml/2.2 http://schemas.opengis.net/kml/2.2.0/ogckml22.xsd http://www.google.com/kml/ext/2.2 http://code.google.com/apis/kml/schema/kml22gx.xsd\"";
            if (str.Contains(str2))
            {
                str = str.Replace(str2, "");
            }
            return (str + str3);
        }

        public static KmlFile GetKmlEntryFromKmlFile(string kmlFile)
        {
            string compatibleXml = GetCompatibleXml(new StreamReader(kmlFile).ReadToEnd());
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(compatibleXml)))
            {
                return KmlFile.Load(stream);
            }
        }

        public static List<KmlPlaceMark> GetPlaceMarksFromKmlFile(string kmlFile)
        {
            string compatibleXml = GetCompatibleXml(new StreamReader(kmlFile).ReadToEnd());
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(compatibleXml)))
            {
                KmlFile file = KmlFile.Load(stream);
                List<KmlPlaceMark> list = new List<KmlPlaceMark>();
                foreach (Placemark placemark in file.Root.Flatten().OfType<Placemark>())
                {
                    try
                    {
                        Geometry geo = placemark.Geometry;
                        if (geo != null)
                        {
                            if (geo is MultipleGeometry)
                            {
                                MultipleGeometry geometry2 = geo as MultipleGeometry;
                                if (geometry2.Geometry != null)
                                {
                                    foreach (Geometry geometry3 in geometry2.Geometry)
                                    {
                                        KmlPlaceMark placeMark = new KmlPlaceMark
                                                                 {
                                                                     Name = placemark.Name,
                                                                     StyleUrl = placemark.StyleUrl.ToString()
                                                                 };
                                        if (placemark.Description != null)
                                        {
                                            placeMark.Description = placemark.Description.Text;
                                        }
                                        Geometry geometry4 = geometry3;
                                        FillPlacemarkWithGeometry(ref placeMark, ref geometry4);
                                        list.Add(placeMark);
                                    }
                                }
                            }
                            else
                            {
                                KmlPlaceMark mark2 = new KmlPlaceMark
                                                     {
                                                         Name = placemark.Name
                                                     };
                                if (placemark.Description != null)
                                {
                                    mark2.Description = placemark.Description.Text;
                                }
                                FillPlacemarkWithGeometry(ref mark2, ref geo);
                                list.Add(mark2);
                            }
                        }
                        continue;
                    }
                    catch
                    {
                        continue;
                    }
                }
                return list;
            }
        }

        public static List<List<PointLatLng>> GetPolygonsFromKMLFile(string kmlFile)
        {
            XmlDocument document = new XmlDocument();
            document.Load(kmlFile);
            List<List<PointLatLng>> list = new List<List<PointLatLng>>();
            XmlNodeList elementsByTagName = document.GetElementsByTagName("Polygon");
            for (int i = 0; i < elementsByTagName.Count; i++)
            {
                XmlNodeList list3 = (elementsByTagName.Item(i) as XmlElement).GetElementsByTagName("coordinates");
                for (int j = 0; j < list3.Count; j++)
                {
                    string[] strArray = list3.Item(j).InnerText.Replace("\n", "").Split(new char[] { ' ' });
                    List<PointLatLng> item = new List<PointLatLng>();
                    for (int k = 0; k < strArray.Length; k++)
                    {
                        if (!strArray[k].Trim().Equals(""))
                        {
                            string[] strArray2 = strArray[k].Split(new char[] { ',' });
                            double num4 = double.Parse(strArray2[0]);
                            double lat = double.Parse(strArray2[1]);
                            PointLatLng lng = new PointLatLng(lat, num4);
                            item.Add(lng);
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public static void SaveEntry(KmlFile kml, string name, string filePath)
        {
            kml.Save(filePath);
        }

        public static void SaveLineString(List<PointLatLng> points, string name, string filePath)
        {
            Placemark root = new Placemark
                             {
                                 Name = name
                             };
            LineString str = new LineString
                             {
                                 Coordinates = new CoordinateCollection()
                             };
            foreach (PointLatLng lng in points)
            {
                str.Coordinates.Add(new Vector(lng.Lat, lng.Lng));
            }
            root.Geometry = str;
            KmlFile.Create(root, false).Save(filePath);
        }

        public static void SavePoints(List<PointLatLng> points, string name, string filePath)
        {
            Kml root = new Kml();
            Document document = new Document();
            foreach (PointLatLng lng in points)
            {
                Placemark feature = new Placemark();
                Point point = new Point
                              {
                                  Coordinate = new Vector(lng.Lat, lng.Lng)
                              };
                feature.Geometry = point;
                document.AddFeature(feature);
            }
            root.Feature = document;
            KmlFile.Create(root, false).Save(filePath);
        }

        public static int SavePoints(List<PointLatLngWithProperty> points, string name, string filePath)
        {
            int num = 0;
            Kml root = new Kml();
            Document document = new Document();
            foreach (PointLatLngWithProperty property in points)
            {
                try
                {
                    if ((property.Point.Lat != 0.0) && (property.Point.Lng != 0.0))
                    {
                        Placemark feature = new Placemark();
                        Point point = new Point
                                      {
                                          Coordinate = new Vector(property.Point.Lat, property.Point.Lng)
                                      };
                        feature.Geometry = point;
                        feature.Description = new Description();
                        foreach (KeyValuePair<string, string> pair in property.Properties)
                        {
                            Description description = feature.Description;
                            description.Text = description.Text + string.Format("{0}:{1}\r\n", pair.Key, pair.Value);
                        }
                        document.AddFeature(feature);
                        num++;
                    }
                    continue;
                }
                catch
                {
                    continue;
                }
            }
            root.Feature = document;
            KmlFile.Create(root, false).Save(filePath);
            return num;
        }

        public static void SavePolygon(List<PointLatLng> points, string name, string filePath)
        {
            SharpKml.Dom.Placemark root = new SharpKml.Dom.Placemark
                             {
                                 Name = name
                             };
            SharpKml.Dom.Polygon polygon = new SharpKml.Dom.Polygon
                              {
                                  OuterBoundary = new OuterBoundary()
                              };
            polygon.OuterBoundary.LinearRing = new LinearRing();
            polygon.OuterBoundary.LinearRing.Coordinates = new CoordinateCollection();
            foreach (PointLatLng lng in points)
            {
                polygon.OuterBoundary.LinearRing.Coordinates.Add(new Vector(lng.Lat, lng.Lng));
            }
            root.Geometry = polygon;
            KmlFile.Create(root, false).Save(filePath);
        }
    }
}
