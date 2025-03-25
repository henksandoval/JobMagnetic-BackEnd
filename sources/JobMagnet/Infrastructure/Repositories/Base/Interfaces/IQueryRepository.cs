using System.Linq.Expressions;

namespace JobMagnet.Infrastructure.Repositories.Base.Interfaces;

public interface IQueryRepository<TEntity, in TKey> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<ICollection<TEntity>> GetAllAsync();
    Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> CountAsync();
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
}