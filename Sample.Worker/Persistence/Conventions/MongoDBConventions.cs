namespace Sample.Worker.Persistence.Conventions
{
    public static class MongoDBConventions
    {
        public static void Configure()
        {
            CamelCaseConvention.Register();
            IgnoreNullConvention.Register();
            EnumAsStringConvention.Register();
        }
    }
}
