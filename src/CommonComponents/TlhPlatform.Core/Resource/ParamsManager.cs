/*
 * @Descripttion: 
 * @version: 
 * @Author: wangjf
 * @Date: 2019-07-31 13:25:53
 * @LastEditors: wangjf
 * @LastEditTime: 2019-08-07 15:32:55
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TlhPlatform.Core.ConfigurationClass;
using TlhPlatform.Core.Utility;
namespace TlhPlatform.Core.Resource
{
    /// <summary>
    /// 参数配置xml
    /// </summary>
    public class ParamsManager
    {
        private const string PARASECTION_CONFIGNAME = "parameConfigFile";
        private static string configurationFilePath = string.Empty;
        private static object LockObject = new object();     
        
        static ParamsManager()
        {
            var filePath = Path.GetDirectoryName(ConfigurationFilePath);
            var delayFileSystemWatcher = new DelayFileSystemWatcher(filePath, "*.*", (object sender, FileSystemEventArgs e) =>
            {
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Changed:
                        Console.WriteLine("File change event processing logic{0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name);
                        listCollection = null;
                        ConfigurationFactory.RemoveCache<webParams>(PARASECTION_CONFIGNAME);
                        GetListGroup();
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
                    configurationFilePath = SystemConfig.GlobalAppSettings[PARASECTION_CONFIGNAME];
                    configurationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configurationFilePath);
                }
                return configurationFilePath;
            }
        }
        private  static webParams ConfigList
        {
            get
            {
                webParams listConfig = ConfigurationFactory.GetConfigurationItem<webParams>(PARASECTION_CONFIGNAME, ConfigurationFilePath);
                return listConfig;
            }
        } 
        public static Dictionary<string, string> ParaCollection
        {
            get
            {
                if(listCollection==null || listCollection.Count==0)
                {
                    GetListGroup();
                }
                return listCollection;
            }
        }
        private static Dictionary<string, string> listCollection = new Dictionary<string, string>();

        /// <summary>
        /// 获取所有分组
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetListGroup()
        {
            lock (LockObject)
            {
                if (listCollection == null || listCollection.Count == 0)
                {
                    foreach (var list in ConfigList.Items)
                    {
                        if (!listCollection.ContainsKey(list.key))
                        {
                            listCollection.Add(list.key, list.value);
                        }
                    }
                }
                return listCollection;
            }
        }
        /// <summary>
        /// 根据Key获取
        /// </summary>
        /// <returns></returns>
        public static string GetParameterValue(string key)
        {
            return ParaCollection[key];
        }
    }
}
