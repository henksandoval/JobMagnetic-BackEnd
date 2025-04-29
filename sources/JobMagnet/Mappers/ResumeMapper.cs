using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Resume;
using JobMagnet.Models.Responses.Resume;
using Mapster;

namespace JobMagnet.Mappers;

internal static class ResumeMapper
{
    static ResumeMapper()
    {
        TypeAdapterConfig<ResumeEntity, ResumeModel>
            .NewConfig()
            .Map(dest => dest.ResumeData, src => src);

        TypeAdapterConfig<ResumeCreateCommand, ResumeEntity>
            .NewConfig()
            .Map(dest => dest, src => src.ResumeData);

        TypeAdapterConfig<ResumeEntity, ResumeUpdateCommand>
            .NewConfig()
            .Map(dest => dest.ResumeData, src => src);

        TypeAdapterConfig<ResumeUpdateCommand, ResumeEntity>
            .NewConfig()
            .Map(dest => dest, src => src.ResumeData);
    }

    internal static ResumeModel ToModel(this ResumeEntity entity)
    {
        return entity.Adapt<ResumeModel>();
    }

    internal static ResumeEntity ToEntity(this ResumeCreateCommand command)
    {
        return command.Adapt<ResumeEntity>();
    }

    internal static ResumeUpdateCommand ToUpdateRequest(this ResumeEntity entity)
    {
        return entity.Adapt<ResumeUpdateCommand>();
    }

    internal static void UpdateEntity(this ResumeEntity entity, ResumeUpdateCommand command)
    {
        command.Adapt(entity);
    }
}