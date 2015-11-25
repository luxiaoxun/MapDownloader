using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;

namespace GMapUtil
{
    /// <summary>
    /// Helper class with static methods
    /// </summary>
    public static class GMapUtils
    {
        //Radius of the Earth
        internal static readonly double earthRadius = 6371d;

        /// <summary>
        /// Given a start point, initial bearing, and distance in km, this will calculate the destination point travelling along a (shortest distance) great circle arc.
        /// See: http://www.movable-type.co.uk/scripts/latlong.html 
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="bearing">The bearing in decimal degrees.</param>
        /// <param name="distance">The distance in km</param>
        /// <returns></returns>
        public static PointLatLng GetPointLatLngFromStartPoint(PointLatLng startPoint, double bearing, double distance)
        {
            // convert distance to angular distance in radians
            distance = distance / earthRadius;
            // Convert bearing to Radians
            bearing = ToRadians(bearing);

            //Convert Degrees of startpoint to radians
            double lat1 = ToRadians(startPoint.Lat);
            double lon1 = ToRadians(startPoint.Lng);

            double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(distance) + Math.Cos(lat1) * Math.Sin(distance) * Math.Cos(bearing));
            double lon2 = lon1 + Math.Atan2(Math.Sin(bearing) * Math.Sin(distance) * Math.Cos(lat1), Math.Cos(distance) - Math.Sin(lat1) * Math.Sin(lat2));
            //normalise to -180..+180º
            lon2 = (lon2 + 3 * Math.PI) % (2 * Math.PI) - Math.PI;

            return new PointLatLng(ToDegrees(lat2), ToDegrees(lon2));
        }

        /// <summary>
        /// Gets the distance (in km) from Point A to Point B (both A and B must lie on a polyline defined by a list of points).
        /// If B is before A, the distance is negative!
        /// </summary>
        /// <param name="A">First point</param>
        /// <param name="B">Second point</param>
        /// <param name="points">List of Points that define the polyline</param>
        /// <returns></returns>
        public static double GetDistanceOnPolyline(PointLatLng A, PointLatLng B, List<PointLatLng> points)
        {
            double dist = 0d;

            //If both points are the same return 0!
            if (A == B)
                return dist;

            bool foundA = false;
            bool foundB = false;
            double distP1toA, distP2toA, distP1toB, distP2toB, distP1toP2;

            for (int i = 0; i < points.Count - 1; i++)
            {
                var p1 = points[i];
                var p2 = points[i + 1];

                bool AisBetween = GMapUtils.IsPointOnLineSegment(p1, p2, A, out distP1toA, out distP2toA, out distP1toP2);
                if (AisBetween)
                    foundA = true;

                bool BisBetween = GMapUtils.IsPointOnLineSegment(p1, p2, B, out distP1toB, out distP2toB, out distP1toP2);
                if (BisBetween)
                    foundB = true;

                if (AisBetween && BisBetween)
                {
                    dist = GMapProviders.EmptyProvider.Projection.GetDistance(A, B);
                    if (distP1toB < distP1toA)
                        dist *= -1d;
                    return dist;
                }

                //Point A was found, but not B:
                if (AisBetween && !foundB)
                    dist += distP2toA;
                else if (foundA && !AisBetween && !foundB)
                    dist += distP1toP2;
                else if (foundA && !AisBetween && BisBetween)
                {
                    dist += distP1toB;
                    break;
                }
                //Point B was found, but not A:
                else if (BisBetween && !foundA)
                    dist += distP2toB;
                else if (foundB && !BisBetween && !foundA)
                    dist += distP1toP2;
                else if (foundB && !BisBetween && AisBetween)
                {
                    dist += distP1toA;
                    dist *= -1d;
                    break;
                }
            }
            if (!foundA || !foundB)
                throw new Exception("Point A or Point B (or both) are not on the polyline!");

            return dist;
        }

        /// <summary>
        /// Returns the point that lies on the polyline and is closest to a given point.
        /// </summary>
        /// <param name="points">A List of Vectors/Points that define the polyline</param>
        /// <param name="pos">The given point</param>
        /// <returns>Closest point that is on the polyline.</returns>
        public static PointLatLng GetClosestPointOnPolyline(List<PointLatLng> points, PointLatLng pos)
        {
            SortedDictionary<double, Vector> distances = new SortedDictionary<double, Vector>();
            Vector v0 = new Vector(pos.Lng, pos.Lat);
            for (int i = 0; i < points.Count - 1; i++)
            {
                double dist;
                Vector v1 = new Vector(points[i].Lng, points[i].Lat);
                Vector v2 = new Vector(points[i + 1].Lng, points[i + 1].Lat);
                var point = GMapUtils.GetClosestPointOnLinesegment(v1, v2, v0, out dist);
                if (!distances.ContainsKey(dist))
                    distances.Add(dist, point);
            }
            return new PointLatLng(distances.First().Value.Y, distances.First().Value.X);
        }

        /// <summary>
        /// Checks if the given point X is on a given Polyline defined by a List PointlatLngs
        /// </summary>
        /// <param name="points">A List of PointLatLng that consists of all points of the polyline</param>
        /// <param name="X">The PointLatLng to check.</param>
        /// <returns>True if the PointlatLng X is on the Polyline, false otherwise</returns>
        public static bool IsPointOnPolyline(List<PointLatLng> points, PointLatLng X)
        {
            bool onPolyline = false;
            for (int i = 0; i < points.Count - 1; i++)
            {
                PointLatLng A = points[i];
                PointLatLng B = points[i + 1];
                if (IsPointOnLineSegment(A, B, X))
                {
                    onPolyline = true;
                    break;
                }
            }
            return onPolyline;
        }

        /// <summary>
        /// Returns a point that lies on the polyline defined by the point list, and is a certain distance (in km!) from point A (that must also lie on the polyline).
        /// If the point would lie outside of the polyline, the last point of the polyline is returned!
        /// If the distance is negative, the new point lies BEFORE the given point!
        /// </summary>
        /// <param name="A">The Start point</param>
        /// <param name="dist">The distance from the start point in km</param>
        /// <param name="points">The points defining the polyline</param>
        /// <returns></returns>
        public static PointLatLng GetDistantPointOnPolyline(PointLatLng A, double distance, List<PointLatLng> points)
        {
            if (distance == 0d)
                return A;

            PointLatLng newPoint = new PointLatLng();
            double distP1toA, distP2toA, distP1toP2;
            double fooDist = 0d, lastDist = 0d, newDist = 0d;

            if (distance > 0d)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var p1 = points[i];
                    var p2 = points[i + 1];

                    if (GMapUtils.IsPointOnLineSegment(p1, p2, A, out distP1toA, out distP2toA, out distP1toP2))
                    {
                        fooDist = distP2toA;
                        while (i < points.Count - 2 && fooDist < distance)
                        {
                            i++;
                            p1 = points[i];
                            p2 = points[i + 1];
                            lastDist = GMapProviders.EmptyProvider.Projection.GetDistance(p1, p2);
                            fooDist += lastDist;
                        }

                        if (fooDist < distance)
                            return points.Last();

                        double bearing = GMapProviders.EmptyProvider.Projection.GetBearing(p1, p2);

                        //Case 1: Last Distance is greater than 0 (the while loop ran at least one time)
                        if (lastDist > 0d)
                        {
                            newDist = lastDist - (fooDist - distance);
                            newPoint = GMapUtils.GetClosestPointOnPolyline(points, GMapUtils.GetPointLatLngFromStartPoint(p1, bearing, newDist));
                        }
                        //Case 2: Last Distance is 0, the while loop was not called:
                        else
                        {
                            newPoint = GMapUtils.GetClosestPointOnPolyline(points, GMapUtils.GetPointLatLngFromStartPoint(A, bearing, distance));
                        }
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var p1 = points[i];
                    var p2 = points[i + 1];

                    if (GMapUtils.IsPointOnLineSegment(p1, p2, A, out distP1toA, out distP2toA, out distP1toP2))
                    {
                        fooDist = distP1toA;
                        while (i > 0 && fooDist < Math.Abs(distance))
                        {
                            i--;
                            p1 = points[i];
                            p2 = points[i + 1];
                            lastDist = GMapProviders.EmptyProvider.Projection.GetDistance(p1, p2);
                            fooDist += lastDist;
                        }

                        if (fooDist < Math.Abs(distance))
                            return points.First();

                        double bearing = GMapProviders.EmptyProvider.Projection.GetBearing(p2, p1);

                        //Case 1: Last Distance is greater than 0 (the while loop ran at least one time)
                        if (lastDist > 0d)
                        {
                            newDist = lastDist - (fooDist - Math.Abs(distance));
                            newPoint = GMapUtils.GetClosestPointOnPolyline(points, GMapUtils.GetPointLatLngFromStartPoint(p2, bearing, newDist));
                        }
                        //Case 2: Last Distance is 0, the while loop was not called:
                        else
                            newPoint = GMapUtils.GetClosestPointOnPolyline(points, GMapUtils.GetPointLatLngFromStartPoint(A, bearing, Math.Abs(distance)));

                        break;
                    }
                }
            }
            return newPoint;
        }

        /// <summary>
        /// Gets a List with all intersection points for two polygons
        /// </summary>
        /// <param name="poly1">The first polygon</param>
        /// <param name="poly2">The second polygon</param>
        /// <returns>List of intersection points</returns>
        public static List<PointLatLng> GetIntersectionPoints(GMapPolygon poly1, GMapPolygon poly2)
        {
            return GetIntersectionPoints(poly1.Points, poly2.Points, true, true);
        }

        /// <summary>
        /// Gets a List with all intersection points for a route and a polygon
        /// </summary>
        /// <param name="route">The route</param>
        /// <param name="poly">The polygon</param>
        /// <returns>List of intersection points</returns>
        public static List<PointLatLng> GetIntersectionPoints(GMapRoute route, GMapPolygon poly)
        {
            return GetIntersectionPoints(route.Points, poly.Points, false, true);
        }

        /// <summary>
        /// Gets a List with all intersection points for two routes
        /// </summary>
        /// <param name="route1">The first route</param>
        /// <param name="route2">The second route</param>
        /// <returns>List of intersection points</returns>
        public static List<PointLatLng> GetIntersectionPoints(GMapRoute route1, GMapRoute route2)
        {
            return GetIntersectionPoints(route1.Points, route2.Points, false, false);
        }

        /// <summary>
        /// Gets a list with all intersection points for two given Polylines
        /// </summary>
        /// <param name="points1">List of PointLatLng that define the first polyline.</param>
        /// <param name="points2">List of PointLatLng that define the second polyline.</param>
        /// <returns>List of intersection points</returns>
        public static List<PointLatLng> GetIntersectionPoints(List<PointLatLng> points1, List<PointLatLng> points2, bool points1IsPolygon, bool points2IsPolygon)
        {
            List<PointLatLng> retList = new List<PointLatLng>();

            //Check EVERY LineSegment with EVERY other segment!
            for (int i = 0; i < points1.Count - 1; i++)
            {
                PointLatLng p1Start = points1[i];
                PointLatLng p1End = points1[i + 1];

                for (int j = 0; j < points2.Count - 1; j++)
                {
                    PointLatLng p2Start = points2[j];
                    PointLatLng p2End = points2[j + 1];

                    PointLatLng intersection = GetLineSegmentIntersection(p1Start, p1End, p2Start, p2End);
                    if (false == intersection.IsEmpty)
                    {
                        retList.Add(intersection);
                    }
                }
                //For polygons, check last linesegment between last and first point as well!
                if (points2IsPolygon)
                {
                    PointLatLng p2Start = points2[points2.Count - 1];
                    PointLatLng p2End = points2[0];

                    PointLatLng intersection = GetLineSegmentIntersection(p1Start, p1End, p2Start, p2End);
                    if (false == intersection.IsEmpty)
                    {
                        retList.Add(intersection);
                    }
                }
            }

            if (points1IsPolygon)
            {
                PointLatLng p1Start = points1[points1.Count - 1];
                PointLatLng p1End = points1[0];

                for (int j = 0; j < points2.Count - 1; j++)
                {
                    PointLatLng p2Start = points2[j];
                    PointLatLng p2End = points2[j + 1];

                    PointLatLng intersection = GetLineSegmentIntersection(p1Start, p1End, p2Start, p2End);
                    if (false == intersection.IsEmpty)
                    {
                        retList.Add(intersection);
                    }
                }
                //For polygons, check last linesegment between last and first point as well!
                if (points2IsPolygon)
                {
                    PointLatLng p2Start = points2[points2.Count - 1];
                    PointLatLng p2End = points2[0];

                    PointLatLng intersection = GetLineSegmentIntersection(p1Start, p1End, p2Start, p2End);
                    if (false == intersection.IsEmpty)
                    {
                        retList.Add(intersection);
                    }
                }
            }
            return retList;
        }

        /// <summary>
        /// Get the intersection between 2 line Segments with given Start and Endpoints
        /// </summary>
        /// <param name="start1">The start point of linesegment 1</param>
        /// <param name="end1">The end point of linesegment 1</param>
        /// <param name="start2">The start point of linesegment 2</param>
        /// <param name="end2">The end point of linesegment 2</param>
        /// <returns>Intersectionpoint or Empty Point if there is no intersection</returns>
        public static PointLatLng GetLineSegmentIntersection(PointLatLng start1, PointLatLng end1, PointLatLng start2, PointLatLng end2)
        {
            double bearing1 = GMapProviders.EmptyProvider.Projection.GetBearing(start1, end1);
            double bearing2 = GMapProviders.EmptyProvider.Projection.GetBearing(start2, end2);

            return GetLineSegmentIntersection(start1, bearing1, start2, bearing2, end1, end2);
        }

        /// <summary>
        /// Returns the point of intersection of two paths defined by point and bearing
        /// </summary>
        /// <see cref="http://williams.best.vwh.net/avform.htm#Intersection"/>
        /// <param name="start1">First point</param>
        /// <param name="brng1">Initial bearing from first point</param>
        /// <param name="start2">Second point</param>
        /// <param name="brng2">Initial bearing from second point</param>
        /// <returns>Destination point (PointLatLng.Empty if no unique intersection defined)</returns>
        public static PointLatLng GetLineSegmentIntersection(PointLatLng start1, double brng1, PointLatLng start2, double brng2, PointLatLng end1, PointLatLng end2)
        {
            double lat1 = ToRadians(start1.Lat);
            double lon1 = ToRadians(start1.Lng);
            double lat2 = ToRadians(start2.Lat);
            double lon2 = ToRadians(start2.Lng);

            //Check for special case:
            if (start1.Lng == start2.Lng)
            {
                lon2 = ToRadians(start2.Lng + 0.0000001d);
            }
            if (start1.Lat == start2.Lat)
            {
                lat2 = ToRadians(start2.Lat + 0.0000001d);
            }

            double brng13 = ToRadians(brng1);
            double brng23 = ToRadians(brng2);

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            double dist12 = 2d * Math.Asin(Math.Sqrt(Math.Sin(dLat / 2d) * Math.Sin(dLat / 2d) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Sin(dLon / 2d) * Math.Sin(dLon / 2d)));
            if (dist12 == 0d)
            {
                return PointLatLng.Empty;
            }

            // initial/final bearings between points
            double brngA = Math.Acos((Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(dist12)) / (Math.Sin(dist12) * Math.Cos(lat1)));
            if (Double.IsNaN(brngA))
            {
                // protect against rounding
                brngA = 0d;
            }
            double brngB = Math.Acos((Math.Sin(lat1) - Math.Sin(lat2) * Math.Cos(dist12)) / (Math.Sin(dist12) * Math.Cos(lat2)));

            double brng12, brng21;
            if (Math.Sin(lon2 - lon1) > 0d)
            {
                brng12 = brngA;
                brng21 = 2d * Math.PI - brngB;
            }
            else
            {
                brng12 = 2d * Math.PI - brngA;
                brng21 = brngB;
            }

            double alpha1 = (brng13 - brng12 + Math.PI) % (2d * Math.PI) - Math.PI;  // angle 2-1-3
            double alpha2 = (brng21 - brng23 + Math.PI) % (2d * Math.PI) - Math.PI;  // angle 1-2-3

            if (Math.Sin(alpha1) == 0d && Math.Sin(alpha2) == 0d)
            {
                //Infinite intersections
                return PointLatLng.Empty;
            }
            if (Math.Sin(alpha1) * Math.Sin(alpha2) < 0d)
            {
                //Ambiguous intersection
                return PointLatLng.Empty;
            }

            double alpha3 = Math.Acos(-Math.Cos(alpha1) * Math.Cos(alpha2) + Math.Sin(alpha1) * Math.Sin(alpha2) * Math.Cos(dist12));
            double dist13 = Math.Atan2(Math.Sin(dist12) * Math.Sin(alpha1) * Math.Sin(alpha2), Math.Cos(alpha2) + Math.Cos(alpha1) * Math.Cos(alpha3));
            double lat3 = Math.Asin(Math.Sin(lat1) * Math.Cos(dist13) + Math.Cos(lat1) * Math.Sin(dist13) * Math.Cos(brng13));
            double dLon13 = Math.Atan2(Math.Sin(brng13) * Math.Sin(dist13) * Math.Cos(lat1), Math.Cos(dist13) - Math.Sin(lat1) * Math.Sin(lat3));
            double lon3 = lon1 + dLon13;

            //Normalise to -180...+180°
            lon3 = (lon3 + 3d * Math.PI) % (2d * Math.PI) - Math.PI;

            PointLatLng intersect = new PointLatLng(ToDegrees(lat3), ToDegrees(lon3));

            //Check if intersection point is inside the bounding box of BOTH rectangles defined by start and end points!
            if (IsPointInBoundingBox(start1, end1, intersect) && IsPointInBoundingBox(start2, end2, intersect))
            {
                return intersect;
            }
            else
            {
                return PointLatLng.Empty;
            }
        }

        /// <summary>
        /// p1 and p2 represent two coordinates that make up the bounding box
        /// pX is a point that we are checking to see if it is inside the box
        /// </summary>
        /// <param name="p1">PointLatLng 1</param>
        /// <param name="p2">PointLatLng 1</param>
        /// <param name="pX">PointLatLng to check</param>
        /// <returns>True if pX is inside the bounding box, false otherwise</returns>
        private static bool IsPointInBoundingBox(PointLatLng p1, PointLatLng p2, PointLatLng pX)
        {
            bool betweenLats;
            bool betweenLons;

            if (p1.Lat < p2.Lat)
            {
                betweenLats = (p1.Lat <= pX.Lat && p2.Lat >= pX.Lat);
            }
            else
            {
                betweenLats = (p1.Lat >= pX.Lat && p2.Lat <= pX.Lat);
            }

            if (p1.Lng < p2.Lng)
            {
                betweenLons = (p1.Lng <= pX.Lng && p2.Lng >= pX.Lng);
            }
            else
            {
                betweenLons = (p1.Lng >= pX.Lng && p2.Lng <= pX.Lng);
            }

            return (betweenLats && betweenLons);
        }

        /// <summary>
        /// Convert given decimal degrees to radians
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        internal static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180d);
        }

        /// <summary>
        /// Convert given radians to decimal degrees
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        internal static double ToDegrees(double radians)
        {
            return radians * (180d / Math.PI);
        }

        /// <summary>
        /// Checks if the given point X is on a linesegment between A and B and calculates the distances
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        internal static bool IsPointOnLineSegment(PointLatLng A, PointLatLng B, PointLatLng X, out double distAX, out double distBX, out double distAB)
        {
            if (X == A)
            {
                distAX = 0;
                distBX = GMapProviders.EmptyProvider.Projection.GetDistance(B, X);
                distAB = distBX;
                return true;
            }
            else if (X == B)
            {
                distBX = 0;
                distAX = GMapProviders.EmptyProvider.Projection.GetDistance(A, X);
                distAB = distAX;
                return true;
            }
            distAX = GMapProviders.EmptyProvider.Projection.GetDistance(A, X);
            distBX = GMapProviders.EmptyProvider.Projection.GetDistance(B, X);
            distAB = GMapProviders.EmptyProvider.Projection.GetDistance(A, B);
            //if abs(ac + bc - ab) < EPSILON the point X must be ON the line segment between points A and B
            double checkVal = Math.Abs(distAX + distBX - distAB);
            return (checkVal < 0.0000001d);
        }

        /// <summary>
        /// Returns the closest Point ON a line segment defined by Points A and B to a given point P, and gets the distance to this point
        /// </summary>
        /// <param name="A">The Startpoint of the linesegment</param>
        /// <param name="B">The Endpoint of the linesegment</param>
        /// <param name="P">The given point.</param>
        /// <param name="distance">The distance from the line segment to P</param>
        /// <returns>Vector (Point) that lies on the linesegement and is closest to P.</returns>
        internal static Vector GetClosestPointOnLinesegment(Vector A, Vector B, Vector P, out double distance)
        {
            Vector AP = P - A;
            Vector AB = B - A;
            double ab2 = AB.LengthSquared;
            double ap_ab = Vector.Multiply(AP, AB);
            double t = ap_ab / ab2;

            if (t < 0.0f)
                t = 0.0f;
            else if (t > 1.0f)
                t = 1.0f;
            Vector closestPoint = A + AB * t;

            //Don't use this Length with Lat/Long coordinates!
            //distance = (closestPoint - P).Length;
            distance = GMapProviders.EmptyProvider.Projection.GetDistance(new GMap.NET.PointLatLng(P.Y, P.X), new GMap.NET.PointLatLng(closestPoint.Y, closestPoint.X));
            return closestPoint;
        }

        /// <summary>
        /// Checks if the given point X is on a linesegment between A and B
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        internal static bool IsPointOnLineSegment(PointLatLng A, PointLatLng B, PointLatLng X)
        {
            if (X == A || X == B)
                return true;
            double distAX = GMapProviders.EmptyProvider.Projection.GetDistance(A, X);
            double distBX = GMapProviders.EmptyProvider.Projection.GetDistance(B, X);
            double distAB = GMapProviders.EmptyProvider.Projection.GetDistance(A, B);
            //if abs(ac + bc - ab) < EPSILON the point X must be ON the line segment between points A and B
            double checkVal = Math.Abs(distAX + distBX - distAB);
            //Debug.WriteLine("Distance for IsPointOnLineSegment:  " + checkVal);
            return (checkVal < 0.0000001d);
        }

        /// <summary>
        /// Gets a list of Points along a great circle of the earth
        /// </summary>
        /// <param name="start">The Start point</param>
        /// <param name="end">The end point</param>
        /// <param name="numPointsBetween">The number of points along the great circle route</param>
        /// <returns></returns>
        public static List<PointLatLng> GetGreatCircleRoute(PointLatLng start, PointLatLng end, int numPointsBetween)
        {
            ///Check numPointsBetween
            if (numPointsBetween <= 1)
            {
                throw new ArgumentException("numPointsBetween must be larger than 1!");
            }
            //Create List and add first point:
            List<PointLatLng> lstPoints = new List<PointLatLng>(numPointsBetween + 2) { start };

            double lat1 = ToRadians(start.Lat);
            double lon1 = ToRadians(start.Lng);
            double lat2 = ToRadians(end.Lat);
            double lon2 = ToRadians(end.Lng);

            double d = GetDistanceInRadians(lat1, lon1, lat2, lon2);

            double increment = 1d / (double)numPointsBetween;
            double f = 0d + increment;

            for (int i = 0; i < numPointsBetween; i++)
            {
                lstPoints.Add(GetGreatCirclePoint(f, d, lat1, lon1, lat2, lon2));

                f += increment;
            }

            //Add last point
            lstPoints.Add(end);

            return lstPoints;
        }

        /// <summary>
        /// Returns a point that lies on the Great Circle described by Start and End points at a fraction f of distance d
        /// </summary>
        /// <see cref="http://williams.best.vwh.net/avform.htm#Intermediate"/>
        /// <param name="f">Fraction f (0.0 = Startpoint, 1.0 = Endpoint)</param>
        /// <param name="d">Distance between Start and Endpoints in RADIANS</param>
        /// <param name="lat1">Latitude of Startpoint IN RADIANS</param>
        /// <param name="lon1">Longitude of Startpoint IN RADIANS</param>
        /// <param name="lat2">Latitude of Endpoint IN RADIANS</param>
        /// <param name="lon2">Longitude of Endpoint IN RADIANS</param>
        /// <returns>PointlatLng that lies on the great Circle</returns>
        public static PointLatLng GetGreatCirclePoint(double f, double d, double lat1, double lon1, double lat2, double lon2)
        {
            double A = Math.Sin((1d - f) * d) / Math.Sin(d);
            double B = Math.Sin(f * d) / Math.Sin(d);

            double x = A * Math.Cos(lat1) * Math.Cos(lon1) + B * Math.Cos(lat2) * Math.Cos(lon2);
            double y = A * Math.Cos(lat1) * Math.Sin(lon1) + B * Math.Cos(lat2) * Math.Sin(lon2);
            double z = A * Math.Sin(lat1) + B * Math.Sin(lat2);

            double lat = Math.Atan2(z, Math.Sqrt(x * x + y * y));
            double lon = Math.Atan2(y, x);

            return new PointLatLng(ToDegrees(lat), ToDegrees(lon));
        }

        /// <summary>
        /// Gets the distance (in RADIANS) between two points specified by latitude/longitude in RADIANS!
        /// </summary>
        /// <param name="lat1">Latitude of Startpoint IN RADIANS</param>
        /// <param name="lon1">Longitude of Startpoint IN RADIANS</param>
        /// <param name="lat2">Latitude of Endpoint IN RADIANS</param>
        /// <param name="lon2">Longitude of Endpoint IN RADIANS</param>
        /// <returns>Distabnce in RADIANS</returns>
        public static double GetDistanceInRadians(double lat1, double lon1, double lat2, double lon2)
        {
            double d = 2d * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin((lat1 - lat2) / 2d), 2d) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin((lon1 - lon2) / 2d), 2d)));
            return d;
        }

        /// <summary>
        /// Gets the distance (in kilometer) between two points
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetDistanceInKM(PointLatLng p1, PointLatLng p2)
        {
            return GMapProviders.EmptyProvider.Projection.GetDistance(p1, p2);
        }

        /// <summary>
        /// Gets the distance (in meter) between two points
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double GetDistanceInMeter(PointLatLng p1, PointLatLng p2)
        {
            return GMapProviders.EmptyProvider.Projection.GetDistance(p1, p2) * 1000d;
        }

        private static double SignedPolygonArea(List<PointLatLng> points)
        {
            // Add the first point to the end.
            int pointsCount = points.Count;
            PointLatLng[] pts = new PointLatLng[pointsCount + 1];
            points.CopyTo(pts, 0);
            pts[pointsCount] = points[0];

            for (int i = 0; i < pointsCount + 1; ++i)
            {
                //6378137 meter is the radius of the earth
                pts[i].Lat = pts[i].Lat * (System.Math.PI * 6378137 / 180);
                pts[i].Lng = pts[i].Lng * (System.Math.PI * 6378137 / 180);
            }

            // Get the areas.
            double area = 0;
            for (int i = 0; i < pointsCount; i++)
            {
                area += (pts[i + 1].Lat - pts[i].Lat) * (pts[i + 1].Lng + pts[i].Lng) / 2;
            }

            // Return the result.
            return area;
        }

        /// <summary>
        /// Get the area of a polygon
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static double GetPolygonArea(List<PointLatLng> points)
        {
            // Return the absolute value of the signed area.
            // The signed area is negative if the polygon is oriented clockwise.
            return Math.Abs(SignedPolygonArea(points));
        }

        /// <summary>
        /// Get the area of a circle
        /// </summary>
        /// <param name="center">The center of the circle</param>
        /// <param name="edgePoint">The point on the edge of the circle</param>
        /// <returns></returns>
        public static double GetCircleArea(PointLatLng center, PointLatLng edgePoint)
        {
            double radius = GetDistanceInMeter(center, edgePoint);
            double aera = System.Math.PI * radius * radius;

            return aera;
        }

        /// <summary>
        /// Get which side (left or right) of the point to Line(pnt1,pnt2)
        /// </summary>
        /// <param name="pnt1"></param>
        /// <param name="pnt2"></param>
        /// <param name="pnt"></param>
        /// <returns></returns>
        public static string GetSide(PointLatLng pnt1, PointLatLng pnt2, PointLatLng pnt)
        {
            double num = ((pnt2.Lat - pnt1.Lat)*(pnt.Lng - pnt1.Lng)) - ((pnt.Lat - pnt1.Lat)*(pnt2.Lng - pnt1.Lng));
            if (Math.Abs(num) <= 0.00000001)
            {
                return null; // The third pnt is on the line
            }
            if (num > 0.0)
            {
                return "left";
            }
            if (num < 0.0)
            {
                return "right";
            }
            return null;
        }

        /// <summary>
        /// Get which side (left or right) of the point to Line(pnt1,pnt2)
        /// </summary>
        /// <param name="pnt1"></param>
        /// <param name="pnt2"></param>
        /// <param name="pnt"></param>
        /// <returns>1:left, -1:right, 0:on the line</returns>
        public static int GetPointSide(PointLatLng pnt1, PointLatLng pnt2, PointLatLng pnt)
        {
            double num = ((pnt2.Lat - pnt1.Lat) * (pnt.Lng - pnt1.Lng)) - ((pnt.Lat - pnt1.Lat) * (pnt2.Lng - pnt1.Lng));
            if (Math.Abs(num) <= 0.00000001)
            {
                return 0; // The third point is on the line
            }
            if (num > 0.0)
            {
                return 1;
            }
            if (num < 0.0)
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// Judge two line is intersected or not
        /// </summary>
        /// <param name="start1"></param>
        /// <param name="end1"></param>
        /// <param name="start2"></param>
        /// <param name="end2"></param>
        /// <returns></returns>
        public static bool IsLineIntersect(PointLatLng start1, PointLatLng end1, PointLatLng start2, PointLatLng end2)
        {
            int test1 = GetPointSide(start1, end1, start2) * GetPointSide(start1, end1, end2);
            int test2 = GetPointSide(start2, end2, start1) * GetPointSide(start2, end2, end1);

            return (test1 <= 0) && (test2 <= 0);
        }

        public static RectLatLng GetRegionMaxRect(GMapPolygon polygon)
        {
            double latMin = 90;
            double latMax = -90;
            double lngMin = 180;
            double lngMax = -180;
            foreach (var point in polygon.Points)
            {
                if (point.Lat < latMin)
                {
                    latMin = point.Lat;
                }
                if (point.Lat > latMax)
                {
                    latMax = point.Lat;
                }
                if (point.Lng < lngMin)
                {
                    lngMin = point.Lng;
                }
                if (point.Lng > lngMax)
                {
                    lngMax = point.Lng;
                }
            }

            return new RectLatLng(latMax, lngMin, lngMax - lngMin, latMax - latMin);
        }

        public static RectLatLng GetPointsMaxRect(IList<PointLatLng> points)
        {
            double latMin = 90;
            double latMax = -90;
            double lngMin = 180;
            double lngMax = -180;
            foreach (var point in points)
            {
                if (point.Lat < latMin)
                {
                    latMin = point.Lat;
                }
                if (point.Lat > latMax)
                {
                    latMax = point.Lat;
                }
                if (point.Lng < lngMin)
                {
                    lngMin = point.Lng;
                }
                if (point.Lng > lngMax)
                {
                    lngMax = point.Lng;
                }
            }

            return new RectLatLng(latMax, lngMin, lngMax - lngMin, latMax - latMin);
        }

        // Get routes from Shape File 
        //public static List<GMapRoute> GetRoutesFromShapefile(String filename)
        //{

        //    if (".shp" != Path.GetExtension(filename).ToLower())
        //    {
        //        throw new Exception("The file must be a shapefile!");
        //    }

        //    String basePath = Path.GetDirectoryName(filename) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(filename);

        //    if (!File.Exists(basePath + ".prj"))
        //    {
        //        throw new Exception("Could not find the projection file!");
        //    }

        //    //Get info from WKT-File:
        //    String wkt = File.ReadAllText(basePath + ".prj");

        //    ICoordinateSystem gcs = CoordinateSystemWktReader.Parse(wkt) as ICoordinateSystem;
        //    bool doTransformation = true;
        //    if (gcs.AuthorityCode == 4326)
        //        doTransformation = false;

        //    GeographicCoordinateSystem wgs84 = GeographicCoordinateSystem.WGS84;
        //    CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
        //    ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(gcs, wgs84);

        //    //Open shape-file
        //    IntPtr shapeFile = ShapeLib.SHPOpen(basePath, "rb");

        //    int numOfRecords = 0;

        //    //Check type and get number of entries
        //    ShapeLib.ShapeType shapeType = 0;
        //    ShapeLib.SHPGetInfo(shapeFile, ref numOfRecords, ref shapeType, null, null);

        //    if(shapeType != ShapeLib.ShapeType.PolyLine)
        //    {
        //         throw new Exception("The shape type is not polyline but " + shapeType);
        //    }

        //    List<GMapRoute> lstRoutes = new List<GMapRoute>(numOfRecords);

        //    //Get info from shapefile and save data:
        //    for (int i = 0; i < numOfRecords; i++)
        //    {
        //        //Add all GPS-Points:
        //        //Get pointer to object
        //        IntPtr ptrPolyline = ShapeLib.SHPReadObject(shapeFile, i);

        //        //Create actual object:
        //        ShapeLib.SHPObject polyline = new ShapeLib.SHPObject();
        //        Marshal.PtrToStructure(ptrPolyline, polyline);

        //        if (polyline.nParts > 1)
        //        {
        //            //TODO...
        //            throw new Exception("Multipart polylines are not supported!");
        //        }
        //        else
        //        {
        //            //Get number of points and arrays of X and Y values:
        //            int numPoints = polyline.nVertices;

        //            List<PointLatLng> lstpoints = new List<PointLatLng>(numPoints);

        //            double[] xCoord = new double[numPoints];
        //            double[] yCoord = new double[numPoints];

        //            //Fill the arrays:
        //            Marshal.Copy(polyline.padfX, xCoord, 0, numPoints);
        //            Marshal.Copy(polyline.padfY, yCoord, 0, numPoints);

        //            //Add all GPS-Points
        //            for (int j = 0; j < numPoints; j++)
        //            {
        //                double latitude = 0d;
        //                double longitude = 0d;

        //                if (doTransformation)
        //                {
        //                    //Convert from original coordinate system to wgs84!
        //                    double[] fromPoint = new double[] { xCoord[j], yCoord[j] };
        //                    double[] toPoint = trans.MathTransform.Transform(fromPoint);
        //                    //Get point from polyline
        //                    longitude = toPoint[0];
        //                    latitude = toPoint[1];
        //                }
        //                else
        //                {
        //                    longitude = xCoord[j];
        //                    latitude = yCoord[j];
        //                }

        //                lstpoints.Add(new PointLatLng(latitude, longitude));
        //            }

        //            lstRoutes.Add(new GMapRoute(lstpoints, "Route" + (i+1).ToString()));
        //        }

        //        //Release shape-object:
        //        polyline = null;
        //        ShapeLib.SHPDestroyObject(ptrPolyline);
        //    }
        //    //Finally close files
        //    ShapeLib.SHPClose(shapeFile);

        //    return lstRoutes;
        //}
    }
}
