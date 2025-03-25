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
        var entity = await _dbSet.FindAsync(id);
        return entity;
    }

    public async Task CreateAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        if (!_isTransactional)
            await dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        if (!_isTransactional)
            return await dbContext.SaveChangesAsync() > 0;

        return false;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        _isTransactional = true;
        return await dbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync(IDbContextTransaction transaction)
    {
        try
        {
            if (_isTransactional) await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }

    public async Task<bool?> HardDeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        if (!_isTransactional)
            return await dbContext.SaveChangesAsync() > 0;

        return false;
    }
}