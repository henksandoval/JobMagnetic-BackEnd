using AwesomeAssertions;
using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Domain.Aggregates.Profiles;

namespace JobMagnet.Shared.Tests.AssertionExtensions;

public static class ProfileAssertionExtensions
{
    public static void ShouldBeEquivalentToDto(this Profile profile, ProfileParseDto dto)
    {
        profile.Should().NotBeNull();
        profile.Name.FirstName.Should().Be(dto.FirstName);
        profile.Name.LastName.Should().Be(dto.LastName);
        profile.Name.MiddleName.Should().Be(dto.MiddleName);
        profile.Name.SecondLastName.Should().Be(dto.SecondLastName);
        profile.BirthDate.Value.Should().Be(dto.BirthDate);
        profile.ProfileImage.Url.Should().Be(dto.ProfileImageUrl);
    }
}