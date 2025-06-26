using JobMagnet.Application.Contracts.Commands.Profile;
using JobMagnet.Application.Contracts.Responses.Profile;
using JobMagnet.Domain.Core.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ProfileMapper
{
    static ProfileMapper()
    {
        ConfigMapper();
    }

    public static ProfileEntity ToEntity(this ProfileCommand createCommand) => createCommand.Adapt<ProfileEntity>();

    public static ProfileResponse ToModel(this ProfileEntity entity) => entity.Adapt<ProfileResponse>();

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
            .Map(dest => dest, src => src.ProfileData)
            .MapToConstructor(true);
    }
}