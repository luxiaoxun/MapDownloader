
namespace GMap.NET.CacheProviders
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using GMap.NET;
    using MySql.Data.MySqlClient;
    using GMap.NET.MapProviders;

    /// <summary>
    /// image cache for mysql server
    /// </summary>
    public class MySQLPureImageCacheMulti : PureImageCache, IDisposable
    {
        string connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                if (connectionString != value)
                {
                    connectionString = value;
                    initialized = false;
                    Initialize();
                }
            }
        }

        bool initialized = false;

        /// <summary>
        /// is cache initialized
        /// </summary>
        public bool Initialized
        {
            get
            {
                lock (this)
                {
                    return initialized;
                }
            }
            private set
            {
                lock (this)
                {
                    initialized = value;
                }
            }
        }

        /// <summary>
        /// inits connection to server
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            lock (this)
            {
                if (!initialized)
                {
                    #region prepare mssql & cache table
                    try
                    {
                        using (MySqlConnection cnCreate = new MySqlConnection(connectionString))
                        {
                            cnCreate.Open();
                            using (MySqlCommand cmd = new MySqlCommand(
                               @" CREATE TABLE IF NOT EXISTS `gmapnetcache` (
                             `Type` int(10) NOT NULL,
                             `Zoom` int(10) NOT NULL,
                             `X` int(10) NOT NULL,
                             `Y` int(10) NOT NULL,
                             `Tile` longblob NOT NULL,
                             PRIMARY KEY (`Type`,`Zoom`,`X`,`Y`)
                           ) ENGINE=InnoDB DEFAULT CHARSET=utf8;", cnCreate))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }

                        initialized = true;
                    }
                    catch (Exception ex)
                    {
                        this.initialized = false;
                        Debug.WriteLine("MySQL Initialize: "+ex.Message);
                    }
                    #endregion
                }
                return initialized;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
        }
        #endregion

        #region PureImageCache Members
        public bool PutImageToCache(byte[] tile, int type, GPoint pos, int zoom)
        {
            bool ret = true;
            {
                if (Initialize())
                {
                    try
                    {
                        //lock (cmdInsert)
                        using (MySqlConnection cnSet = new MySqlConnection(connectionString))
                        {
                            cnSet.Open();
                            using (
                                MySqlCommand cmdInsert =
                                    new MySqlCommand(
                                        "INSERT IGNORE INTO `gmapnetcache` ( Type, Zoom, X, Y, Tile ) VALUES ( @type, @zoom, @x, @y, @tile )",
                                        cnSet))
                            {
                                cmdInsert.Parameters.Add("@type", MySqlDbType.Int32);
                                cmdInsert.Parameters.Add("@zoom", MySqlDbType.Int32);
                                cmdInsert.Parameters.Add("@x", MySqlDbType.Int32);
                                cmdInsert.Parameters.Add("@y", MySqlDbType.Int32);
                                cmdInsert.Parameters.Add("@tile", MySqlDbType.Blob); //, calcmaximgsize);

                                cmdInsert.Parameters["@type"].Value = type;
                                cmdInsert.Parameters["@zoom"].Value = zoom;
                                cmdInsert.Parameters["@x"].Value = pos.X;
                                cmdInsert.Parameters["@y"].Value = pos.Y;
                                cmdInsert.Parameters["@tile"].Value = tile;
                                cmdInsert.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("MySQL PutImageToCache: " + ex.ToString());
                        ret = false;
                        //Dispose();
                    }
                }
            }
            return ret;
        }

        public PureImage GetImageFromCache(int type, GPoint pos, int zoom)
        {
            PureImage ret = null;
            {
                if (Initialize())
                {
                    try
                    {
                        object odata = null;
                        using (MySqlConnection cnGet = new MySqlConnection(connectionString))
                        {
                            cnGet.Open();
                            using (
                                MySqlCommand cmdFetch =
                                    new MySqlCommand(
                                        "SELECT Tile FROM `gmapnetcache` WHERE Type=@type AND Zoom=@zoom AND X=@x AND Y=@y",
                                        cnGet))
                            {
                                cmdFetch.Parameters.Add("@type", MySqlDbType.Int32);
                                cmdFetch.Parameters.Add("@zoom", MySqlDbType.Int32);
                                cmdFetch.Parameters.Add("@x", MySqlDbType.Int32);
                                cmdFetch.Parameters.Add("@y", MySqlDbType.Int32);
                                cmdFetch.Prepare();
                                cmdFetch.Parameters["@type"].Value = type;
                                cmdFetch.Parameters["@zoom"].Value = zoom;
                                cmdFetch.Parameters["@x"].Value = pos.X;
                                cmdFetch.Parameters["@y"].Value = pos.Y;
                                odata = cmdFetch.ExecuteScalar();
                            }
                        }
                        if (odata != null && odata != DBNull.Value)
                        {
                            byte[] tile = (byte[])odata;
                            if (tile != null && tile.Length > 0)
                            {
                                if (GMapProvider.TileImageProxy != null)
                                {
                                    ret = GMapProvider.TileImageProxy.FromArray(tile);
                                }
                            }
                            tile = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("MySQL GetImageFromCache: " + ex.ToString());
                        ret = null;
                        //Dispose();
                    }
                }
            }
            return ret;
        }

        int PureImageCache.DeleteOlderThan(DateTime date, int? type)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

