using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Skill;
using Mapster;

namespace JobMagnet.Mappers;

internal static class SkillMapper
{
    internal static SkillEntity ToEntity(SkillCreateRequest request)
    {
        return request.Adapt<SkillEntity>();
    }

    internal static SkillModel ToModel(SkillEntity entity)
    {
        return entity.Adapt<SkillModel>();
    }

    internal static SkillRequest ToUpdateRequest(SkillEntity entity)
    {
        return entity.Adapt<SkillRequest>();
    }

    internal static void UpdateEntity(this SkillEntity entity, SkillRequest request)
    {
        request.Adapt(entity);
    }
}