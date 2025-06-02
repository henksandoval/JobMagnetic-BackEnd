namespace JobMagnet.Domain.Ports.Repositories.Base;

public interface ICommandRepository<in TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool?> HardDeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}