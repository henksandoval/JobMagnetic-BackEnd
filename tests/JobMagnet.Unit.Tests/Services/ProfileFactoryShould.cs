using AutoFixture;
using CSharpFunctionalExtensions;
using FluentAssertions;
using JobMagnet.Application.Factories;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Application.UseCases.CvParser.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Services;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using Moq;

namespace JobMagnet.Unit.Tests.Services;

public class ProfileFactoryShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly ProfileRawBuilder _profileBuilder;
    private readonly ProfileFactory _profileFactory;
    private readonly Mock<IContactTypeResolverService> _contactTypeResolverMock;
    private readonly Mock<ISkillTypeResolverService> _skillTypeResolverMock;

    public ProfileFactoryShould()
    {
        _profileBuilder = new ProfileRawBuilder(_fixture);
        _skillTypeResolverMock = new Mock<ISkillTypeResolverService>();
        _contactTypeResolverMock = new Mock<IContactTypeResolverService>();
        _profileFactory = new ProfileFactory(
            _contactTypeResolverMock.Object,
            _skillTypeResolverMock.Object);
    }

    [Fact(DisplayName = "Map root properties from a simple DTO")]
    public async Task MapRootProperties_FromSimpleDto()
    {
        // Given
        var profileDto = _profileBuilder
            .Build()
            .ToProfileParseDto();

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        profile.Should().NotBeNull();
        profile.Should().BeEquivalentTo(profileDto, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map talents collection when the DTO provides them")]
    public async Task MapTalents_WhenDtoProvidesThem()
    {
        // Given
        var profileDto = _profileBuilder
            .WithTalents()
            .Build()
            .ToProfileParseDto();

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        profile.Should().NotBeNull();
        profile.Talents.Should().BeEquivalentTo(profileDto.Talents, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map testimonials collection when the DTO provides them")]
    public async Task MapTestimonials_WhenDtoProvidesThem()
    {
        // Given
        var profileDto = _profileBuilder
            .WithTestimonials()
            .Build()
            .ToProfileParseDto();

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        profile.Should().NotBeNull();
        profile.Testimonials.Should()
            .BeEquivalentTo(profileDto.Testimonials, options => options.ExcludingMissingMembers());
    }

    #region Resume Mapping Tests

    [Fact(DisplayName = "Map resume aggregation when the DTO provides them")]
    public async Task MapResume_WhenDtoProvidesThem()
    {
        // Given
        var profileDto = _profileBuilder
            .WithResume()
            .Build()
            .ToProfileParseDto();

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        profile.Should().NotBeNull();
        profile.Resume.Should().BeEquivalentTo(profileDto.Resume, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map contact info when the type exists")]
    public async Task MapContactInfo_WhenTypeExists_MapsCorrectly()
    {
        // Given
        var emailType = new ContactTypeEntity(1, "Email", "bx bx-envelope");

        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync(
                It.Is<string>(s => s.Equals("Email", StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(emailType));

        var contacts = new[] { new ContactInfoRaw("EMAIL", "test@test.com") };
        var profileDto = _profileBuilder
            .WithResume()
            .WithContactInfo(contacts.ToList())
            .Build()
            .ToProfileParseDto();

        var expectedContactInfo = new List<ContactInfoEntity> { new(0, "test@test.com", emailType) };

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        var actualContactInfo = profile.Resume!.ContactInfo!.ToList();
        actualContactInfo.Should().BeEquivalentTo(expectedContactInfo, options => options.Excluding(entity => entity.Id));
    }

    [Fact(DisplayName = "Map contact info when the type is an alias")]
    public async Task MapContactInfo_WhenTypeIsAlias_MapsToCorrectBaseType()
    {
        // Given
        var phoneType = new ContactTypeEntity(2, "Phone", "bx bx-mobile");

        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync("Teléfono", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(phoneType));

        var contacts = new[] { new ContactInfoRaw("Teléfono", "+58 412457824") };
        var profileDto = _profileBuilder
            .WithResume()
            .WithContactInfo(contacts.ToList())
            .Build()
            .ToProfileParseDto();

        var expectedContactInfo = new List<ContactInfoEntity> { new(0, "+58 412457824", phoneType) };

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        var actualContactInfo = profile.Resume!.ContactInfo!.ToList();
        actualContactInfo.Should().BeEquivalentTo(expectedContactInfo, options => options.Excluding(entity => entity.Id));
    }

    [Fact(DisplayName = "Map contact info when the type does not exist")]
    public async Task MapContactInfo_WhenTypeDoesNotExist_CreatesAdHocTypeWithDefaultIcon()
    {
        // Given
        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync("TypeDontExist", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe<ContactTypeEntity>.None);

        var contacts = new[] { new ContactInfoRaw("TypeDontExist", "Some value") };
        var profileDto = _profileBuilder
            .WithResume()
            .WithContactInfo(contacts.ToList())
            .Build()
            .ToProfileParseDto();

        var expectedAdHocType = new ContactTypeEntity("TypeDontExist");
        expectedAdHocType.SetDefaultIcon();

        var expectedContactInfo = new List<ContactInfoEntity> { new(0, "Some value", expectedAdHocType) };

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        var actualContactInfo = profile.Resume!.ContactInfo!.ToList();
        actualContactInfo.Should().BeEquivalentTo(expectedContactInfo, options => options.Excluding(entity => entity.Id));
    }

    [Fact(DisplayName = "Map multiple contact infos with mixed types")]
    public async Task MapContactInfo_WithMultipleItems_MapsAllCorrectly()
    {
        // Given
        var emailType = new ContactTypeEntity(1, "Email", "bx bx-envelope");
        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync("Email", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(emailType));

        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync("TypeDontExist", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe<ContactTypeEntity>.None);

        var contacts = new[]
        {
            new ContactInfoRaw("Email", "test@test.com"),
            new ContactInfoRaw("TypeDontExist", "Some value")
        };
        var profileDto = _profileBuilder
            .WithResume()
            .WithContactInfo(contacts.ToList())
            .Build()
            .ToProfileParseDto();

        var expectedAdHocType = new ContactTypeEntity("TypeDontExist");
        expectedAdHocType.SetDefaultIcon();

        var expectedContactInfo = new List<ContactInfoEntity>
        {
            new(0, "test@test.com", emailType),
            new(0, "Some value", expectedAdHocType)
        };

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        var actualContactInfo = profile.Resume!.ContactInfo!.OrderBy(c => c.Value).ToList();
        var orderedExpected = expectedContactInfo.OrderBy(c => c.Value).ToList();
        actualContactInfo.Should().BeEquivalentTo(orderedExpected, options => options.Excluding(entity => entity.Id));
    }
    #endregion

    #region SkillSet Mapping Tests

    [Fact(DisplayName = "Map SkillSet aggregation when the DTO provides them")]
    public async Task MapSkillSet_WhenDtoProvidesThem()
    {
        // Given
        var profileDto = _profileBuilder
            .WithSkillSet()
            .Build()
            .ToProfileParseDto();

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        profile.Should().NotBeNull();
        profile.SkillSet.Should().BeEquivalentTo(profileDto.SkillSet, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map skills collection when the type exists")]
    public async Task MapSkills_WhenTypeExists_MapsCorrectly()
    {
        // Given
        var skills = new List<SkillRaw> { new ("C#", "10") };
        var csharpSkill = new SkillType(
            1,
            "C#",
            new SkillCategory("Programming"),
            new Uri("https://example.com/csharp-icon.png"));

        _skillTypeResolverMock
            .Setup(r => r.ResolveAsync(
                It.Is<string>(s => s.Equals("C#", StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(csharpSkill));

        var profileDto = _profileBuilder
            .WithSkillSet()
            .WithSkills(skills)
            .Build()
            .ToProfileParseDto();

        var skillSet = new SkillSetEntity("Test Overview", 1);
        skillSet.AddSkill(10, csharpSkill);
        var expectedSkill = skillSet.Skills.ToList();

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        var currentSkills = profile.SkillSet!.Skills!.ToList();
        currentSkills.Should().BeEquivalentTo(expectedSkill, options => options
            .Excluding(entity => entity.SkillSet)
        );
    }

    [Fact(DisplayName = "Map skills collection when the type is an alias")]
    public async Task MapSkills_WhenTypeIsAlias_MapsToCorrectBaseType()
    {
        // Given
        var skills = new List<SkillRaw> { new ("csharp", "10") };
        var csharpSkill = new SkillType(
            1,
            "C#",
            new SkillCategory("Programming"),
            new Uri("https://example.com/csharp-icon.png"));

        _skillTypeResolverMock
            .Setup(r => r.ResolveAsync("csharp", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(csharpSkill));

        var profileDto = _profileBuilder
            .WithSkillSet()
            .WithSkills(skills)
            .Build()
            .ToProfileParseDto();

        var skillSet = new SkillSetEntity("Test Overview", 1);
        skillSet.AddSkill(10, csharpSkill);
        var expectedSkill = skillSet.Skills.ToList();

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        var currentSkills = profile.SkillSet!.Skills!.ToList();
        currentSkills.Should().BeEquivalentTo(expectedSkill, options => options
            .Excluding(entity => entity.SkillSet)
        );
    }

    [Fact(DisplayName = "Map skills when the type does not exist")]
    public async Task MapSkills_WhenTypeDoesNotExist_CreatesAdHocTypeWithDefaultIcon()
    {
        // Given
        var skills = new List<SkillRaw> { new ("TypeDontExist", "10") };

        _skillTypeResolverMock
            .Setup(r => r.ResolveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.None);

        var expectedAdHocType = new SkillType("TypeDontExist");
        expectedAdHocType.SetDefaultIcon();

        var skillSet = new SkillSetEntity("Test Overview", 1);
        skillSet.AddSkill(10, expectedAdHocType);
        var expectedSkill = skillSet.Skills.ToList();

        var profileDto = _profileBuilder
            .WithSkillSet()
            .WithSkills(skills)
            .Build()
            .ToProfileParseDto();

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        var currentSkills = profile.SkillSet!.Skills!.ToList();
        currentSkills.Should().BeEquivalentTo(expectedSkill, options => options
            .Excluding(entity => entity.Id)
            .Excluding(entity => entity.SkillSet)
        );
    }


    [Fact(DisplayName = "Map multiple skills with mixed types")]
    public async Task MapSkills_WithMultipleItems_MapsAllCorrectly()
    {
        // Given
        var csharpSkill = new SkillType(
            1,
            "C#",
            new SkillCategory("Programming"),
            new Uri("https://example.com/csharp-icon.png"));

        _skillTypeResolverMock
            .Setup(r => r.ResolveAsync(It.Is<string>("csharp", StringComparer.InvariantCulture), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(csharpSkill));

        var expectedAdHocType = new SkillType("TypeDontExist");
        expectedAdHocType.SetDefaultIcon();

        var skills = new[]
        {
            new SkillRaw("csharp", "5"),
            new SkillRaw("TypeDontExist", "2")
        };
        var profileDto = _profileBuilder
            .WithSkillSet()
            .WithSkills(skills.ToList())
            .Build()
            .ToProfileParseDto();

        var skillSet = new SkillSetEntity("Test Overview", 1);
        skillSet.AddSkill(5, csharpSkill);
        skillSet.AddSkill(2, expectedAdHocType);
        var expectedSkills = skillSet.Skills.ToList();

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        var currentSkills = profile.SkillSet!.Skills!.ToList();
        currentSkills.Should().BeEquivalentTo(expectedSkills, options => options
            .Excluding(entity => entity.Id)
            .Excluding(entity => entity.SkillSet)
        );
    }
    #endregion
}