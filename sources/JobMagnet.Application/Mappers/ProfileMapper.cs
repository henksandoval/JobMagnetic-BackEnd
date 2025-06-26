using JobMagnet.Application.Contracts.Commands.Profile;
using JobMagnet.Application.Contracts.Responses.Profile;
using JobMagnet.Domain.Aggregates.Profiles;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ProfileMapper
{
    static ProfileMapper()
    {
        ConfigMapper();
    }

    public static Profile ToEntity(this ProfileCommand createCommand) => createCommand.Adapt<Profile>();

    public static ProfileResponse ToModel(this Profile entity) => entity.Adapt<ProfileResponse>();

    public static void UpdateEntity(this Profile entity, ProfileCommand command)
    {
        command.Adapt(entity);
    }

    private static void ConfigMapper()
    {
        TypeAdapterConfig<Profile, ProfileCommand>
            .NewConfig()
            .Map(dest => dest.ProfileData, src => src);

        TypeAdapterConfig<ProfileCommand, Profile>
            .NewConfig()
            .Map(dest => dest, src => src.ProfileData)
            .MapToConstructor(true);
    }
}