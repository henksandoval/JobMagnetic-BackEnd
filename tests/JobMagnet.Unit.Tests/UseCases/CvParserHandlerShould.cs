 using System.Net.Mime;
using AutoFixture;
using AwesomeAssertions;
using CSharpFunctionalExtensions;

using JobMagnet.Application.Factories;
using JobMagnet.Application.UseCases.CvParser;
using JobMagnet.Application.UseCases.CvParser.Commands;
using JobMagnet.Application.UseCases.CvParser.DTO.ParsingDTOs;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Domain.Aggregates.Contact;
using JobMagnet.Domain.Aggregates.Profiles;
using JobMagnet.Domain.Aggregates.Profiles.Entities;

using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Services;
using JobMagnet.Shared.Abstractions;
using JobMagnet.Shared.Tests.Abstractions;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using Moq;


namespace JobMagnet.Unit.Tests.UseCases;

public class CvParserHandlerShould
{
    private readonly IClock _clock;
    private readonly IGuidGenerator _guidGenerator;
    private readonly Mock<IContactTypeResolverService> _contactTypeResolverMock;
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly CvParserHandler _handler;
    private readonly Mock<IGenericCommandRepository<Profile>> _profileCommandRepositoryMock;
    private readonly Mock<IProfileFactory> _profileFactoryMock;
    private readonly Mock<IRawCvParser> _rawCvParserMock;
    private readonly Mock<IProfileSlugGenerator> _slugGeneratorMock;

    public CvParserHandlerShould()
    {
        _rawCvParserMock = new Mock<IRawCvParser>();
        _profileCommandRepositoryMock = new Mock<IGenericCommandRepository<Profile>>();
        _slugGeneratorMock = new Mock<IProfileSlugGenerator>();
        _profileFactoryMock = new Mock<IProfileFactory>();
        _contactTypeResolverMock = new Mock<IContactTypeResolverService>();
        _clock = new DeterministicClock();
        _guidGenerator = new SequentialGuidGenerator();

        _handler = new CvParserHandler(
            _guidGenerator,
            _clock,
            _rawCvParserMock.Object,
            _profileCommandRepositoryMock.Object,
            _slugGeneratorMock.Object,
            _profileFactoryMock.Object);
    }

    [Fact(DisplayName = "Resolve existing contact types, create new ones, and enrich profile within transaction")]
    public async Task ResolveExistingAndCreateNewContactTypesAndEnrichProfile()
    {
        // --- Given ---
        var contactInfoRaw = PrepareContactInfoData();

        var profileRawBuilder = new ProfileRawBuilder(_fixture);
        var profileRaw = profileRawBuilder
            .WithResume()
            .WithContactInfo(contactInfoRaw)
            .WithSkillSet()
            .Build();

        var contactInfoEmail = contactInfoRaw.FirstOrDefault(c => c.ContactType == "EMAIL");
        var resumeEntity = _fixture.Create<ProfileHeader>();
        var contactType = ContactType.CreateInstance(_guidGenerator, _clock, contactInfoEmail!.ContactType ?? string.Empty);
        resumeEntity.AddContactInfo(_guidGenerator, _clock, contactInfoEmail.Value!, contactType);

        var profileEntity = Profile.CreateInstance(
            _guidGenerator,
            _clock,
            profileRaw.FirstName,
            profileRaw.LastName);

        profileEntity.AddResume(resumeEntity);

        _rawCvParserMock.Setup(p => p.ParseAsync(It.IsAny<Stream>()))
            .ReturnsAsync(profileRaw);
        _slugGeneratorMock.Setup(g => g.GenerateProfileSlug(It.IsAny<Profile>())).Returns("test-slug");
        _profileFactoryMock.Setup(p => p.CreateProfileFromDtoAsync(It.IsAny<ProfileParseDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(profileEntity);

        // --- When ---
        var result = await _handler.ParseAsync(new CvParserCommand(new MemoryStream(), "fileName", MediaTypeNames.Text.Plain));

        // --- Then ---
        _profileCommandRepositoryMock.Verify(
            repo => repo.CreateAsync(It.IsAny<Profile>(), It.IsAny<CancellationToken>()),
            Times.Once);

        result.Should().NotBeNull();
        result.UserEmail.Should().Be("test@test.com");
    }

    private List<ContactInfoRaw> PrepareContactInfoData()
    {
        var emailType = ContactType.CreateInstance(_guidGenerator, _clock,  "Email", "bx bx-envelope");
        var phoneType = ContactType.CreateInstance(_guidGenerator, _clock, "Phone", "bx bx-mobile");
        var linkedInType = ContactType.CreateInstance(_guidGenerator, _clock, "LinkedIn", "bx bx-linkedin");

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
            new("EMAIL", "test@test.com"),
            new("PHONE", "123456789"),
            new("LinkedIn", "linkedin.com/test"),
            new("Teléfono", "+58 412457824"),
            new("TypeDontExist", "Some value")
        };
        return contactInfoRaw;
    }
}