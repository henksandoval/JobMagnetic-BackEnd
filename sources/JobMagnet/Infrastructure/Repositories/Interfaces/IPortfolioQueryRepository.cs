using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Implements;

namespace JobMagnet.Infrastructure.Repositories.Interfaces;

public interface IPortfolioQueryRepository : IQueryRepository<PortfolioEntity, long>
{
    PortfolioQueryRepository IncludeGalleryItems();
    Task<IReadOnlyCollection<PortfolioEntity>> GetAllWithIncludesAsync();
    Task<PortfolioEntity?> GetByIdWithIncludesAsync(long id);
}