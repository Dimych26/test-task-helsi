using MongoDB.Driver;

namespace Helsi.Test_Task.Core;
public interface IMongoRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAll();

    Task<TEntity> GetById(string id);

    Task<List<TEntity>> GetByFilter(FilterDefinition<TEntity> filter);

    Task Add(TEntity entity);

    Task AddRange(IEnumerable<TEntity> entities);

    Task Update(TEntity entity);

    Task Delete(TEntity entity);

    IMongoCollection<TEntity> GetCollection();
}
