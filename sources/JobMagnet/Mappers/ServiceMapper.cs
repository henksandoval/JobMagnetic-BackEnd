using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Service;
using Mapster;

namespace JobMagnet.Mappers;

internal static class ServiceMapper
{
    internal static ServiceEntity ToEntity(ServiceCreateRequest request)
    {
        return request.Adapt<ServiceEntity>();
    }

    internal static ServiceModel ToModel(ServiceEntity entity)
    {
        return entity.Adapt<ServiceModel>();
    }

    internal static ServiceRequest ToUpdateRequest(ServiceEntity entity)
    {
        return entity.Adapt<ServiceRequest>();
    }

    internal static void UpdateEntity(this ServiceEntity entity, ServiceRequest request)
    {
        request.Adapt(entity);
    }
}