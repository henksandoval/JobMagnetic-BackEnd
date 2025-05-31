using JobMagnet.Application.Contracts.Commands.Profile;
using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using Mapster;

namespace JobMagnet.Application.UseCases.CvParser.Mappers;

public static class ProfileMapper
{
    static ProfileMapper()
    {
        ConfigMapper();
    }

    public static ProfileCommand ToCommand(this ProfileParseDto profileParse)
    {
        return profileParse.Adapt<ProfileCommand>();
    }

    private static void ConfigMapper()
    {
        TypeAdapterConfig<ProfileParseDto, ProfileCommand>
            .NewConfig();
    }
}