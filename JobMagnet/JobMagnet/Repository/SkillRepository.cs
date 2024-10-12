using JobMagnet.Context;
using JobMagnet.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Repository
{
    public class SkillRepository<TEntity> : ISkillRepository<TEntity> where TEntity : class
    {
        private readonly JobMagnetDbContext dbContext;
        private DbSet<TEntity> dbSet;

        public SkillRepository(JobMagnetDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}
