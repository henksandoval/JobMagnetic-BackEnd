using JobMagnet.Application.Contracts.Responses.Profile;
using JobMagnet.Domain.Aggregates.Profiles;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ProfileMapper
{
    static ProfileMapper()
    {
        TypeAdapterConfig<Profile, ProfileResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ProfileData, src => src);
    }

    public static ProfileResponse ToModel(this Profile entity) => entity.Adapt<ProfileResponse>();
}