using JobMagnet.Context;
using JobMagnet.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Repositories;

public class AboutRepository<TEntity> : IAboutRepository<TEntity> where TEntity : class
{
    private readonly JobMagnetDbContext _dbContext;
    private readonly DbSet<TEntity> dbSet;

    public AboutRepository(JobMagnetDbContext dbContext)
    {
        _dbContext = dbContext;
        dbSet = dbContext.Set<TEntity>();
    }

    public async Task CreateAsync(TEntity entity)
    {
        await dbSet.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await dbSet.FindAsync(id);
    }
}