namespace JobMagnet.Domain.Ports.Repositories.Base;

public interface ICommandRepository<in TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void HardDelete(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}