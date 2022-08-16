using MongoDB.Bson.Serialization.Conventions;

namespace Sample.Worker.Persistence.Conventions
{
    public class CamelCaseConvention
    {
        public static void Register()
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);
        }
    }
}
