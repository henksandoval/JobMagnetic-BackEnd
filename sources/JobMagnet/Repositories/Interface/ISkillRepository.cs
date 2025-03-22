namespace JobMagnet.Repositories.Interface;

public interface ISkillRepository<TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(int id);
}