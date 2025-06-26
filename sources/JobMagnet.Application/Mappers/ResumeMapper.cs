using JobMagnet.Application.Contracts.Commands.Resume;
using JobMagnet.Application.Contracts.Responses.Resume;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ResumeMapper
{
    static ResumeMapper()
    {
        TypeAdapterConfig<Resume, ResumeResponse>
            .NewConfig()
            .Map(dest => dest.ResumeData, src => src);

        TypeAdapterConfig<Resume, ResumeCommand>
            .NewConfig()
            .Map(dest => dest.ResumeData, src => src);
    }

    public static ResumeResponse ToModel(this Resume entity) => entity.Adapt<ResumeResponse>();

    public static ResumeCommand ToUpdateRequest(this Resume entity) => entity.Adapt<ResumeCommand>();
}