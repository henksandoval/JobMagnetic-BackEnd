using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;

namespace JobMagnet.Infrastructure.Repositories.Interfaces;

public interface IPortfolioQueryRepository : IQueryRepository<PortfolioEntity, long>
{
    IPortfolioQueryRepository IncludeGalleryItems();
    Task<IReadOnlyCollection<PortfolioEntity>> GetAllWithIncludesAsync();
    Task<PortfolioEntity?> GetByIdWithIncludesAsync(long id);
}