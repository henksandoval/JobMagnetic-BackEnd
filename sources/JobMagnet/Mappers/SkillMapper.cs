using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Skill;
using JobMagnet.Models.Responses.Skill;
using Mapster;

namespace JobMagnet.Mappers;

internal static class SkillMapper
{
    internal static SkillEntity ToEntity(this SkillCreateCommand command)
    {
        return command.Adapt<SkillEntity>();
    }

    internal static SkillModel ToModel(this SkillEntity entity)
    {
        return entity.Adapt<SkillModel>();
    }

    internal static SkillPatchCommand ToUpdateRequest(this SkillEntity entity)
    {
        return entity.Adapt<SkillPatchCommand>();
    }

    internal static void UpdateEntity(this SkillEntity entity, SkillPatchCommand patchCommand)
    {
        patchCommand.Adapt(entity);
    }
}