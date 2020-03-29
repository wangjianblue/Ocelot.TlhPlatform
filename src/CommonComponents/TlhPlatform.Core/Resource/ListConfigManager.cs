/*************************************************
Description:Read List Config From COnfigfile
CreateUser:fwq
CreateDate:
ChangeHistory:
[User]-[yyyy/mm/dd]-[Remark]

**************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using TlhPlatform.Core.ConfigurationClass;
using TlhPlatform.Core.Utility;
namespace TlhPlatform.Core.Resource
{
    /// <summary>
    /// 列表集合
    /// </summary>
    public static class ListConfigManager
    {
        private const string LiftSECTION_CONFIGNAME = "ListConfigFile";
        private static string configurationFilePath = string.Empty;
        private static object  LockObject = new object();
        
        static ListConfigManager()
        {

            var filePath = Path.GetDirectoryName(ConfigurationFilePath);
            var delayFileSystemWatcher = new DelayFileSystemWatcher(filePath, "*.*", (object sender, FileSystemEventArgs e) =>
            {
                switch (e.ChangeType)
                {
                    case WatcherChangeTypes.Changed:
                        Console.WriteLine("File change event processing logic{0}  {1}  {2}", e.ChangeType, e.FullPath, e.Name);
                        listCollection = null;
                        ConfigurationFactory.RemoveCache<ListConfig>(LiftSECTION_CONFIGNAME);
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
                    configurationFilePath = SystemConfig.GlobalAppSettings[LiftSECTION_CONFIGNAME];
                    configurationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configurationFilePath);
                }
                return configurationFilePath;
            }
        }
        public static ListConfig ConfigList
        {
            get
            {
                ListConfig listConfig = ConfigurationFactory.GetConfigurationItem<ListConfig>(LiftSECTION_CONFIGNAME, ConfigurationFilePath);
                return listConfig;
            }
        }

        private static Dictionary<string, ListGroup> listCollection = new Dictionary<string, ListGroup>();

        /// <summary>
        ///获取所有列表
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, ListGroup> GetListGroup()
        {
            lock (LockObject)
            {
                if (listCollection == null || listCollection.Count == 0)
                {
                    foreach (var list in ConfigList.Items)
                    {
                        if (!listCollection.ContainsKey(list.Name))
                        {
                            listCollection.Add(list.Name, list);
                        }
                    }
                }
                return listCollection;
            }
        }
        /// <summary>
        /// 获取某一个列表
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public static  ListItem[] GetListItem(ListName listName)
        {
            if(listCollection==null || listCollection.Count==0)
            {
                GetListGroup();
            }
            return listCollection[Enum.GetName(typeof (ListName), listName)].listItem;
        }
    }
}
