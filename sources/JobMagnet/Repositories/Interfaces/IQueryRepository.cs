namespace JobMagnet.Repositories.Interfaces;

public interface IQueryRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id);
}