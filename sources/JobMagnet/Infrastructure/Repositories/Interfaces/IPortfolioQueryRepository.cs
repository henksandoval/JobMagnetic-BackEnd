using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;

namespace JobMagnet.Infrastructure.Repositories.Interfaces;

public interface IPortfolioQueryRepository : IQueryRepository<PortfolioGalleryEntity, long>
{
    IPortfolioQueryRepository IncludeGalleryItems();
    Task<IReadOnlyCollection<PortfolioGalleryEntity>> GetAllWithIncludesAsync();
    Task<PortfolioGalleryEntity?> GetByIdWithIncludesAsync(long id);
}