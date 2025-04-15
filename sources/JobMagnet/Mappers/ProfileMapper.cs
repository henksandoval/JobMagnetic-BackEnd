using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Profile;
using Mapster;

namespace JobMagnet.Mappers;

internal static class ProfileMapper
{
    static ProfileMapper()
    {
        TypeAdapterConfig<ProfileEntity, ProfileModel>
            .NewConfig()
            .Map(dest => dest.Talents,
                src => src.Talents.Select(talent => talent.Description).ToArray());
    }

    internal static ProfileModel ToModel(ProfileEntity entity)
    {
        return entity.Adapt<ProfileModel>();
    }
}