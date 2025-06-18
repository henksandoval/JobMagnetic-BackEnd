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
        var profileDto = _profileBuilder
            .Build()
            .ToProfileParseDto();

        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        profile.Should().NotBeNull();
        profile.Should().BeEquivalentTo(profileDto, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map talents collection when the DTO provides them")]
    public async Task MapTalents_WhenDtoProvidesThem()
    {
        var profileDto = _profileBuilder
            .WithTalents()
            .Build()
            .ToProfileParseDto();

        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        profile.Should().NotBeNull();
        profile.Talents.Should().BeEquivalentTo(profileDto.Talents, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map testimonials collection when the DTO provides them")]
    public async Task MapTestimonials_WhenDtoProvidesThem()
    {
        var profileDto = _profileBuilder
            .WithTestimonials()
            .Build()
            .ToProfileParseDto();

        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        profile.Should().NotBeNull();
        profile.Testimonials.Should().BeEquivalentTo(profileDto.Testimonials, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map resume aggregation when the DTO provides them")]
    public async Task MapResume_WhenDtoProvidesThem()
    {
        var profileDto = _profileBuilder
            .WithResume()
            .Build()
            .ToProfileParseDto();

        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        profile.Should().NotBeNull();
        profile.Resume.Should().BeEquivalentTo(profileDto.Resume, options => options.ExcludingMissingMembers());
    }

    [Fact(DisplayName = "Map resume aggregation with contact info collection when the DTO provides them")]
    public async Task MapResumeWithContactInfo_WhenDtoProvidesThem()
    {
        var contactInfoCollection = PrepareAndGetContactInfoData();

        var profileDto = _profileBuilder
            .WithResume()
            .WithContactInfo(contactInfoCollection)
            .Build()
            .ToProfileParseDto();

        var profile = await _profileFactory.CreateProfileFromDtoAsync(profileDto, CancellationToken.None);

        profile.Should().NotBeNull();
        profile.Resume.Should().BeEquivalentTo(profileDto.Resume, options => options
            .Excluding(x => x!.ContactInfo)
            .ExcludingMissingMembers());

        var mappedContacts = profile.Resume!.ContactInfo!.OrderBy(c => c.Value).ToList();
        var sourceContacts = profileDto.Resume!.ContactInfo.OrderBy(c => c.Value).ToList();

        mappedContacts.Should().HaveSameCount(sourceContacts);

        for (var i = 0; i < sourceContacts.Count; i++)
        {
            var source = sourceContacts[i];
            var mapped = mappedContacts[i];

            mapped.Value.Should().Be(source.Value);

            if (source.ContactType == "TypeDontExist")
            {
                mapped.ContactType.Name.Should().Be(source.ContactType);
            }
            else
            {
                var expectedTypeName = await _contactTypeResolverMock.Object.ResolveAsync(
                    source.ContactType!, CancellationToken.None);
                mapped.ContactType.Name.Should().Be(expectedTypeName.Value.Name);
            }
        }
    }

    private List<ContactInfoRaw> PrepareAndGetContactInfoData()
    {
        var emailType = new ContactTypeEntity(1, "Email", "bx bx-envelope");
        var phoneType = new ContactTypeEntity(2, "Phone", "bx bx-mobile");
        var linkedInType = new ContactTypeEntity(3, "LinkedIn", "bx bx-linkedin");

        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync("EMAIL", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(emailType));

        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync("PHONE", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(phoneType));

        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync("Teléfono", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(phoneType));

        _contactTypeResolverMock
            .Setup(r => r.ResolveAsync("LinkedIn", It.IsAny<CancellationToken>()))
            .ReturnsAsync(Maybe.From(linkedInType));

        var contactInfoRaw = new List<ContactInfoRaw>
        {
            new("EMAIL", "test@test.com" ),
            new("PHONE", "123456789" ),
            new("LinkedIn", "linkedin.com/test"),
            new ("Teléfono", "+58 412457824"),
            new ("TypeDontExist", "Some value")
        };
        return contactInfoRaw;
    }
}