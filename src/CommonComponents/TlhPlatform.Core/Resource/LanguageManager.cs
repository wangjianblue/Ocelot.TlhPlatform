using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TlhPlatform.Core.ConfigurationClass;
using TlhPlatform.Core.Utility;
namespace TlhPlatform.Core.Resource
{
    /// <summary>
    /// 多语言管理配置
    /// </summary>
    public static class LanguageManager
    { 

        private const string LiftSECTION_CONFIGNAME = "LanguageConfigFile";

        private static string configurationFilePath = string.Empty;
        private static object LockObject = new object();
        static LanguageManager()
        {

            var filePath = Path.GetDirectoryName(ConfigurationFilePath);
            //文件监控
            var delayFileSystemWatcher = new DelayFileSystemWatcher(filePath, "*.*", (object sender, FileSystemEventArgs e) =>
            {
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Changed:
                        Console.WriteLine("File change event processing logic{0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name);
                        stringDictory = null;
                        ConfigurationFactory.RemoveCache<ListConfig>(LiftSECTION_CONFIGNAME);
                        ConstructLanguageItem();
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
                    configurationFilePath = SystemConfig.GlobalAppSettings[LiftSECTION_CONFIGNAME];
                    configurationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configurationFilePath);
                }
                return configurationFilePath;
            }
        }

        public static Language ConfigLanguageFile
        {
            get
            {
                Language listConfig = ConfigurationFactory.GetConfigurationItem<Language>(LiftSECTION_CONFIGNAME, ConfigurationFilePath);
                return listConfig;
            }
        }


        private static Dictionary<string, LanguageLocaleResource> stringDictory = null;
        /// <summary>
        /// 获取所有多语言配置
        /// </summary>
        private static void ConstructLanguageItem()
        {
            if (stringDictory == null)
            {
                stringDictory = new Dictionary<string, LanguageLocaleResource>();
            }
            lock (LockObject)
            {
                foreach (LanguageLocaleResource item in ConfigLanguageFile.LocaleResource)
                {
                    if (!stringDictory.ContainsKey(item.Name))
                    {
                        stringDictory.Add(item.Name, item);
                    }
                }
            }
        }
        /// <summary>
        /// 获取多语言常量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static LanguageLocaleResource GetLanguageItem(string key)
        {
            ConstructLanguageItem();
            if (stringDictory.ContainsKey(key))
            {
                LanguageLocaleResource item = stringDictory[key];
                return item;
            }
            return null;
        }
    }
}
