using AutoFixture;
using CSharpFunctionalExtensions;
using FluentAssertions;
using JobMagnet.Application.Factories;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Application.UseCases.CvParser.Mappers;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
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
    private readonly Mock<IQueryRepository<SkillType, int>> _skillTypeRepository;
    private readonly Mock<IQueryRepository<ContactTypeEntity, int>> _contactTypeRepository;
    private readonly Mock<IContactTypeResolverService> _contactTypeResolverMock;

    public ProfileFactoryShould()
    {
        _profileBuilder = new ProfileRawBuilder(_fixture);
        _skillTypeRepository = new Mock<IQueryRepository<SkillType, int>>();
        _contactTypeRepository = new Mock<IQueryRepository<ContactTypeEntity, int>>();
        _contactTypeResolverMock = new Mock<IContactTypeResolverService>();
        _profileFactory = new ProfileFactory(
            _skillTypeRepository.Object,
            _contactTypeRepository.Object,
            _contactTypeResolverMock.Object);
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

    [Fact(DisplayName = "Map resume aggregation with contact info collection when the DTO provides them")]
    public async Task MapResumeWithContactInfo_WhenDtoProvidesThem()
    {
        // Given
        var typeMappings = GetContactTypeEntities();
        SetupContactTypeResolverMock(typeMappings);

        var contactInfoCollection = new List<ContactInfoRaw>
        {
            new("EMAIL", "test@test.com"),
            new("PHONE", "123456789"),
            new("LinkedIn", "linkedin.com/test"),
            new("Teléfono", "+58 412457824"),
            new("TypeDontExist", "Some value")
        };

        var profileDto = _profileBuilder
            .WithResume()
            .WithContactInfo(contactInfoCollection)
            .Build()
            .ToProfileParseDto();

        var expectedContactInfo = contactInfoCollection
            .Select(source =>
            {
                typeMappings.TryGetValue(source.ContactType!, out var resolvedType);
                var contactType = resolvedType ?? new ContactTypeEntity(source.ContactType!);
                if (resolvedType == null)
                    contactType.SetDefaultIcon();
                return new ContactInfoEntity(0, source.Value!, contactType);
            })
            .OrderBy(c => c.Value)
            .ToList();

        // When
        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        // Then
        profile.Should().NotBeNull();
        profile.Resume.Should().BeEquivalentTo(profileDto.Resume, options => options
            .Excluding(x => x!.ContactInfo)
            .ExcludingMissingMembers());

        var contactInfo = profile.Resume!.ContactInfo!.OrderBy(c => c.Value).ToList();
        contactInfo.Should().HaveSameCount(expectedContactInfo);
        var firstContactInfoEntity = contactInfo[0];
        var firstContactInfoEntityExpected = expectedContactInfo[0];
        firstContactInfoEntity.Should().BeEquivalentTo(firstContactInfoEntityExpected);
        contactInfo.Should().BeEquivalentTo(expectedContactInfo, options => options
            .WithMapping<ContactInfoEntity>(expect => expect.Value, sut => sut.Value)
            .ExcludingMissingMembers()
        );
    }

    private static Dictionary<string, ContactTypeEntity> GetContactTypeEntities()
    {
        var emailType = new ContactTypeEntity(1, "EMAIL", "bx bx-envelope");
        var phoneType = new ContactTypeEntity(2, "Phone", "bx bx-mobile");
        var linkedInType = new ContactTypeEntity(3, "LinkedIn", "bx bx-linkedin");

        var typeMappings = new Dictionary<string, ContactTypeEntity>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "Email", emailType },
            { "Phone", phoneType },
            { "Teléfono", phoneType },
            { "LinkedIn", linkedInType }
        };
        return typeMappings;
    }

    private void SetupContactTypeResolverMock(Dictionary<string, ContactTypeEntity> typeMappings)
    {
        foreach (var mapping in typeMappings)
        {
            _contactTypeResolverMock
                .Setup(r => r.ResolveAsync(
                    It.Is<string>(s => s.Equals(mapping.Key, StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Maybe.From(mapping.Value));
        }
    }
}