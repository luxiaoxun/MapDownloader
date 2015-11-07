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
                return "<SpatialReference xsi:type='typens:ProjectedCoordinateSystem'><WKT>PROJCS[&quot;WGS_1984_Web_Mercator_Auxiliary_Sphere&quot;,GEOGCS[&quot;GCS_WGS_1984&quot;,DATUM[&quot;D_WGS_1984&quot;,SPHEROID[&quot;WGS_1984&quot;,6378137.0,298.257223563]],PRIMEM[&quot;Greenwich&quot;,0.0],UNIT[&quot;Degree&quot;,0.0174532925199433]],PROJECTION[&quot;Mercator_Auxiliary_Sphere&quot;],PARAMETER[&quot;False_Easting&quot;,0.0],PARAMETER[&quot;False_Northing&quot;,0.0],PARAMETER[&quot;Central_Meridian&quot;,0.0],PARAMETER[&quot;Standard_Parallel_1&quot;,0.0],PARAMETER[&quot;Auxiliary_Sphere_Type&quot;,0.0],UNIT[&quot;Meter&quot;,1.0],AUTHORITY[&quot;EPSG&quot;,3857]]</WKT><XOrigin>-20037700</XOrigin><YOrigin>-30241100</YOrigin><XYScale>148923141.92838538</XYScale><ZOrigin>-100000</ZOrigin><ZScale>10000</ZScale><MOrigin>-100000</MOrigin><MScale>10000</MScale><XYTolerance>0.001</XYTolerance><ZTolerance>0.001</ZTolerance><MTolerance>0.001</MTolerance><HighPrecision>true</HighPrecision><WKID>3857</WKID></SpatialReference>";
            }
            if (epsgCode == 4326)
            {
                return "<SpatialReference xsi:type='typens:GeographicCoordinateSystem'><WKT>GEOGCS[&quot;GCS_WGS_1984&quot;,DATUM[&quot;D_WGS_1984&quot;,SPHEROID[&quot;WGS_1984&quot;,6378137.0,298.257223563]],PRIMEM[&quot;Greenwich&quot;,0.0],UNIT[&quot;Degree&quot;,0.0174532925199433],AUTHORITY[&quot;EPSG&quot;,4326]]</WKT><XOrigin>-399.99999999999989</XOrigin><YOrigin>-399.99999999999989</YOrigin><XYScale>11258999068426.24</XYScale><ZOrigin>-100000</ZOrigin><ZScale>10000</ZScale><MOrigin>-100000</MOrigin><MScale>10000</MScale><XYTolerance>8.9831528411952133e-009</XYTolerance><ZTolerance>0.001</ZTolerance><MTolerance>0.001</MTolerance><HighPrecision>true</HighPrecision><LeftLongitude>-180</LeftLongitude><WKID>4326</WKID></SpatialReference>";
            }
            return null;
        }
    }
}
