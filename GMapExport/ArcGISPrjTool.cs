using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMapExport
{
    public static class ArcGISPrjTool
    {
        // Methods
        public static string GetSpatialReferenceXML(int epsgCode)
        {
            if (epsgCode == 3857)
            {
                return "<SpatialReference xsi:type=\"typens:ProjectedCoordinateSystem\"><WKT>PROJCS[\"WGS_1984_Web_Mercator_Auxiliary_Sphere\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Mercator_Auxiliary_Sphere\"],PARAMETER[\"False_Easting\",0.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",0.0],PARAMETER[\"Standard_Parallel_1\",0.0],PARAMETER[\"Auxiliary_Sphere_Type\",0.0],UNIT[\"Meter\",1.0],AUTHORITY[\"EPSG\",3857]]</WKT><XOrigin>-20037700</XOrigin><YOrigin>-30241100</YOrigin><XYScale>148923141.92838538</XYScale><ZOrigin>-100000</ZOrigin><ZScale>10000</ZScale><MOrigin>-100000</MOrigin><MScale>10000</MScale><XYTolerance>0.001</XYTolerance><ZTolerance>0.001</ZTolerance><MTolerance>0.001</MTolerance><HighPrecision>true</HighPrecision><WKID>3857</WKID></SpatialReference>";
            }
            if (epsgCode == 4326)
            {
                return "<SpatialReference xsi:type=\"typens:GeographicCoordinateSystem\"><WKT>GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433],AUTHORITY[\"EPSG\",4326]]</WKT><XOrigin>-399.99999999999989</XOrigin><YOrigin>-399.99999999999989</YOrigin><XYScale>11258999068426.24</XYScale><ZOrigin>-100000</ZOrigin><ZScale>10000</ZScale><MOrigin>-100000</MOrigin><MScale>10000</MScale><XYTolerance>8.9831528411952133e-009</XYTolerance><ZTolerance>0.001</ZTolerance><MTolerance>0.001</MTolerance><HighPrecision>true</HighPrecision><LeftLongitude>-180</LeftLongitude><WKID>4326</WKID></SpatialReference>";
            }
            return null;
        }
    }
}
