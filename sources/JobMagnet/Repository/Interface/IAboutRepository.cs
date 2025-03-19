namespace JobMagnet.Repository.Interface;

public interface IAboutRepository<TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(int id);
}