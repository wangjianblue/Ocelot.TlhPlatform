using System;
using System.Configuration;
using System.Collections.Specialized;

namespace TlhPlatform.Core.ConfigurationClass
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public static class SystemConfig
    {
        private static GlobalConfigurationSource globalSource =null;
       /// <summary>
       ///全局来源
       /// </summary>
        public static GlobalConfigurationSource GlobalSource
        {
            get
            {
                if (globalSource == null)
                {
                    globalSource = GlobalConfigurationSource.Create();
                }
                return globalSource;
            }
        }
  

        /// <summary>
        /// 获取 GlobalSettings.config 文件中的所有appSettings节点
        /// </summary>
        public static NameValueCollection GlobalAppSettings
        {
            get
            {
                NameValueCollection result = new NameValueCollection ();
                AppSettingsSection section = GlobalSource.GetSection<AppSettingsSection>("appSettings");
                if(section != null)
                {
                    //遍历所有appSettings 节点
                    foreach (string key in section.Settings.AllKeys)
                    {
                        result.Add(key, section.Settings[key].Value);
                    }   
                }
                return result;
            }
        }
    }
}