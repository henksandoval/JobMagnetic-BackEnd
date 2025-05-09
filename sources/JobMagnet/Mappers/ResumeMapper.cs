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

        TypeAdapterConfig<ResumeEntity, ResumeCommand>
            .NewConfig()
            .Map(dest => dest.ResumeData, src => src);

        TypeAdapterConfig<ResumeCommand, ResumeEntity>
            .NewConfig()
            .Map(dest => dest, src => src.ResumeData);
    }

    internal static ResumeModel ToModel(this ResumeEntity entity)
    {
        return entity.Adapt<ResumeModel>();
    }

    internal static ResumeEntity ToEntity(this ResumeCommand command)
    {
        return command.Adapt<ResumeEntity>();
    }

    internal static ResumeCommand ToUpdateRequest(this ResumeEntity entity)
    {
        return entity.Adapt<ResumeCommand>();
    }

    internal static void UpdateEntity(this ResumeEntity entity, ResumeCommand command)
    {
        command.Adapt(entity);
    }
}