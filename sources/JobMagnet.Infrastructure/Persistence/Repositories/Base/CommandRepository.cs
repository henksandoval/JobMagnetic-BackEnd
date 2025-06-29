using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Repositories.Base;

public class CommandRepository<TEntity>(JobMagnetDbContext dbContext) : ICommandRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}