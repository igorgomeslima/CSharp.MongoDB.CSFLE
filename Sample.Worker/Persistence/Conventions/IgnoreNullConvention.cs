using MongoDB.Bson.Serialization.Conventions;

namespace Sample.Worker.Persistence.Conventions
{
    public class IgnoreNullConvention
    {
        public static void Register()
        {
            ConventionRegistry.Register("Ignore", new ConventionPack { new IgnoreIfNullConvention(true) }, _ => true);
        }
    }
}
