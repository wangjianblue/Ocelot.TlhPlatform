using System;

namespace TlhPlatform.Infrastructure.MongoDB
{
    public class MongoDB
    {
        public MongoOption MongoOption1 { get; set; }
        public MongoOption MongoOption2 { get; set; } 
    }

    /// <summary>
    /// Mongo参数配置实体（从配置文件加载）
    /// </summary>
    public class MongoOption
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }

   
}