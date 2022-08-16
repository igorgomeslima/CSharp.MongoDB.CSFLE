using Sample.Worker.Persistence;
using Microsoft.Extensions.Hosting;
using Sample.Worker.Persistence.Mappings;
using Sample.Worker.Persistence.Conventions;
using Sample.Worker.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sample.Worker.Persistence.Encryption.Helpers;

namespace Sample.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IFooRepository, FooRepository>();
                    services.AddSingleton<IDbContext, DbContext>();
                    services.AddSingleton<IMongoDBHelper, MongoDBHelper>();
                    
                    MongoDBConventions.Configure();
                    MongoDBMappings.Configure();
                    
                    services.AddHostedService<Worker>();
                });
    }
}