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
    }

    public static ResumeResponse ToModel(this ResumeEntity entity)
    {
        return entity.Adapt<ResumeResponse>();
    }

    public static ResumeCommand ToUpdateRequest(this ResumeEntity entity)
    {
        return entity.Adapt<ResumeCommand>();
    }
}