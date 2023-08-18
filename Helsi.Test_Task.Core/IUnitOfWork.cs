
namespace Helsi.Test_Task.Core;
public interface IUnitOfWork
{
    IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : class;

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
