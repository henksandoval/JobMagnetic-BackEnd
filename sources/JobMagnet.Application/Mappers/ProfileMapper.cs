using JobMagnet.Application.Contracts.Responses.Base;
using JobMagnet.Application.Contracts.Responses.Profile;
using JobMagnet.Domain.Aggregates.Profiles;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ProfileMapper
{
    static ProfileMapper()
    {
        TypeAdapterConfig<Profile, ProfileBase>
            .NewConfig()
            .Map(dest => dest.FirstName, src => src.Name.FirstName)
            .Map(dest => dest.LastName, src => src.Name.LastName)
            .Map(dest => dest.MiddleName, src => src.Name.MiddleName)
            .Map(dest => dest.SecondLastName, src => src.Name.SecondLastName)
            .Map(dest => dest.ProfileImageUrl, src => src.ProfileImage.GetUrl())
            .Map(dest => dest.BirthDate, src => src.BirthDate.Value);

        TypeAdapterConfig<Profile, ProfileResponse>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ProfileData, src => src);
    }

    public static ProfileResponse ToModel(this Profile entity) => entity.Adapt<ProfileResponse>();
}