namespace JobMagnet.Infrastructure.Repositories.Base.Interfaces;

public interface IQueryRepository<TEntity, in TKey> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TKey id);
}