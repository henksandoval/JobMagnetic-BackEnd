using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Repositories.Implements;

public class ServiceQueryRepository(JobMagnetDbContext dbContext)
    : Repository<ServiceEntity, long>(dbContext), IServiceQueryRepository
{
    private IQueryable<ServiceEntity> _query = dbContext.Set<ServiceEntity>();

    public ServiceQueryRepository IncludeGalleryItems()
    {
        _query = _query.Include(p => p.GalleryItems);
        return this;
    }

    public async Task<IReadOnlyCollection<ServiceEntity>> GetAllWithIncludesAsync()
    {
        return await _query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<ServiceEntity?> GetByIdWithIncludesAsync(long id)
    {
        return await _query.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
    }
}