using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Persistence.Repositories;

public class ServiceQueryRepository(JobMagnetDbContext dbContext)
    : Repository<ServiceEntity, long>(dbContext), IServiceQueryRepository
{
    private IQueryable<ServiceEntity> _query = dbContext.Set<ServiceEntity>();

    public IServiceQueryRepository IncludeGalleryItems()
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