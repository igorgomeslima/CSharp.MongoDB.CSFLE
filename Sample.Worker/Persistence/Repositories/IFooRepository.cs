using System;
using MongoDB.Driver;
using System.Threading;
using System.Text.Json;
using Sample.Worker.Models;
using System.Threading.Tasks;
using Sample.Worker.Persistence.Extensions;

namespace Sample.Worker.Persistence.Repositories
{
    public interface IFooRepository
    {
        Task<string> CreateFoo(Foo foo, CancellationToken cancellationToken);
        Task<bool> UpsertBar(string fooId, Bar bar, CancellationToken cancellationToken);
    }

    public class FooRepository : IFooRepository
    {
        readonly IMongoCollection<Foo> _fooCollection;

        public FooRepository(IDbContext dbContext)
        {
            _fooCollection = dbContext.Foo;
        }
        
        public async Task<string> CreateFoo(Foo foo, CancellationToken cancellationToken)
        {
            await _fooCollection.InsertOneAsync(foo, cancellationToken: cancellationToken);

            return foo.Id;
        }
    
        public async Task<bool> UpsertBar(string fooId, Bar bar, CancellationToken cancellationToken)
        {
            var filterBuilder = Builders<Foo>.Filter;

            var filter = filterBuilder.Eq(_ => _.Id, fooId) & filterBuilder.ElemMatch(foo => foo.Bars, b => b.Name == bar.Name);

            var update = Builders<Foo>.Update.Set(foo => foo.Bars[-1], bar);

            var updateResult = await _fooCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
            
            Console.WriteLine(JsonSerializer.Serialize(updateResult));
	
            return updateResult.IsSuccess();
        }
    }
}