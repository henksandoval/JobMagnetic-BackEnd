using JobMagnet.Context;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly JobMagnetDbContext dbContext;
        private readonly DbSet<TEntity> dbSet;

        public Repository(JobMagnetDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await dbSet.FirstAsync();
            return entity;
        }
    }
}
