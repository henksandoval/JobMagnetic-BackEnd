using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Commands.Skill;
using JobMagnet.Models.Responses.Skill;
using Mapster;

namespace JobMagnet.Mappers;

internal static class SkillMapper
{
    static SkillMapper()
    {
        TypeAdapterConfig<SkillEntity, SkillModel>
            .NewConfig()
            .Map(dest => dest.SkillData, src => src);

        TypeAdapterConfig<SkillCommand, SkillEntity>
            .NewConfig()
            .Map(dest => dest, src => src.SkillData);

        TypeAdapterConfig<SkillEntity, SkillCommand>
            .NewConfig()
            .Map(dest => dest.SkillData, src => src);

        TypeAdapterConfig<SkillCommand, SkillEntity>
            .NewConfig()
            .Map(dest => dest, src => src.SkillData);
    }

    internal static SkillEntity ToEntity(this SkillCommand command)
    {
        return command.Adapt<SkillEntity>();
    }

    internal static SkillModel ToModel(this SkillEntity entity)
    {
        return entity.Adapt<SkillModel>();
    }

    internal static SkillCommand ToUpdateCommand(this SkillEntity entity)
    {
        return entity.Adapt<SkillCommand>();
    }

    internal static void UpdateEntity(this SkillEntity entity, SkillCommand command)
    {
        command.Adapt(entity);
    }
}