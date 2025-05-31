using JobMagnet.Application.Commands.Profile;
using JobMagnet.Application.Models.Responses.Profile;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ProfileMapper
{
    static ProfileMapper()
    {
        ConfigMapper();
    }

    public static ProfileEntity ToEntity(this ProfileCommand createCommand)
    {
        return createCommand.Adapt<ProfileEntity>();
    }

    public static ProfileModel ToModel(this ProfileEntity entity)
    {
        return entity.Adapt<ProfileModel>();
    }

    public static ProfileCommand ToCommand(this ProfileParseDto profileParse)
    {
        return profileParse.Adapt<ProfileCommand>();
    }

    public static void UpdateEntity(this ProfileEntity entity, ProfileCommand command)
    {
        command.Adapt(entity);
    }

    private static void ConfigMapper()
    {
        TypeAdapterConfig<ProfileEntity, ProfileCommand>
            .NewConfig()
            .Map(dest => dest.ProfileData, src => src);

        TypeAdapterConfig<ProfileCommand, ProfileEntity>
            .NewConfig()
            .Map(dest => dest, src => src.ProfileData);
    }
}