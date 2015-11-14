using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using MySql.Data.MySqlClient;

namespace MapDownloader
{
    public class MySQLPoiCache
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MySQLPoiCache));

        private string connectionString = string.Empty;
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

                    if (Initialized)
                    {
                        Initialize();
                    }
                }
            }
        }

        bool initialized = false;
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

        public bool Initialize()
        {
            lock (this)
            {
                if (!initialized)
                {
                    #region prepare mssql & cache table
                    try
                    {
                        // different connections so the multi-thread inserts and selects don't collide on open readers.
                        using(var connCreate = new MySqlConnection(connectionString))
                        {
                            using (MySqlCommand cmd = new MySqlCommand(
                               @" CREATE TABLE IF NOT EXISTS `gmappoicache` (
                             `id` int(11) NOT NULL AUTO_INCREMENT,
                             `name` varchar(100) NOT NULL,
                             `address` varchar(200) NOT NULL,
                             `longitude` double(0) NOT NULL,
                             `latitude` double(0) NOT NULL,
                             PRIMARY KEY (`id`),
                               UNIQUE KEY(`name`)
                           ) ENGINE=InnoDB DEFAULT CHARSET=utf8;", connCreate))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }

                        Initialized = true;
                    }
                    catch (Exception ex)
                    {
                        this.initialized = false;
                        log.Error(ex);
                    }
                    #endregion
                }
                return initialized;
            }
        }

        public bool PutPoiDataToCache()
        {
            try
            {
                using (var connGet = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = connGet.CreateCommand())
                    {
                        cmd.CommandText =
                            "INSERT INTO `gmappoicache` ( id, name, address, longitude, latitude ) VALUES ( @id, @name, @address, @longitude, @latitude )";

                        cmd.Parameters.Add("@id", MySqlDbType.Int32);
                        cmd.Parameters.Add("@name", MySqlDbType.String);
                        cmd.Parameters.Add("@address", MySqlDbType.String);
                        cmd.Parameters.Add("@longitude", MySqlDbType.Double);
                        cmd.Parameters.Add("@latitude", MySqlDbType.Double);

                        cmd.Parameters["@id"].Value = 1;
                        cmd.Parameters["@name"].Value = "aaaa";
                        cmd.Parameters["@address"].Value = "bbbb";
                        cmd.Parameters["@longitude"].Value = 128.23565;
                        cmd.Parameters["@latitude"].Value = 32.1235456;
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                log.Error(ex);
            }
        }
    }
}
