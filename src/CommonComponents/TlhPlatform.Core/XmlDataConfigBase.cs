using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using TlhPlatform.Core.Utility;

namespace TlhPlatform.Core
{
    /// <summary>
    ///将配置文件反序列化为运行时对象，并监视配置文件和
    ///也反映了对运行时对象的任何更改。
    /// </ summary>
    /// <备注>
    ///扩展器的注意事项：
    /// 未来的工作：
    ///目前，配置管理器依赖Cache来管理配置对象。
    ///未使用MS企业库缓存组件，因为filedependecy每次都会检查文件的日期
    ///访问缓存项，从而施加过多的IO操作。
    /// Cache支持许多功能，因此在这里使用。然而，这限制了
    ///此组件的未来可伸缩性和使用方案导致消费者必须依赖
    /// 
    ///解决方案：为使用FileSystemWatcher的MS企业库实现custom filedependy
    ///获取文件更改的通知，而不是主动检查文件日期，从而减少IO操作。
    /// </备注>
    public class XmlDataConfigBase
    {

        private readonly IMemoryCache m_CacheManager;
        private const string LogCategory = "CacheItemRemoved";
        #region exception class for loading configuration file.

        [Serializable]
        public class LoadFileException : ApplicationException
        {
            private string m_FileName;
            private string m_TypeName;

            public LoadFileException(string typeName, string fileName)
            {
                m_FileName = fileName;
                m_TypeName = typeName;
            }

            public override string Message
            {
                get
                {
                    return string.Format("Unable to load file {0} for type {1}", m_FileName, m_TypeName);
                }
            }
        }
        #endregion

        private object m_SyncObject;

        public XmlDataConfigBase()
        {
            m_SyncObject = new object();

            m_CacheManager = new MemoryCache(new MemoryCacheOptions());


        }



        #region cache manipulation
        /// <summary>
        ///如果序列化失败，则抛出异常。
        /// </ summary>
        /// <typeparam name =“T”> </ typeparam>
        /// <param name =“key”> </ param>
        /// <param name =“configFilePath”> </ param>
        /// <param name =“needLog”> </ param>
        /// <returns> </ returns>
        /// <exception cref =“LoadFileException”>当配置文件无法加载</ exception>时
        private T LoadConfiguration<T>(string key, string configFilePath, bool needLog)
            where T : class
        {
            T config;

            config = ObjectXmlSerializer.LoadFromXml<T>(configFilePath, needLog);
            if (config != null)
            {
                AddToCache(key, config, configFilePath, needLog);
            }
            else
            {
                throw new LoadFileException(typeof(T).Name, configFilePath);
            }

            return config;
        }

        /// <summary>
        ///将配置添加到缓存
        /// </ summary>
        /// <param name =“key”>在web.config中定义的部分名称</ param>
        /// <param name =“value”>配置对象</ param>
        /// <param name =“depedencyFile”>配置文件</ param>
        /// <param name =“depedencyFile”>需要Log </ param>
        private void AddToCache(string key, object value, string depedencyFile, bool needLog)
        {
            m_CacheManager.Set(key, value);
        }

        /// <summary>
        ///从缓存中获取配置对象。 如果基础文件发生更改，则将重新加载该对象。
        /// </ summary>
        /// <typeparam name =“T”>配置对象的类型</ typeparam>
        /// <param name =“key”>配置对象的文件名</ param>
        /// <param name =“depedencyFile”>需要Log </ param>
        /// <param name =“filePath”>如果数据不在缓存中，必须重新加载，那么提供配置文件Paht </ param>
        /// <returns>配置对象</ returns>
        /// <exception cref =“LoadFileException”>当配置文件无法加载</ exception>时
        public T GetFromCache<T>(string key, string filePath, bool needLog) where T : class
        {
            T res = m_CacheManager.Get<T>(key);
            if (res == null)
            {
                lock (m_SyncObject)
                {
                    res = m_CacheManager.Get<T>(key);
                    if (res == null)
                    {
                        try
                        {
                            res = LoadConfiguration<T>(key, filePath, needLog);
                        }
                        catch (LoadFileException)
                        {
                            res = null;
                        }
                    }
                }
            }
            return res;
        }

        public void RemoveCache<T>(string key) where T : class
        {
            m_CacheManager.Remove(key);
        }

        /// <summary>
        ///从缓存中获取配置对象。 如果基础文件发生更改，则将重新加载该对象。
        /// </ summary>
        /// <typeparam name =“T”>配置对象的类型</ typeparam>
        /// <param name =“key”>配置对象的文件名</ param>
        /// <param name =“depedencyFile”>需要Log </ param>
        /// <returns>配置对象</ returns>
        /// <exception cref =“LoadFileException”>当配置文件无法加载</ exception>时
        public T GetFromCacheByFullPath<T>(string key, string filePath, bool needLog) where T : class
        {
            T res = m_CacheManager.Get(key) as T;
            if (res == null)
            {
                lock (m_SyncObject)
                {
                    res = m_CacheManager.Get(key) as T;
                    if (res == null)
                    {
                        //string configFile ;
                        if (!File.Exists(filePath))
                        {
                            try
                            {
                                res = LoadConfiguration<T>(key, filePath, needLog);
                            }
                            catch (LoadFileException)
                            {
                                res = null;
                            }
                        }
                    }
                }
            }
            return res;
        }

        public T GetFromCache<T>(string key, string filePath) where T : class
        {
            return GetFromCache<T>(key, filePath, true);
        }

        public T GetFromCacheByFullPath<T>(string key, string filePath) where T : class
        {
            return GetFromCacheByFullPath<T>(key, filePath, true);
        }
        #endregion // End of cache manipulation



    }
}
