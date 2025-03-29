using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Skill;
using Mapster;

namespace JobMagnet.Mappers;

public static class SkillMapper
{
    public static SkillEntity ToEntity(SkillCreateRequest request)
    {
        return request.Adapt<SkillEntity>();
    }

    public static SkillModel ToModel(SkillEntity entity)
    {
        return entity.Adapt<SkillModel>();
    }

    public static SkillRequest ToUpdateRequest(SkillEntity entity)
    {
        return entity.Adapt<SkillRequest>();
    }

    public static void UpdateEntity(this SkillEntity entity, SkillRequest request)
    {
        request.Adapt(entity);
    }
}