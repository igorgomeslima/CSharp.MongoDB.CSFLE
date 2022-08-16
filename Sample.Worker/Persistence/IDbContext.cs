using MongoDB.Driver;
using Sample.Worker.Models;
using Sample.Worker.Persistence.Encryption.Models;
using Sample.Worker.Persistence.Encryption.Helpers;

namespace Sample.Worker.Persistence
{
    public interface IDbContext
    {
        public IMongoCollection<Foo> Foo { get; }
    }
    
    internal class DbContext : IDbContext
    {
        private readonly IMongoDatabase _mongoDatabase;

        public DbContext(IMongoDBHelper mongoDBHelper)
        {
            var mongoDBClientWithAutoEncrypting = mongoDBHelper.GetMongoDBClientWithAutoEncrypting(KmsProvider.Local);

            _mongoDatabase = mongoDBClientWithAutoEncrypting.GetDatabase(mongoDBHelper.DatabaseName);
        }

        private IMongoCollection<Foo> _foo;

        public IMongoCollection<Foo> Foo => _foo ??= _mongoDatabase.GetCollection<Foo>(nameof(Foo).ToLower());
    }
}