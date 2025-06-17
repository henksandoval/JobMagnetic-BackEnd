using System.Linq.Expressions;
using System.Net.Mime;
using AutoFixture;
using CSharpFunctionalExtensions;
using FluentAssertions;
using JobMagnet.Application.Services;
using JobMagnet.Application.UseCases.CvParser;
using JobMagnet.Application.UseCases.CvParser.Commands;
using JobMagnet.Application.UseCases.CvParser.DTO.RawDTOs;
using JobMagnet.Application.UseCases.CvParser.Ports;
using JobMagnet.Domain.Core.Entities;
using JobMagnet.Domain.Ports.Repositories.Base;
using JobMagnet.Domain.Services;
using JobMagnet.Shared.Tests.Fixtures;
using JobMagnet.Shared.Tests.Fixtures.Builders;
using Moq;
using Shouldly;

namespace JobMagnet.Unit.Tests.UseCases;

public class CvParserHandlerShould
{
    private readonly IFixture _fixture = FixtureBuilder.Build();
    private readonly Mock<IRawCvParser> _rawCvParserMock;
    private readonly Mock<IProfileSlugGenerator> _slugGeneratorMock;
    private readonly Mock<IContactTypeResolverService> _contactTypeResolverMock;
    private readonly Mock<ISkillTypeResolverService> _skillTypeResolverMock;
    private readonly Mock<ICommandRepository<ProfileEntity>> _profileCommandRepositoryMock;
    private readonly Mock<IProfileFactoryService> _profileFactoryMock;

    private readonly CvParserHandler _handler;

    public CvParserHandlerShould()
    {
        _rawCvParserMock = new Mock<IRawCvParser>();
        _profileCommandRepositoryMock = new Mock<ICommandRepository<ProfileEntity>>();
        _slugGeneratorMock = new Mock<IProfileSlugGenerator>();
        _profileFactoryMock = new Mock<IProfileFactoryService>();
        _contactTypeResolverMock = new Mock<IContactTypeResolverService>();
        _skillTypeResolverMock = new Mock<ISkillTypeResolverService>();

        _handler = new CvParserHandler(
            _rawCvParserMock.Object,
            _profileCommandRepositoryMock.Object,
            _slugGeneratorMock.Object,
            _profileFactoryMock.Object,
            _contactTypeResolverMock.Object,
            _skillTypeResolverMock.Object);
    }

    [Fact(DisplayName = "Resolve existing contact types, create new ones, and enrich profile within transaction")]
    public async Task ResolveExistingAndCreateNewContactTypesAndEnrichProfile()
    {
        // Given
        var contactInfoRaw = PrepareContactInfoData();

        var profileRawBuilder = new ProfileRawBuilder(_fixture);
        var profileRaw = profileRawBuilder
            .WithResume()
            .WithContactInfo(contactInfoRaw)
            .WithSkillSet()
            .Build();

        _rawCvParserMock.Setup(p => p.ParseAsync(It.IsAny<Stream>()))
            .ReturnsAsync(profileRaw);

        _slugGeneratorMock.Setup(g => g.GenerateProfileSlug(It.IsAny<ProfileEntity>())).Returns("test-slug");

        // WHEN
        var result = await _handler.ParseAsync(new CvParserCommand(new MemoryStream(), "fileName", MediaTypeNames.Text.Plain));

        // THEN
        _profileCommandRepositoryMock.Verify(
            repo => repo.CreateAsync(It.IsAny<ProfileEntity>(), It.IsAny<CancellationToken>()),
            Times.Once);

        result.ShouldNotBeNull();
        result.UserEmail.Should().Be("test@test.com");
    }

    private List<ContactInfoRaw> PrepareContactInfoData()
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