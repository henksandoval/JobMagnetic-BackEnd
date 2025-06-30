using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.TalentShowcase;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class TalentMapper
{
    static TalentMapper()
    {
        TypeAdapterConfig<Talent, TalentResponse>
            .NewConfig()
            .Map(dest => dest.TalentBase, src => src);
    }
    public static TalentResponse ToModel(this Talent talent) => talent.Adapt<TalentResponse>();
}