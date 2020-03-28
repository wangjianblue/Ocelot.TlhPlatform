using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using TlhPlatform.Infrastructure.Cache.Key;
using WebApplication1.Models.Cache;

namespace TlhPlatform.Infrastructure.Cache.Key
{
    public class KeyManager : IKeyManager
    {
        //KeyName集合
        private Hashtable _keyNameList; //锁对象
        private readonly object objLock = new object(); //监控文件对象
        private readonly KeyOptions _option = null;
        /// <summary>
        /// 静态构造只执行一次 /// </summary>
        public KeyManager(Action<KeyOptions> options)
        {
            _option = new KeyOptions();
            options(_option);
            //创建对配置文件夹的监听，如果遇到文件更改则清空KeyNameList，重新读取
            var watcher = new FileSystemWatcher();
            watcher.Path = _option.FilePath; //监听路径
            watcher.Filter = _option.FileName; //监听文件名
            watcher.NotifyFilter =
                NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.Size; //仅监听文件创建时间、文件变更时间、文件大小
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true; //最后开启监听
        }

        /// <summary>
        /// 读取KeyName文件 /// </summary>
        public void ReaderKeyFile()
        {
            if (_keyNameList == null || _keyNameList.Count == 0)
            {
                //锁定读取xml操作
                lock (objLock)
                {
                    //获取配置文件
                    string configFile = String.Concat(_option.FilePath, _option.FileName); //检查文件
                    if (!File.Exists(configFile))
                    {
                        throw new FileNotFoundException(String.Concat("file not exists:", configFile));
                    } //读取xml文件

                    XmlReaderSettings xmlSetting = new XmlReaderSettings();
                    xmlSetting.IgnoreComments = true; //忽略注释
                    XmlReader xmlReader = XmlReader.Create(configFile, xmlSetting); //一次读完整个文档
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlReader);
                    xmlReader.Close(); //关闭读取对象 //获取指定节点下的所有子节点
                    XmlNodeList nodeList = xmlDoc.SelectSingleNode("//configuration//list")?.ChildNodes; //获得一个线程安全的Hashtable对象
                    _keyNameList = Hashtable.Synchronized(new Hashtable()); //将xml中的属性赋值给Hashtable
                    if (nodeList != null)
                        foreach (XmlNode node in nodeList)
                        {
                            XmlElement element = (XmlElement)node; //转为元素获取属性
                            KeyEntity entity = new KeyEntity();
                            entity.Name = element.GetAttribute("name");
                            entity.Key = element.GetAttribute("key");
                            entity.ValidTime = Convert.ToInt32(element.GetAttribute("validTime"));
                            entity.Enabled = Convert.ToBoolean(element.GetAttribute("enabled"));

                            _keyNameList.Add(entity.Name, entity);
                        }
                }
            }
        }

        /// <summary>
        /// 变更事件会触发两次是正常情况，是系统保存文件机制导致
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                if (e.Name.ToLower() == _option.FileName.ToLower())
                {
                    _keyNameList =
                        null; //因为此事件会被调用两次，所以里面的代码要有幕等性，如果无法实现幕等性， //则应该在Init()中绑定事件 //watcher.Changed += new FileSystemEventHandler(OnChanged); //在OnChanged()事件中解绑事件 //watcher.Changed -= new FileSystemEventHandler(OnChanged);
                }
            }
        }

        /// <summary>
        /// 根据KeyName获取Key配置对象 /// </summary>
        /// <param name="name">Key名称</param>
        /// <returns></returns>
        public KeyEntity Get<T>(T name)
        {
            return Get(name, null);
        }

        /// <summary>
        /// 根据KeyName获取Key配置对象 /// </summary>
        /// <param name="name">Key名称</param>
        /// <param name="identities">Key标识(用于替换Key中的{0}占位符)</param>
        /// <returns></returns>
        public KeyEntity Get<T>(T name, params string[] identities)
        {
            //检查Hash中是否有值
            if (_keyNameList == null || _keyNameList.Count == 0)
                ReaderKeyFile(); //检查Hash中是否有此Key
            string tmpName = name.ToString().ToLower();
            if (_keyNameList != null && !_keyNameList.ContainsKey(tmpName))
                throw new ArgumentException("keyNameList中不存在此KeyName", "name");
            if (_keyNameList != null)
            {
                var entity = _keyNameList[tmpName] as KeyEntity; //检查Key是否需要含有占位符
                if (entity != null && entity.Key.IndexOf('{') > 0)
                {
                    //检查参数数组是否有值
                    if (identities != null && identities.Length > 0)
                        entity.Key = String.Format(entity.Key, identities);
                    else
                        throw new ArgumentException("需要此参数identities标识字段，但并未传递", "identities");
                }
                return entity;
            }

            return null;
        }

    }

}