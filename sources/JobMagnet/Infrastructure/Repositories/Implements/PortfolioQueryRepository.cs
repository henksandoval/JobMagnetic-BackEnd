using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Repositories.Implements;

public class PortfolioQueryRepository(JobMagnetDbContext dbContext)
    : Repository<PortfolioGalleryEntity, long>(dbContext), IPortfolioQueryRepository
{
    private IQueryable<PortfolioGalleryEntity> _query = dbContext.Set<PortfolioGalleryEntity>();

    public IPortfolioQueryRepository IncludeGalleryItems()
    {
        _query = _query.Include(p => p.GalleryItems);
        return this;
    }

    public async Task<IReadOnlyCollection<PortfolioGalleryEntity>> GetAllWithIncludesAsync()
    {
        return await _query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<PortfolioGalleryEntity?> GetByIdWithIncludesAsync(long id)
    {
        return await _query.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
    }
}