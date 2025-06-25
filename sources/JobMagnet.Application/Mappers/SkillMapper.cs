using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Skill;
using JobMagnet.Domain.Core.Entities.Skills;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class SkillMapper
{
    static SkillMapper()
    {
        TypeAdapterConfig<Skill, SkillItemBase>
            .NewConfig()
            .Map(dest => dest.ProficiencyLevel, src => src.ProficiencyLevel)
            .Map(dest => dest.Rank, src => src.Rank)
            .Map(dest => dest.Name, src => src.SkillType.Name)
            .Map(dest => dest.IconUrl, src => src.SkillType.IconUrl)
            .Map(dest => dest.Category, src => src.SkillType.Category.Name);

        TypeAdapterConfig<SkillSet, SkillResponse>
            .NewConfig()
            .Map(dest => dest.SkillData, src => src);

        TypeAdapterConfig<SkillCommand, SkillSet>
            .NewConfig()
            .Map(dest => dest, src => src.SkillData);

        TypeAdapterConfig<SkillSet, SkillCommand>
            .NewConfig()
            .Map(dest => dest.SkillData, src => src);
    }

    public static SkillResponse ToResponse(this SkillSet set) => set.Adapt<SkillResponse>();

    public static SkillCommand ToUpdateCommand(this SkillSet set) => set.Adapt<SkillCommand>();
}