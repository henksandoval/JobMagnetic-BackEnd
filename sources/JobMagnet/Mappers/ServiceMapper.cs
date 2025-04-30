using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Service;
using JobMagnet.Models.Responses.Service;
using Mapster;

namespace JobMagnet.Mappers;

internal static class ServiceMapper
{
    static ServiceMapper()
    {
        TypeAdapterConfig<ServiceCreateCommand, ServiceEntity>
            .NewConfig()
            .Map(dest => dest, src => src.ServiceData);
    }

    internal static ServiceEntity ToEntity(ServiceCreateCommand command)
    {
        return command.Adapt<ServiceEntity>();
    }

    internal static ServiceModel ToModel(ServiceEntity entity)
    {
        return entity.Adapt<ServiceModel>();
    }

    internal static ServiceCommand ToUpdateRequest(ServiceEntity entity)
    {
        return entity.Adapt<ServiceCommand>();
    }

    internal static void UpdateEntity(this ServiceEntity entity, ServiceCommand command)
    {
        command.Adapt(entity);
    }
}