using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Contracts.Responses.Skill;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class SkillMapper
{
    static SkillMapper()
    {
        TypeAdapterConfig<SkillEntity, SkillResponse>
            .NewConfig()
            .Map(dest => dest.SkillData, src => src);

        TypeAdapterConfig<SkillCommand, SkillEntity>
            .NewConfig()
            .Map(dest => dest, src => src.SkillData);

        TypeAdapterConfig<SkillEntity, SkillCommand>
            .NewConfig()
            .Map(dest => dest.SkillData, src => src);
    }

    public static SkillEntity ToEntity(this SkillCommand command)
    {
        var entity = new SkillEntity
        {
            Id = 0,
            Overview = command.SkillData.Overview,
            ProfileId = command.SkillData.ProfileId,
        };

        foreach (var skillDetailCommand in command.SkillData.SkillDetails)
        {
            var skillDetail = new SkillItemEntity(
                skillDetailCommand.Name!,
                skillDetailCommand.IconUrl!,
                skillDetailCommand.Category!,
                entity,
                skillDetailCommand.ProficiencyLevel,
                skillDetailCommand.Rank);
            entity.Add(skillDetail);
        }

        return entity;
    }

    public static SkillResponse ToModel(this SkillEntity entity)
    {
        return entity.Adapt<SkillResponse>();
    }

    public static SkillCommand ToUpdateCommand(this SkillEntity entity)
    {
        return entity.Adapt<SkillCommand>();
    }

    public static void UpdateEntity(this SkillEntity entity, SkillCommand command)
    {
        command.Adapt(entity);
    }
}