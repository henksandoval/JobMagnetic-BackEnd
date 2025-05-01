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

        TypeAdapterConfig<SkillCreateCommand, SkillEntity>
            .NewConfig()
            .Map(dest => dest, src => src.SkillData);

        TypeAdapterConfig<SkillEntity, SkillUpdateCommand>
            .NewConfig()
            .Map(dest => dest.SkillData, src => src);

        TypeAdapterConfig<SkillUpdateCommand, SkillEntity>
            .NewConfig()
            .Map(dest => dest, src => src.SkillData);
    }

    internal static SkillEntity ToEntity(this SkillCreateCommand command)
    {
        return command.Adapt<SkillEntity>();
    }

    internal static SkillModel ToModel(this SkillEntity entity)
    {
        return entity.Adapt<SkillModel>();
    }

    internal static SkillUpdateCommand ToUpdateCommand(this SkillEntity entity)
    {
        return entity.Adapt<SkillUpdateCommand>();
    }

    internal static void UpdateEntity(this SkillEntity entity, SkillUpdateCommand updateCommand)
    {
        updateCommand.Adapt(entity);
    }
}