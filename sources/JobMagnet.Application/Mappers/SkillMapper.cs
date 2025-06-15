using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Contracts.Responses.Skill;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class SkillMapper
{
    static SkillMapper()
    {
        TypeAdapterConfig<SkillSetEntity, SkillResponse>
            .NewConfig()
            .Map(dest => dest.SkillData, src => src);

        TypeAdapterConfig<SkillCommand, SkillSetEntity>
            .NewConfig()
            .Map(dest => dest, src => src.SkillData);

        TypeAdapterConfig<SkillSetEntity, SkillCommand>
            .NewConfig()
            .Map(dest => dest.SkillData, src => src);
    }

    public static SkillSetEntity ToEntity(this SkillCommand command)
    {
        var entity = new SkillSetEntity(command.SkillData.Overview!, command.SkillData.ProfileId);

        foreach (var skillDetailCommand in command.SkillData.Skills)
        {
            var skillDetail = new SkillEntity(skillDetailCommand.ProficiencyLevel,
                skillDetailCommand.Rank,
                entity,
                null);
            entity.Add(skillDetail);
        }

        return entity;
    }

    public static SkillResponse ToResponse(this SkillSetEntity setEntity)
    {
        return setEntity.Adapt<SkillResponse>();
    }

    public static SkillCommand ToUpdateCommand(this SkillSetEntity setEntity)
    {
        return setEntity.Adapt<SkillCommand>();
    }

    public static void UpdateEntity(this SkillSetEntity setEntity, SkillCommand command)
    {
        command.Adapt(setEntity);
    }
}