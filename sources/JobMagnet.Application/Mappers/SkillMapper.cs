using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Skill;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Skills;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class SkillMapper
{
    static SkillMapper()
    {
        TypeAdapterConfig<Skill, SkillBase>
            .NewConfig()
            .Map(dest => dest.ProficiencyLevel, src => src.ProficiencyLevel)
            .Map(dest => dest.Name, src => src.SkillType.Name);

        TypeAdapterConfig<SkillSet, SkillSetBase>
            .NewConfig()
            .Map(dest => dest.ProfileId, src => src.ProfileId.Value);

        TypeAdapterConfig<SkillSet, SkillResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.SkillSetData, src => src);

        TypeAdapterConfig<SkillSet, SkillCommand>
            .NewConfig()
            .Map(dest => dest.SkillSetData, src => src);
    }

    public static SkillBase ToModel(this Skill skill) => skill.Adapt<SkillBase>();
    public static SkillResponse ToModel(this SkillSet skillSet) => skillSet.Adapt<SkillResponse>();

    public static SkillResponse ToResponse(this SkillSet set) => set.Adapt<SkillResponse>();

    public static SkillCommand ToUpdateCommand(this SkillSet set) => set.Adapt<SkillCommand>();
}