using JobMagnet.Entities;
using JobMagnet.Models;
using Mapster;

namespace JobMagnet.Mappers;

public static class SkillMapper
{
    public static SkillModel ToModel(SkillEntity entity)
    {
        return entity.Adapt<SkillModel>();
    }

    public static SkillEntity ToEntity(SkillCreateRequest request)
    {
        return request.Adapt<SkillEntity>();
    }
}