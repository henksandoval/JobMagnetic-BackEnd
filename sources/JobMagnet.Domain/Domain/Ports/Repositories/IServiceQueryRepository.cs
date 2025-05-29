using JobMagnet.Domain.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Entities;

namespace JobMagnet.Domain.Domain.Ports.Repositories;

public interface IServiceQueryRepository : IQueryRepository<ServiceEntity, long>
{
    IServiceQueryRepository IncludeGalleryItems();
    Task<IReadOnlyCollection<ServiceEntity>> GetAllWithIncludesAsync();
    Task<ServiceEntity?> GetByIdWithIncludesAsync(long id);
}