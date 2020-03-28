using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using WebApplication1.Models.Cache;

namespace TlhPlatform.Infrastructure.Cache.Key
{
    public interface IKeyManager
    {
        /// <summary>
        /// 读取RedisFile 文件
        /// </summary>
        void ReaderKeyFile();


        /// <summary>
        /// 变更事件会触发两次是正常情况，是系统保存文件机制导致
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        void OnChanged(object source, FileSystemEventArgs e);


        /// <summary>
        /// 根据KeyName获取Key配置对象 /// </summary>
        /// <param name="name">Key名称</param>
        /// <returns></returns>
        KeyEntity Get<T>(T name);


        /// <summary>
        /// 根据KeyName获取Key配置对象 /// </summary>
        /// <param name="name">Key名称</param>
        /// <param name="identities">Key标识(用于替换Key中的{0}占位符)</param>
        /// <returns></returns>
        KeyEntity Get<T>(T name, params string[] identities);

    }
}
