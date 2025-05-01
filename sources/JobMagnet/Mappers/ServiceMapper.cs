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

    internal static ServiceEntity ToEntity(this ServiceCreateCommand command)
    {
        return command.Adapt<ServiceEntity>();
    }

    internal static ServiceModel ToModel(ServiceEntity entity)
    {
        return entity.Adapt<ServiceModel>();
    }

    internal static ServiceUpdateCommand ToUpdateRequest(ServiceEntity entity)
    {
        return entity.Adapt<ServiceUpdateCommand>();
    }

    internal static void UpdateEntity(this ServiceEntity entity, ServiceUpdateCommand updateCommand)
    {
        updateCommand.Adapt(entity);
    }
}