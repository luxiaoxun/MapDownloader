using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace GMapProvidersExt.Baidu.HTTPService
{
    /// <summary>
    /// 提供地图相关服务
    /// </summary>
    public class MapService:ServiceBase
    {
        private static string _road_url = "http://online{0}.map.bdimg.com/onlinelabel/";  //地图切片URL
        private static string _sate_url = "http://shangetu{0}.map.bdimg.com/it/";  //卫星图切片URL

        /* http://online9.map.bdimg.com/onlinelabel/?qt=tile&x=796&y=287&z=12&styles=pl */
        /* http://shangetu9.map.bdimg.com/it/u=x=796;y=287;z=13;v=009;type=sate&fm=46 */

        /// <summary>
        /// 下载地图瓦片
        /// </summary>
        /// <param name="x">瓦片方块横坐标</param>
        /// <param name="y">瓦片方块纵坐标</param>
        /// <param name="zoom">当前地图缩放级别（1-18）</param>
        /// <param name="map_mode">地图模式</param>
        /// <param name="load_mode">加载瓦片方式</param>
        /// <returns></returns>
        public Bitmap LoadMapTile(int x, int y, int zoom, MapMode map_mode, LoadMapMode load_mode)
        {
            if (load_mode == LoadMapMode.Server)  //直接从服务器下载图片
            {
                return TileFromServer(zoom, x, y, map_mode);
            }
            else if (load_mode == LoadMapMode.Cache)  //从本地缓存中下载图片
            {
                return TileFromCache(zoom, x, y, map_mode);
            }
            else if (load_mode == LoadMapMode.CacheServer)  //先从本地缓存中找，如果没有则从服务器上下载
            {
                Bitmap bitmap = TileFromCache(zoom, x, y, map_mode);
                if (bitmap == null)
                {
                    bitmap = TileFromServer(zoom, x, y, map_mode);
                }
                return bitmap;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 清空瓦片缓存
        /// </summary>
        public void ClearTileCache()
        {
            try
            {
                string cache_path = Properties.BMap.Default.MapCachePath;
                DirectoryInfo dir = new DirectoryInfo(cache_path);
                foreach (DirectoryInfo d in dir.GetDirectories())
                {
                    if (d.Name == MapMode.Normal.ToString() || d.Name == MapMode.RoadNet.ToString() || d.Name == MapMode.Satellite.ToString())
                    {
                        d.Delete(true);
                    }
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 从缓存中加载瓦片
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="map_mode"></param>
        /// <returns></returns>
        private Bitmap TileFromCache(int zoom, int x, int y, MapMode map_mode)
        {
            try
            {
                string cache_path = Properties.BMap.Default.MapCachePath;
                if (Directory.Exists(cache_path + "\\" + map_mode.ToString()))
                {
                    string cache_name = cache_path + "\\" + map_mode.ToString() + "\\" + zoom + "_" + x + "_" + y + ".bmp";
                    if (File.Exists(cache_name))
                    {
                        return new Bitmap(cache_name);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 从服务器上加载瓦片
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="map_mode"></param>
        /// <returns></returns>
        private Bitmap TileFromServer(int zoom, int x, int y, MapMode map_mode)
        {
            try
            {
                Random r = new Random(DateTime.Now.Millisecond);
                int server_index = r.Next(0, 10);  //随即产生0~9之间的整数
                string url = "";
                if (map_mode == MapMode.Normal) //地图
                {
                    url = String.Format(_road_url, server_index) + "?qt=tile&x=" + x + "&y=" + y + "&z=" + zoom + "&styles=pl";
                }
                if (map_mode == MapMode.Satellite) //卫星图
                {
                    url = String.Format(_sate_url, server_index) + "u=x=" + x + ";y=" + y + ";z=" + zoom + ";v=009;type=sate&fm=46";
                }
                if (map_mode == MapMode.RoadNet) //道路网
                {
                    url = String.Format(_road_url, server_index) + "?qt=tile&x=" + x + "&y=" + y + "&z=" + zoom + "&styles=sl";
                }
                byte[] bytes = DownloadData(url);
                Bitmap bitmap = Image.FromStream(new MemoryStream(bytes)) as Bitmap;
                SaveTile2Cache(zoom, x, y, map_mode, bitmap);
                return bitmap;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 将从服务器上下载的瓦片保存到缓存
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="map_mode"></param>
        /// <param name="tile"></param>
        private void SaveTile2Cache(int zoom, int x, int y, MapMode map_mode, Bitmap tile)
        {
            try
            {
                string cache_path = Properties.BMap.Default.MapCachePath;
                if (!Directory.Exists(cache_path + "\\" + map_mode.ToString()))
                {
                    Directory.CreateDirectory(cache_path + "\\" + map_mode.ToString());
                }
                tile.Save(cache_path + "\\" + map_mode.ToString() + "\\" + zoom + "_" + x + "_" + y + ".bmp");
            }
            catch
            {

            }
        }
    }
}
