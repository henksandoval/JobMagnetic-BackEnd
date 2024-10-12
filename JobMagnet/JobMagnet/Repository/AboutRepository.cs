﻿using JobMagnet.Context;
using JobMagnet.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Repository
{
    public class AboutRepository<TEntity> : IAboutRepository<TEntity> where TEntity : class
    {
        private readonly JobMagnetDbContext dbContext;
        private readonly DbSet<TEntity> dbSet;

        public AboutRepository(JobMagnetDbContext dbContext)
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
