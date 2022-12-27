using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace XRDL.DDL.K3.BOS.Tools
{
    public class BaseConfigHelper
    {
        public BaseConfigHelper()
        {
            ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
            filemap.ExeConfigFilename = filePath;//配置文件路径
            this.config = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
        }

        public BaseConfigHelper(string ConfigPath)
        {
            ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
            filemap.ExeConfigFilename = ConfigPath;//配置文件路径
            this.config = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
        }

        protected string filePath = AppDomain.CurrentDomain.BaseDirectory + "Config\\Config.config";
        protected Configuration config;

        public virtual string GetAppSettings(string keyName)
        {
            if (AppSettingsKeyExists(keyName, this.config))
            {
                return config.AppSettings.Settings[keyName].Value;
            }
            else
            {
                return null;
            }
        }

        public virtual string GetConnection(string connectionString)
        {
            try
            {
                return config.ConnectionStrings.ConnectionStrings[connectionString].ConnectionString;
            }
            catch
            {
                return null;
            }
        }


        /**//// <summary>
            /// 保存节点中ConnectionStrings的子节点配置项的值
            /// </summary>
            /// <param name="elementValue"></param>
        public virtual void ConnectionStringsSave(string ConnectionStringsName, string elementValue)
        {
            this.config.ConnectionStrings.ConnectionStrings[ConnectionStringsName].ConnectionString = elementValue;
            this.config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        /**//// <summary>
            /// 判断appSettings中是否有此项
            /// </summary>
        protected virtual bool AppSettingsKeyExists(string strKey, Configuration config)
        {
            foreach (string str in config.AppSettings.Settings.AllKeys)
            {
                if (str == strKey)
                {
                    return true;
                }
            }
            return false;
        }

        /**//// <summary>
            /// 保存appSettings中某key的value值
            /// </summary>
            /// <param name="strKey">key's name</param>
            /// <param name="newValue">value</param>
        public virtual void AppSettingsSave(string strKey, string newValue)
        {
            if (AppSettingsKeyExists(strKey, this.config))
            {
                config.AppSettings.Settings[strKey].Value = newValue;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}
