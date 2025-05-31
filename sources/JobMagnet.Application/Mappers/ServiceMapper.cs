using JobMagnet.Application.Contracts.Commands.Service;
using JobMagnet.Application.Contracts.Responses.Service;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ServiceMapper
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

        TypeAdapterConfig<ServiceEntity, ServiceResponse>
            .NewConfig()
            .Map(dest => dest.ServiceData, src => src);
    }

    public static ServiceEntity ToEntity(this ServiceCommand command)
    {
        return command.Adapt<ServiceEntity>();
    }

    public static ServiceResponse ToModel(this ServiceEntity entity)
    {
        return entity.Adapt<ServiceResponse>();
    }

    public static ServiceCommand ToUpdateCommand(this ServiceEntity entity)
    {
        return entity.Adapt<ServiceCommand>();
    }

    public static void UpdateEntity(this ServiceEntity entity, ServiceCommand command)
    {
        command.Adapt(entity);
    }
}