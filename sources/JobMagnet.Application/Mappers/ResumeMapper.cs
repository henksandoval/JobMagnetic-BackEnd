using JobMagnet.Application.Contracts.Commands.Resume;
using JobMagnet.Application.Contracts.Responses.Resume;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ResumeMapper
{
    static ResumeMapper()
    {
        TypeAdapterConfig<ResumeEntity, ResumeResponse>
            .NewConfig()
            .Map(dest => dest.ResumeData, src => src);

        TypeAdapterConfig<ResumeEntity, ResumeCommand>
            .NewConfig()
            .Map(dest => dest.ResumeData, src => src);

        TypeAdapterConfig<ResumeCommand, ResumeEntity>
            .NewConfig()
            .Map(dest => dest, src => src.ResumeData);
    }

    public static ResumeResponse ToModel(this ResumeEntity entity)
    {
        return entity.Adapt<ResumeResponse>();
    }

    public static ResumeEntity ToEntity(this ResumeCommand command)
    {
        return command.Adapt<ResumeEntity>();
    }

    public static ResumeCommand ToUpdateRequest(this ResumeEntity entity)
    {
        return entity.Adapt<ResumeCommand>();
    }

    public static void UpdateEntity(this ResumeEntity entity, ResumeCommand command)
    {
        command.Adapt(entity);
    }
}