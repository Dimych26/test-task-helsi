using System.Linq.Expressions;

namespace Helsi.Test_Task.Core;
public interface IRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAll();

    Task<List<TEntity>> GetByFilter(Expression<Func<TEntity, bool>> filter = null);

    Task Add(TEntity entity);

    Task AddRange(IEnumerable<TEntity> entities);

    Task Update(TEntity entity);

    Task Delete(TEntity entity);
}
