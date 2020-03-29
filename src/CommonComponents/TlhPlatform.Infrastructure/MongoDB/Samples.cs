using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using TlhPlatform.Infrastructure.MongoDB;
using TlhPlatform.Infrastructure.MongoDB.Base;

namespace TlhPlatform.Infrastructure.MongoDB
{
    class Samples
    {
        void SamplesMethod()
        {
            
            //从配置文件加载MongoOption
            MongoOption option = new MongoOption() { ConnectionString = "mongodb://10.1.20.143:27017", DatabaseName="DB1" };
            var mongoRepository = new MongoRepository(option);
            var u = new User
            {
                Name = "username",
                BirthDateTime = new DateTime(1991, 2, 2),
                AddressList = new List<string> { "shanxi", "xian" },
                Sex = 1,
                Son = new User
                {
                    Name = "user01",
                    BirthDateTime = DateTime.Now
                }
            };

            var addresult = mongoRepository.Add(u);

            var upResulr = mongoRepository.GetAndUpdate<User>(a => a.Id == u.Id, a => new User { Sex = 3 });

            var getResult = mongoRepository.Get<User>(a => a.Id == u.Id);
            getResult.Name = "username2";

            mongoRepository.Update(a => a.Id == getResult.Id, getResult);

            mongoRepository.Update<User>(a => a.Id == u.Id, a => new User { AddressList = new List<string> { "shanxi", "xian", "yanta" } });

            mongoRepository.Exists<User>(a => a.Id == u.Id);

            mongoRepository.Delete<User>(a => a.Id == u.Id);


        }
    }

    public class User : MongoEntity
    {
        public string Name { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime BirthDateTime { get; set; }

        public User Son { get; set; }

        public int Sex { get; set; }

        public List<string> AddressList { get; set; }
    }
}
