
using Helsi.Test_Task.Core;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Helsi.Test_Task.Persistens.EF;

public class MongoRepository<TEntity> : IMongoRepository<TEntity>
    where TEntity : class
{
    private readonly IMongoCollection<TEntity> _collection;

    public MongoRepository(IMongoCollection<TEntity> collection)
    {
        _collection = collection;
    }

    public IMongoCollection<TEntity> GetCollection()
    {
        return _collection;
    }

    public Task<List<TEntity>> GetAll()
    {
        return _collection.Find(new BsonDocument()).ToListAsync();
    }

    public Task<TEntity> GetById(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", ObjectId.Parse(id));
        return _collection.Find(filter).FirstOrDefaultAsync();
    }

    public Task<List<TEntity>> GetByFilter(FilterDefinition<TEntity> filter)
    {
        return _collection.Find(filter).ToListAsync();
    }

    public Task Add(TEntity entity)
    {
        return _collection.InsertOneAsync(entity);
    }

    public Task AddRange(IEnumerable<TEntity> entities)
    {
        return _collection.InsertManyAsync(entities);
    }

    public Task Update(TEntity entity)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", GetId(entity));
        return _collection.ReplaceOneAsync(filter, entity);
    }

    public Task Delete(TEntity entity)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", GetId(entity));
        return _collection.DeleteOneAsync(filter);
    }

    private static ObjectId GetId(TEntity entity)
    {
        var property = typeof(TEntity).GetProperty("Id");
        var id = property?.GetValue(entity)?.ToString();

        if (!ObjectId.TryParse(id, out var objectId))
        {
            objectId = ObjectId.Empty;
        }

        return objectId;
    }
}
