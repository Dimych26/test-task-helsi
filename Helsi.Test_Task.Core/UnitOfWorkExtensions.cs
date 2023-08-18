using MongoDB.Bson;
using MongoDB.Driver;

namespace Helsi.Test_Task.Core;

public static class UnitOfWorkExtensions
{
    public static IMongoCollection<TEntity> GetCollection<TEntity>(this IMongoUnitOfWork unitOfWork)
        where TEntity : class
    {
        return unitOfWork.GetRepository<TEntity>().GetCollection();
    }

    public static Task<List<TEntity>> GetAllAsync<TEntity>(this IMongoUnitOfWork unitOfWork)
        where TEntity : class
    {
        return unitOfWork.GetRepository<TEntity>().GetAll();
    }

    public static Task<List<TEntity>> GetByFilterAsync<TEntity>(this IMongoUnitOfWork unitOfWork, FilterDefinition<TEntity> filter)
        where TEntity : class
    {
        return unitOfWork.GetRepository<TEntity>().GetByFilter(filter);
    }

    public static Task<TEntity> GetByIdAsync<TEntity>(this IMongoUnitOfWork unitOfWork, string id)
        where TEntity : class
    {
        return unitOfWork.GetRepository<TEntity>().GetById(id);
    }

    public static Task AddAsync<TEntity>(this IMongoUnitOfWork unitOfWork, TEntity entity)
        where TEntity : class
    {
        return unitOfWork.GetRepository<TEntity>().Add(entity);
    }

    public static Task AddRangeAsync<TEntity>(this IMongoUnitOfWork unitOfWork, IEnumerable<TEntity> entities)
        where TEntity : class
    {
        return unitOfWork.GetRepository<TEntity>().AddRange(entities);
    }

    public static Task UpdateAsync<TEntity>(this IMongoUnitOfWork unitOfWork, TEntity entity)
        where TEntity : class
    {
        return unitOfWork.GetRepository<TEntity>().Update(entity);
    }

    public static Task DeleteAsync<TEntity>(this IMongoUnitOfWork unitOfWork, TEntity entity)
        where TEntity : class
    {
        return unitOfWork.GetRepository<TEntity>().Delete(entity);
    }
}