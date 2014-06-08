using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace NetUtilityLib
{
    public static class ConfigHelper
    {
        ///<summary> 
        ///依据连接串名字connectionName返回数据连接字符串  
        ///</summary> 
        ///<param name="connectionName"></param> 
        ///<returns></returns> 
        public static string GetConnectionStringsConfig(string connectionName)
        {
            //默认config读取
            //string connectionString =
            //        ConfigurationManager.ConnectionStrings[connectionName].ConnectionString.ToString();

            //指定config文件读取
            string file = System.Windows.Forms.Application.ExecutablePath;
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            string connectionString =
                config.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString.ToString();
            //Console.WriteLine(connectionString);
            return connectionString;
        }

        ///<summary> 
        ///更新连接字符串  
        ///</summary> 
        ///<param name="newName">连接字符串名称</param> 
        ///<param name="newConString">连接字符串内容</param> 
        ///<param name="newProviderName">数据提供程序名称</param> 
        public static void UpdateConnectionStringsConfig(string newName, string newConString, string newProviderName)
        {
            // 打开可执行的配置文件*.exe.config  
            //Configuration config =
            //    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            //指定config文件读取
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);

            bool exist = false; //记录该连接串是否已经存在  
            //如果要更改的连接串已经存在  
            //if (ConfigurationManager.ConnectionStrings[newName] != null) //默认config
            //{
            //    exist = true;
            //}
            if (config.ConnectionStrings.ConnectionStrings[newName] != null)
            {
                exist = true;
            }
            //新建一个连接字符串实例  
            ConnectionStringSettings mySettings =
                new ConnectionStringSettings(newName, newConString, newProviderName);

            // 如果连接串已存在，首先删除它  
            if (exist)
            {
                config.ConnectionStrings.ConnectionStrings.Remove(newName);
            }
            // 将新的连接串添加到配置文件中.  
            config.ConnectionStrings.ConnectionStrings.Add(mySettings);
            // 保存对配置文件所作的更改  
            config.Save(ConfigurationSaveMode.Modified);
            // 强制重新载入配置文件的ConnectionStrings配置节  
            ConfigurationManager.RefreshSection("ConnectionStrings");
        }

        ///<summary> 
        ///返回*.exe.config文件中appSettings配置节的value项  
        ///</summary> 
        ///<param name="strKey"></param> 
        ///<returns></returns> 
        public static string GetAppConfig(string strKey)
        {
            //foreach (string key in ConfigurationManager.AppSettings)
            //{
            //    if (key == strKey)
            //    {
            //        return ConfigurationManager.AppSettings[strKey];
            //    }
            //}
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == strKey)
                {
                    return config.AppSettings.Settings[strKey].Value.ToString();
                }
            }
            return null;
        }

        ///<summary>  
        ///在*.exe.config文件中appSettings配置节增加一对键值对  
        ///</summary>  
        ///<param name="newKey"></param>  
        ///<param name="newValue"></param>  
        public static void UpdateAppConfig(string newKey, string newValue)
        {
            // Open App.Config of executable  
            //Configuration config =
            //    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string file = System.Windows.Forms.Application.ExecutablePath;
            Configuration config = ConfigurationManager.OpenExeConfiguration(file);
            bool exist = false;
            foreach (string key in config.AppSettings.Settings.AllKeys)
            {
                if (key == newKey)
                {
                    exist = true;
                }
            }

            // You need to remove the old settings object before you can replace it  
            if (exist)
            {
                config.AppSettings.Settings.Remove(newKey);
            }
            // Add an Application Setting.  
            config.AppSettings.Settings.Add(newKey, newValue);
            // Save the changes in App.config file.  
            config.Save(ConfigurationSaveMode.Modified);
            // Force a reload of a changed section.  
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
