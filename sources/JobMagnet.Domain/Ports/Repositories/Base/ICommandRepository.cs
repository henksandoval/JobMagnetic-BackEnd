namespace JobMagnet.Domain.Ports.Repositories.Base;

public interface ICommandRepository<in TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool?> HardDeleteAsync(TEntity entity);
}