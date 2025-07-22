namespace JobMagnet.Domain.Ports.Repositories.Base;

public interface ICommandRepository<in TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entity);
    void Remove(TEntity entity);
}