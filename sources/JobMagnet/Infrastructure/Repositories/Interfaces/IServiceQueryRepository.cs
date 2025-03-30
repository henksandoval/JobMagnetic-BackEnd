using JobMagnet.Infrastructure.Entities;
using JobMagnet.Infrastructure.Repositories.Base.Interfaces;
using JobMagnet.Infrastructure.Repositories.Implements;

namespace JobMagnet.Infrastructure.Repositories.Interfaces;

public interface IServiceQueryRepository : IQueryRepository<ServiceEntity, long>
{
    ServiceQueryRepository IncludeGalleryItems();
    Task<IReadOnlyCollection<ServiceEntity>> GetAllWithIncludesAsync();
    Task<ServiceEntity?> GetByIdWithIncludesAsync(long id);
}