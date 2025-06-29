using JobMagnet.Application.Contracts.Commands.ProfileHeader;
using JobMagnet.Application.Contracts.Responses.ProfileHeader;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using Mapster;

namespace JobMagnet.Application.Mappers;

public static class ProfileHeaderMapper
{
    static ProfileHeaderMapper()
    {
        TypeAdapterConfig<ProfileHeader, ProfileHeaderResponse>
            .NewConfig()
            .Map(dest => dest.ResumeData, src => src);

        TypeAdapterConfig<ProfileHeader, ProfileHeaderCommand>
            .NewConfig()
            .Map(dest => dest.ProfileHeaderData, src => src);
    }

    public static ProfileHeaderResponse ToModel(this ProfileHeader entity) => entity.Adapt<ProfileHeaderResponse>();

    public static ProfileHeaderCommand ToUpdateRequest(this ProfileHeader entity) => entity.Adapt<ProfileHeaderCommand>();
}