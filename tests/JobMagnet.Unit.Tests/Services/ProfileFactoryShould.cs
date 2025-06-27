using System.Linq.Expressions;
using AutoFixture;
using AwesomeAssertions;
using CSharpFunctionalExtensions;

using JobMagnet.Application.Factories;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Application.UseCases.CvParser.Mappers;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;
using JobMagnet.Domain.Aggregates.Skills.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Services;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using Moq;

namespace JobMagnet.Unit.Tests.Services;

public class ProfileFactoryShould
{
    private readonly IClock _clock;
    private readonly IGuidGenerator _guidGenerator;
    private readonly Mock<IContactTypeResolverService> _contactTypeResolverMock;
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly ProfileRawBuilder _profileBuilder;
    private readonly ProfileFactory _profileFactory;
    private readonly Mock<IQueryRepository<SkillCategory, ushort>> _skillCategoryRepositoryMock;
    private readonly Mock<ISkillTypeResolverService> _skillTypeResolverMock;

    public ProfileFactoryShould()
    {
        _profileBuilder = new ProfileRawBuilder(_fixture);
        _skillTypeResolverMock = new Mock<ISkillTypeResolverService>();
        _contactTypeResolverMock = new Mock<IContactTypeResolverService>();
        _skillCategoryRepositoryMock = new Mock<IQueryRepository<SkillCategory, ushort>>();
        _clock = new DeterministicClock();
        _guidGenerator = new SequentialGuidGenerator();

        _profileFactory = new ProfileFactory(
            _guidGenerator,
            _clock,
            _contactTypeResolverMock.Object,
            _skillTypeResolverMock.Object,
            _skillCategoryRepositoryMock.Object);
    }

    [Fact(DisplayName = "Map root properties from a simple DTO")]
    public async Task MapRootProperties_FromSimpleDto()
    {
        // --- Given ---
        var profileDto = _profileBuilder
            .Build()
            .ToProfileParseDto();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,
            CancellationToken.None);

        // --- Then ---
        profile.Should().NotBeNull();
        profile.Should().BeEquivalentTo(profileDto,
            options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map talents collection when the DTO provides them")]
    public async Task CreateProfileFromDtoAsync_WhenDtoContainsTalents_ShouldCreateProfileWithCorrectTalentCollection()
    {
        // --- Given ---
        var profileDto = _profileBuilder
            .WithTalents()
            .Build()
            .ToProfileParseDto();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,
            CancellationToken.None);

        // --- Then ---
        profile.Should().NotBeNull();
        profile.Talents.Should().NotBeNull();

        profile.Talents.Should().BeEquivalentTo(profileDto.Talents,
            options => options);
    }

    [Fact(DisplayName = "Map testimonials collection when the DTO provides them")]
    public async Task MapTestimonials_WhenDtoProvidesThem()
    {
        // --- Given ---
        var profileDto = _profileBuilder
            .WithTestimonials()
            .Build()
            .ToProfileParseDto();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,
            CancellationToken.None);

        // --- Then ---
        profile.Should().NotBeNull();
        profile.Testimonials.Should()
            .BeEquivalentTo(profileDto.Testimonials,
                options => options.ExcludingMissingMembers());
    }
/*
    #region Headline Mapping Tests

    [Fact(DisplayName = "Map resume aggregation when the DTO provides them")]
    public async Task MapResume_WhenDtoProvidesThem()
    {
        // --- Given ---
        var profileDto = _profileBuilder
            .WithResume()
            .Build()
            .ToProfileParseDto();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,
            CancellationToken.None);

        // --- Then ---
        profile.Should().NotBeNull();
        profile.Resume.Should().NotBeNull();
        var resume = profile.Resume!;
        var expectedResume = profileDto.Resume!;

        resume.About.Should().Be(expectedResume.About ?? string.Empty);
        resume.Address.Should().Be(expectedResume.Address ?? string.Empty);
        resume.JobTitle.Should().Be(expectedResume.JobTitle ?? string.Empty);
        resume.Overview.Should().Be(expectedResume.Overview ?? string.Empty);
        resume.Suffix.Should().Be(expectedResume.Suffix ?? string.Empty);
        resume.Title.Should().Be(expectedResume.Title ?? string.Empty);
    }

    [Fact(DisplayName = "Map contact info when the type exists")]
    public async Task MapContactInfo_WhenTypeExists_MapsCorrectly()
    {
        // --- Given ---
        const string expectedTypeName = "Email";
        const string expectedIconClass = "bx bx-envelope";
        const string expectedValue = "test@test.com";

        var emailType = ContactType.CreateInstance(Guid.Empty,
            expectedTypeName,
            expectedIconClass);

        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync(
                It.Is<string>(s => s.Equals(expectedTypeName,
                    StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(emailType));

        var contacts = new[] { new ContactInfoRaw(expectedTypeName.ToUpper(),
            expectedValue) };
        var profileDto = _profileBuilder
            .WithResume()
            .WithContactInfo(contacts.ToList())
            .Build()
            .ToProfileParseDto();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,
            CancellationToken.None);

        // --- Then ---
        profile.Resume.Should().NotBeNull();
        profile.Resume!.ContactInfo.Should().NotBeNull();
        profile.Resume!.ContactInfo.Should().HaveCount(1);

        var actualContactInfo = profile.Resume!.ContactInfo!.First();
        actualContactInfo.Value.Should().Be(expectedValue);
        actualContactInfo.ContactType.Name.Should().Be(expectedTypeName);
        actualContactInfo.ContactType.IconClass.Should().Be(expectedIconClass);
    }

    [Fact(DisplayName = "Map contact info when the type is an alias")]
    public async Task MapContactInfo_WhenTypeIsAlias_MapsToCorrectBaseType()
    {
        // --- Given ---
        const string expectedTypeName = "Phone";
        const string typeAlias = "Teléfono";
        const string expectedIconClass = "bx bx-mobile";
        const string expectedValue = "+58 412457824";

        var phoneType = ContactType.CreateInstance(Guid.Empty,
            expectedTypeName,
            expectedIconClass);

        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync(typeAlias,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(phoneType));

        var contacts = new[] { new ContactInfoRaw(typeAlias,
            expectedValue) };
        var profileDto = _profileBuilder
            .WithResume()
            .WithContactInfo(contacts.ToList())
            .Build()
            .ToProfileParseDto();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,
            CancellationToken.None);

        // --- Then ---
        profile.Resume.Should().NotBeNull();
        profile.Resume!.ContactInfo.Should().NotBeNull();
        profile.Resume!.ContactInfo.Should().HaveCount(1);

        var actualContactInfo = profile.Resume!.ContactInfo!.First();
        actualContactInfo.Value.Should().Be(expectedValue);
        actualContactInfo.ContactType.Name.Should().Be(expectedTypeName);
        actualContactInfo.ContactType.IconClass.Should().Be(expectedIconClass);
    }

    [Fact(DisplayName = "Map contact info when the type does not exist")]
    public async Task MapContactInfo_WhenTypeDoesNotExist_CreatesAdHocTypeWithDefaultIcon()
    {
        // --- Given ---
        const string unknownTypeName = "TypeDontExist";
        const string unknownTypeValue = "Some value";

        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync(unknownTypeName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe<ContactType>.None);

        var contacts = new[] { new ContactInfoRaw(unknownTypeName,
            unknownTypeValue) };
        var profileDto = _profileBuilder
            .WithResume()
            .WithContactInfo(contacts.ToList())
            .Build()
            .ToProfileParseDto();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,
            CancellationToken.None);

        // --- Then ---
        profile.Resume.Should().NotBeNull();
        profile.Resume!.ContactInfo.Should().NotBeNull();
        profile.Resume!.ContactInfo.Should().HaveCount(1);

        var actualContactInfo = profile.Resume!.ContactInfo!.First();
        actualContactInfo.Value.Should().Be(unknownTypeValue);
        actualContactInfo.ContactType.Name.Should().Be(unknownTypeName);
    }

    [Fact(DisplayName = "Map multiple contact infos with mixed types")]
    public async Task MapContactInfo_WithMultipleItems_MapsAllCorrectly()
    {
        // --- Given ---
        const string knownTypeName = "Email";
        const string knownTypeIcon = "bx bx-envelope";
        const string knownTypeValue = "test@test.com";

        const string unknownTypeName = "TypeDontExist";
        const string unknownTypeValue = "Some value";

        var emailType = ContactType.CreateInstance(Guid.Empty,
            knownTypeName,
            knownTypeIcon);
        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync(knownTypeName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(emailType));

        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync(unknownTypeName,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe<ContactType>.None);

        var contacts = new[]
        {
            new ContactInfoRaw(knownTypeName, knownTypeValue),
            new ContactInfoRaw(unknownTypeName, unknownTypeValue)
        };
        var profileDto = _profileBuilder
            .WithResume()
            .WithContactInfo(contacts.ToList())
            .Build()
            .ToProfileParseDto();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // --- Then ---
        profile.Resume.Should().NotBeNull();
        profile.Resume!.ContactInfo.Should().NotBeNull();
        profile.Resume!.ContactInfo.Should().HaveSameCount(contacts);

        var emailContact = profile.Resume!.ContactInfo!.Single(c => c.ContactType.Name == knownTypeName);

        emailContact.Should().NotBeNull();

        emailContact.ContactType.Should().BeSameAs(emailType);
        emailContact.ContactType.Name.Should().Be(knownTypeName);
        emailContact.ContactType.IconClass.Should().Be(knownTypeIcon);

        var adHocContact = profile.Resume!.ContactInfo!.Single(c => c.ContactType.Name == unknownTypeName);

        adHocContact.Should().NotBeNull();

        adHocContact.ContactType.Should().NotBeNull();
        adHocContact.ContactType.Name.Should().Be(unknownTypeName);
        adHocContact.ContactType.IconClass.Should().Be(ContactType.DefaultIconClass);
        adHocContact.ContactType.Id.Should().NotBe(Guid.Empty);
    }

    #endregion

    #region SkillSet Mapping Tests

    [Fact(DisplayName = "Map SkillSet aggregation when the DTO provides them")]
    public async Task MapSkillSet_WhenDtoProvidesThem()
    {
        // --- Given ---
        var profileDto = _profileBuilder
            .WithSkillSet()
            .Build()
            .ToProfileParseDto();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,
            CancellationToken.None);

        // --- Then ---
        profile.Should().NotBeNull();
        profile.SkillSet.Should().BeEquivalentTo(profileDto.SkillSet,
            options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map skills collection when the type exists")]
    public async Task MapSkills_WhenTypeExists_MapsCorrectly()
    {
        // --- Given ---
        var skills = new List<SkillRaw> { new("C#",
            "10") };
        var csharpSkill = SkillType.CreateInstance("C#",
            SkillCategory.CreateInstance("Programming"),
            new Uri("https://example.com/csharp-icon.png"));

        _skillTypeResolverMock
            .Setup(r => r.ResolveAsync(
                It.Is<string>(s => s.Equals("C#",
                    StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(csharpSkill));

        var profileDto = _profileBuilder
            .WithSkillSet()
            .WithSkills(skills)
            .Build()
            .ToProfileParseDto();

        var skillSet = SkillSet.CreateInstance1("Test Overview",
            new ProfileId(),
            new SkillSetId());
        skillSet.AddSkill(10,
            csharpSkill);
        var expectedSkill = skillSet.Skills.ToList();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,
            CancellationToken.None);

        // --- Then ---
        var currentSkills = profile.SkillSet!.Skills.ToList();
        currentSkills.Should().BeEquivalentTo(expectedSkill,
            options => options);
    }

    [Fact(DisplayName = "Map skills collection when the type is an alias")]
    public async Task MapSkills_WhenTypeIsAlias_MapsToCorrectBaseType()
    {
        // --- Given ---
        var skills = new List<SkillRaw> { new("csharp",
            "10") };
        var csharpSkill = SkillType.CreateInstance("C#",
            SkillCategory.CreateInstance("Programming"),
            new Uri("https://example.com/csharp-icon.png"));

        _skillTypeResolverMock
            .Setup(r => r.ResolveAsync("csharp",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(csharpSkill));

        var profileDto = _profileBuilder
            .WithSkillSet()
            .WithSkills(skills)
            .Build()
            .ToProfileParseDto();

        var skillSet = SkillSet.CreateInstance1("Test Overview",
            new ProfileId(),
            new SkillSetId());
        skillSet.AddSkill(10,
            csharpSkill);
        var expectedSkill = skillSet.Skills.ToList();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,
            CancellationToken.None);

        // --- Then ---
        var currentSkills = profile.SkillSet!.Skills.ToList();
        currentSkills.Should().BeEquivalentTo(expectedSkill);
    }

    [Fact(DisplayName = "Map skills when the type does not exist")]
    public async Task MapSkills_WhenTypeDoesNotExist_CreatesAdHocTypeWithDefaultIcon()
    {
        // --- Given ---
        var defaultCategory = SkillCategory.CreateInstance("AdHoc");

        var skills = new List<SkillRaw> { new("TypeDontExist",
            "10") };

        _skillTypeResolverMock
            .Setup(r => r.ResolveAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.None);
        _skillCategoryRepositoryMock
            .Setup(r => r.FirstAsync(
                It.IsAny<Expression<Func<SkillCategory, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(defaultCategory);

        var expectedAdHocType = SkillType.CreateInstance("TypeDontExist",
            SkillCategory.CreateInstance("AdHoc"));

        var skillSet = SkillSet.CreateInstance1("Test Overview",
            new ProfileId(),
            new SkillSetId());
        skillSet.AddSkill(10,
            expectedAdHocType);
        var expectedSkill = skillSet.Skills.ToList();

        var profileDto = _profileBuilder
            .WithSkillSet()
            .WithSkills(skills)
            .Build()
            .ToProfileParseDto();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,
            CancellationToken.None);

        // --- Then ---
        var currentSkills = profile.SkillSet!.Skills.ToList();
        currentSkills.Should().BeEquivalentTo(expectedSkill,
            options => options
                .Excluding(entity => entity.Id)
        );
    }

    [Fact(DisplayName = "Map multiple skills with mixed types")]
    public async Task MapSkills_WithMultipleItems_MapsAllCorrectly()
    {
        // --- Given ---
        const string unknownSkill = "TypeDontExist";
        const string knownSkill = "C#";
        const string knownSkillAlias = "csharp";

        var defaultCategory = SkillCategory.CreateInstance("AdHoc");
        var csharpSkill = SkillType.CreateInstance(knownSkill,
            SkillCategory.CreateInstance("Programming"),
            new Uri("https://example.com/csharp-icon.png"));

        _skillTypeResolverMock
            .Setup(r => r.ResolveAsync(It.Is<string>(knownSkillAlias,
                    StringComparer.InvariantCulture),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(csharpSkill));
        _skillTypeResolverMock
            .Setup(r => r.ResolveAsync(unknownSkill,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe<SkillType>.None);

        _skillCategoryRepositoryMock
            .Setup(r => r.FirstAsync(
                It.IsAny<Expression<Func<SkillCategory, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(defaultCategory);

        var expectedAdHocType = SkillType.CreateInstance(unknownSkill, defaultCategory);

        var skills = new[]
        {
            new SkillRaw(knownSkillAlias, "5"),
            new SkillRaw(unknownSkill, "2")
        };
        var profileDto = _profileBuilder
            .WithSkillSet()
            .WithSkills(skills.ToList())
            .Build()
            .ToProfileParseDto();

        var skillSet = SkillSet.CreateInstance1("Test Overview", new ProfileId(), new SkillSetId());
        skillSet.AddSkill(5,csharpSkill);
        skillSet.AddSkill(2,expectedAdHocType);
        var expectedSkills = skillSet.Skills.OrderBy(s => s.Rank).ToList();

        // --- When ---
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto,CancellationToken.None);

        // --- Then ---
        var currentSkills = profile.SkillSet!.Skills.OrderBy(s => s.Rank).ToList();
        currentSkills.Should().BeEquivalentTo(expectedSkills,
            options => options
                .Excluding(entity => entity.Id)
        );
    }

    #endregion
*/
}