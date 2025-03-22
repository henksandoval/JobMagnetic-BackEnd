namespace JobMagnet.Repositories.Interface;

public interface IQueryRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id);
}