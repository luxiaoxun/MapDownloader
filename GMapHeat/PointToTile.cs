using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace gheat
{
    public class PointToTile
    {
        private List<GMap.NET.Point>[,] _gridTracking;
        public int zoomLevel = -1;
        
        public PointToTile()
        {
            _gridTracking = new List<GMap.NET.Point>[300,300] ;
            for (int x = 0; x < _gridTracking.GetUpperBound(0); x++)
            {
                for (int y = 0; y < _gridTracking.GetUpperBound(1); y++)
                {
                    _gridTracking[x, y] = new List<GMap.NET.Point>();
                }
            }
        }

        public GMap.NET.Point[] GetMapPoints(int gridX, int gridY)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("x=" + gridX.ToString() + " y=" + gridY.ToString());
                return _gridTracking[gridX, gridY].ToArray();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("NOT FOUND x=" + gridX.ToString() + " y=" + gridY.ToString());
                return null;
            }
        }

        public void SetMapPoints(int gridX, int gridY, GMap.NET.Point point)
        {
            _gridTracking[gridX, gridY].Add(point);
        }

        public void SetMapPoints(int gridX, int gridY, GMap.NET.Point[] points)
        {
            _gridTracking[gridX, gridY].AddRange(points); 
        }
    }
}
