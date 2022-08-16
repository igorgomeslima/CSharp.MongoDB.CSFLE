using MongoDB.Bson;
using Sample.Worker.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Sample.Worker.Persistence.Mappings
{
    public static class FooMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Foo>(map =>
            {
                map.AutoMap();
                map.IdMemberMap.SetSerializer(new StringSerializer(BsonType.ObjectId));
                map.MapIdMember(mId => mId.Id).SetIdGenerator(StringObjectIdGenerator.Instance);
            });
        }
    }
}