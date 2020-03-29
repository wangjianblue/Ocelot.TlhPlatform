namespace TlhPlatform.Core
{
    /// <summary>
    /// 配置工厂类
    /// </summary>
    public static class ConfigurationFactory
    {
          
        private static XmlDataConfigBase xmDataconfig = null;
        private static XmlDataConfigBase XmlDataConfig
        {
            get
            {
                if (xmDataconfig == null)
                {
                    xmDataconfig = new XmlDataConfigBase();
                }
                return xmDataconfig;
            }
        }

        private static bool needLog = true;
        public static bool NeedLog
        {
            get { return needLog; }
            set { needLog = value; }
        }

        public static T GetConfigurationItem<T>(string sectionName, string filePath) where T : class
        { 
            return XmlDataConfig.GetFromCache<T>(sectionName, filePath, needLog);
        }

        public static void RemoveCache<T>(string sectionName) where T : class
        {
            XmlDataConfig.RemoveCache<T>(sectionName);
        }
        public static T GetConfigurationItemByPath<T>(string filePath, string key) where T : class
        {
            return XmlDataConfig.GetFromCacheByFullPath<T>(key, filePath, needLog);
        }
    }
}
