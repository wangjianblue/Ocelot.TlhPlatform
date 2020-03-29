using System;
using MongoDB.Bson.Serialization.Attributes;

namespace TlhPlatform.Infrastructure.MongoDB.Base
{
    public class MongoEntity
    {
        public MongoEntity()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        [BsonElement("_id")]
        public string Id { get; set; }
    }
}
