using JobMagnet.Application.Contracts.Commands.Profile;
using JobMagnet.Application.Contracts.Responses.Profile;
using JobMagnet.Domain.Aggregates.Profiles;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ProfileMapper
{
    static ProfileMapper()
    {
        TypeAdapterConfig<Profile, ProfileCommand>
            .NewConfig()
            .Map(dest => dest.ProfileData, src => src);
    }

    public static ProfileResponse ToModel(this Profile entity) => entity.Adapt<ProfileResponse>();

    private static void ConfigMapper()
    {
        TypeAdapterConfig<Profile, ProfileCommand>
            .NewConfig()
            .Map(dest => dest.ProfileData, src => src);
    }
}