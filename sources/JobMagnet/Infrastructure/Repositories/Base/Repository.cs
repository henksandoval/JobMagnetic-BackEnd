using System.Linq.Expressions;
using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobMagnet.Infrastructure.Repositories.Base;

public class Repository<TEntity, TKey>(JobMagnetDbContext dbContext)
    : IQueryRepository<TEntity, TKey>, ICommandRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();
    private bool _isTransactional;

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

    public async Task CreateAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity).ConfigureAwait(false);
        if (!_isTransactional)
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        if (!_isTransactional)
            return await dbContext.SaveChangesAsync().ConfigureAwait(false) > 0;

        return false;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        _isTransactional = true;
        return await dbContext.Database.BeginTransactionAsync().ConfigureAwait(false);
    }

    public async Task CommitAsync(IDbContextTransaction transaction)
    {
        try
        {
            if (_isTransactional) await transaction.CommitAsync().ConfigureAwait(false);
        }
        catch
        {
            await transaction.RollbackAsync().ConfigureAwait(false);
            throw;
        }
        finally
        {
            await transaction.DisposeAsync().ConfigureAwait(false);
        }
    }

    public async Task<bool?> HardDeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        if (!_isTransactional)
            return await dbContext.SaveChangesAsync().ConfigureAwait(false) > 0;

        return false;
    }
}