namespace JobMagnet.Domain.Ports.Repositories.Base;

public interface ICommandRepository<in TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    ICommandRepository<TEntity> Update(TEntity entity);
    ICommandRepository<TEntity> HardDelete(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}