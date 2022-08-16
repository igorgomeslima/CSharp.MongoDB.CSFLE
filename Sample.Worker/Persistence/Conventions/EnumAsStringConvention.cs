using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace Sample.Worker.Persistence.Conventions
{
    public class EnumAsStringConvention
    {
        public static void Register()
        {
            var pack = new ConventionPack{ new EnumRepresentationConvention(BsonType.String) };

            ConventionRegistry.Register("EnumStringConvention", pack, t => true);
        }
    }
}
