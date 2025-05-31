using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;

namespace JobMagnet.Domain.Ports.Repositories;

public interface IServiceQueryRepository : IQueryRepository<ServiceEntity, long>
{
    IServiceQueryRepository IncludeGalleryItems();
    Task<IReadOnlyCollection<ServiceEntity>> GetAllWithIncludesAsync();
    Task<ServiceEntity?> GetByIdWithIncludesAsync(long id);
}