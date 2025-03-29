using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base;
using JobMagnet.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobMagnet.Infrastructure.Repositories.Implements;

public class PortfolioQueryRepository(JobMagnetDbContext dbContext)
    : Repository<PortfolioEntity, long>(dbContext), IPortfolioQueryRepository
{
    private IQueryable<PortfolioEntity> _query = dbContext.Set<PortfolioEntity>();

    public PortfolioQueryRepository IncludeGalleryItems()
    {
        _query = _query.Include(p => p.GalleryItems);
        return this;
    }

    public async Task<IReadOnlyCollection<PortfolioEntity>> GetAllWithIncludesAsync()
    {
        return await _query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<PortfolioEntity?> GetByIdWithIncludesAsync(long id)
    {
        return await _query.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
    }
}