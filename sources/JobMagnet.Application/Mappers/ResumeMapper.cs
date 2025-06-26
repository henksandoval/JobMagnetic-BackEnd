using JobMagnet.Application.Contracts.Commands.Resume;
using JobMagnet.Application.Contracts.Responses.Resume;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ResumeMapper
{
    static ResumeMapper()
    {
        TypeAdapterConfig<Headline, ResumeResponse>
            .NewConfig()
            .Map(dest => dest.ResumeData, src => src);

        TypeAdapterConfig<Headline, ResumeCommand>
            .NewConfig()
            .Map(dest => dest.ResumeData, src => src);
    }

    public static ResumeResponse ToModel(this Headline entity) => entity.Adapt<ResumeResponse>();

    public static ResumeCommand ToUpdateRequest(this Headline entity) => entity.Adapt<ResumeCommand>();
}