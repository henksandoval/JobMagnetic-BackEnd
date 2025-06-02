using System.Linq.Expressions;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Repositories.Base;

public class Repository<TEntity, TKey>(JobMagnetDbContext dbContext)
    : IQueryRepository<TEntity, TKey>, ICommandRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task CreateAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity).ConfigureAwait(false);
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        return await dbContext.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<bool?> HardDeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await dbContext.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        var entity = await _dbSet.FindAsync(id).ConfigureAwait(false);
        return entity;
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync().ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync().ConfigureAwait(false);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate).ConfigureAwait(false);
    }

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync().ConfigureAwait(false);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate).ConfigureAwait(false);
    }
}