using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using MySql.Data.MySqlClient;

namespace GMapPOI
{
    public class MySQLPoiCache
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MySQLPoiCache));

        public MySQLPoiCache(string connString)
        {
            this.ConnectionString = connString;
        }

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
                            connCreate.Open();

                            using (MySqlCommand cmd = new MySqlCommand(
                               @" CREATE TABLE IF NOT EXISTS `gmappoicache` (
                             `id` bigint NOT NULL AUTO_INCREMENT,
                             `name` varchar(100) NOT NULL,
                             `address` varchar(200) NOT NULL,
                             `province` varchar(20) NOT NULL,
                             `city` varchar(20) NOT NULL,
                             `longitude` double NOT NULL,
                             `latitude` double NOT NULL,
                             PRIMARY KEY (`id`)
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

        public bool PutPoiDataToCache(PoiData data)
        {
            try
            {
                if (!Initialized)
                {
                    this.Initialize();
                }

                using (var connPut = new MySqlConnection(connectionString))
                {
                    connPut.Open();

                    using (MySqlCommand cmd = connPut.CreateCommand())
                    {
                        cmd.CommandText =
                            "INSERT INTO `gmappoicache` ( name, address,province,city, longitude, latitude ) VALUES ( @name, @address,@province, @city, @longitude, @latitude )";

                        cmd.Parameters.Add("@name", MySqlDbType.String);
                        cmd.Parameters.Add("@address", MySqlDbType.String);
                        cmd.Parameters.Add("@province", MySqlDbType.String);
                        cmd.Parameters.Add("@city", MySqlDbType.String);
                        cmd.Parameters.Add("@longitude", MySqlDbType.Double);
                        cmd.Parameters.Add("@latitude", MySqlDbType.Double);

                        cmd.Parameters["@name"].Value = data.Name;
                        cmd.Parameters["@address"].Value = data.Address;
                        cmd.Parameters["@province"].Value = data.Province;
                        cmd.Parameters["@city"].Value = data.City;
                        cmd.Parameters["@longitude"].Value = data.Lng;
                        cmd.Parameters["@latitude"].Value = data.Lat;
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
