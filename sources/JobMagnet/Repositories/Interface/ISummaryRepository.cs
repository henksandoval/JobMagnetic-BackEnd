namespace JobMagnet.Repositories.Interface;

public interface ISummaryRepository<TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(int id);
}