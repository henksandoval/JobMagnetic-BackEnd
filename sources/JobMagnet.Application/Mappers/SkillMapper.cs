using JobMagnet.Application.Contracts.Commands.Skill;
using JobMagnet.Application.Contracts.Responses.Skill;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class SkillMapper
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

    public static SkillEntity ToEntity(this SkillCommand command)
    {
        return command.Adapt<SkillEntity>();
    }

    public static SkillModel ToModel(this SkillEntity entity)
    {
        return entity.Adapt<SkillModel>();
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