using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Service;
using JobMagnet.Models.Responses.Service;
using Mapster;

namespace JobMagnet.Mappers;

internal static class ServiceMapper
{
    static ServiceMapper()
    {
        TypeAdapterConfig<ServiceCommand, ServiceEntity>
            .NewConfig()
            .Map(dest => dest, src => src.ServiceData);

        TypeAdapterConfig<ServiceEntity, ServiceCommand>
            .NewConfig()
            .Map(dest => dest.ServiceData, src => src);

        TypeAdapterConfig<ServiceCommand, ServiceEntity>
            .NewConfig()
            .Map(dest => dest, src => src.ServiceData);

        TypeAdapterConfig<ServiceEntity, ServiceModel>
            .NewConfig()
            .Map(dest => dest.ServiceData, src => src);
    }

    internal static ServiceEntity ToEntity(this ServiceCommand command)
    {
        return command.Adapt<ServiceEntity>();
    }

    internal static ServiceModel ToModel(this ServiceEntity entity)
    {
        return entity.Adapt<ServiceModel>();
    }

    internal static ServiceCommand ToUpdateCommand(this ServiceEntity entity)
    {
        return entity.Adapt<ServiceCommand>();
    }

    internal static void UpdateEntity(this ServiceEntity entity, ServiceCommand command)
    {
        command.Adapt(entity);
    }
}