
using Helsi.Test_Task.Core;
using Helsi.Test_Task.MongoDb.Settings;
using Helsi.Test_Task.Persistens.EF;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AdSaver.Persistens.EF;

public class MongoUnitOfWork : IMongoUnitOfWork // якщо треба буде використати інше data source, буде створено інший UnitOfWork по цьому ж принципу
{
    private readonly Dictionary<string, object> _repositories = new();
    private readonly IMongoDatabase _database;

    public MongoUnitOfWork(IOptions<MongoDbSettings> options)
    {
        var settings = options.Value;
        _database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
    }

    public IMongoRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        var entityTypeName = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(entityTypeName))
        {
            var repository = CreateRepository<TEntity>();
            _repositories.Add(entityTypeName, repository);
        }

        return (IMongoRepository<TEntity>)_repositories[entityTypeName];
    }

    protected virtual IMongoRepository<TEntity> CreateRepository<TEntity>() 
        where TEntity : class
    {
        return new MongoRepository<TEntity>(_database.GetCollection<TEntity>(typeof(TEntity).Name));
    }

    public int SaveChanges()
    {
        return 0;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(0);
    }
}
