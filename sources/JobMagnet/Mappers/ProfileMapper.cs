using JobMagnet.Infrastructure.Entities;
using JobMagnet.Models.Profile;
using Mapster;

namespace JobMagnet.Mappers;

internal static class ProfileMapper
{
    internal static ProfileModel ToModel(ProfileEntity entity)
    {
        return entity.Adapt<ProfileModel>();
    }
}