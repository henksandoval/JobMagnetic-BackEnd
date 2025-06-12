using System.Linq.Expressions;

namespace JobMagnet.Domain.Ports.Repositories.Base;

public interface IQueryRepository<TEntity, in TKey> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TEntity>> GetAllAsync();
    Task<IReadOnlyCollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<int> CountAsync();
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
}