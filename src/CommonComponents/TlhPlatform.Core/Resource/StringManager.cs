using System;
using System.Collections.Generic;
using System.IO;
using TlhPlatform.Core.ConfigurationClass;
using TlhPlatform.Core.Utility;

namespace TlhPlatform.Core.Resource
{
    /// <summary>
    /// 字符串配置
    /// </summary>
    public class StringManager
    {

        private const string STRINGSECTION_CONFIGNAME = "StringConfigFile";
        private static string configurationFilePath = string.Empty;
        private static object LockObject = new object();


        static StringManager()
        {

            var filePath = Path.GetDirectoryName(ConfigurationFilePath);
            var delayFileSystemWatcher = new DelayFileSystemWatcher(filePath, "*.*", (object sender, FileSystemEventArgs e) =>
            {
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Changed:
                        Console.WriteLine("File change event processing logic{0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name);
                        stringDictory = null;
                        ConfigurationFactory.RemoveCache<StringSet>(STRINGSECTION_CONFIGNAME);
                        ConstructStringItem();
                        break;
                    default:
                        break;
                }
            }, 1500);
        }
        internal static string ConfigurationFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(configurationFilePath))
                {
                    configurationFilePath = SystemConfig.GlobalAppSettings[STRINGSECTION_CONFIGNAME];
                    configurationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configurationFilePath);
                }
                return configurationFilePath;
            }
        }
        public static StringSet ConfigStringFile
        {
            get
            {
                StringSet stringSet = ConfigurationFactory.GetConfigurationItem<StringSet>(STRINGSECTION_CONFIGNAME, ConfigurationFilePath);
                return stringSet;
            }
        }

        private static Dictionary<string, StringItem> stringDictory = null;
        /// <summary>
        /// 
        /// </summary>
        private static void ConstructStringItem()
        {
            if (stringDictory == null)
            {
                stringDictory = new Dictionary<string, StringItem>();
            }
            lock (LockObject)
            {
                foreach (StringItem item in ConfigStringFile.StringItem)
                {
                    if (!stringDictory.ContainsKey(item.key))
                    {
                        stringDictory.Add(item.key, item);
                    }
                }
            }
        }
        /// <summary>
        /// 获取字符串常量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static StringItem GetStringItem(string key)
        {
            ConstructStringItem();
            if (stringDictory.ContainsKey(key))
            {
                StringItem item = stringDictory[key];
                return item;
            }
            return null;
        }
    }
}
