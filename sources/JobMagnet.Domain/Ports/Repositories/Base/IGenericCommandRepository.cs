namespace JobMagnet.Domain.Ports.Repositories.Base;

public interface IGenericCommandRepository<in TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    IGenericCommandRepository<TEntity> Update(TEntity entity);
    IGenericCommandRepository<TEntity> HardDelete(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}