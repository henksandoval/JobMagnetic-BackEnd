using JobMagnet.Application.UseCases.CvParser.ParsingDTOs;
using JobMagnet.Application.UseCases.CvParser.RawDTOs;
using Mapster;

namespace JobMagnet.Application.UseCases.CvParser.Mappers;

public static class ProfileMapper
{
    static ProfileMapper()
    {
        ConfigMapper();
    }

    public static ProfileParseDto ToCommand(this ProfileRaw profileRaw)
    {
        return profileRaw.Adapt<ProfileParseDto>();
    }

    private static void ConfigMapper()
    {
        TypeAdapterConfig<ProfileRaw, ProfileParseDto>
            .NewConfig();
    }
}