using System.Linq.Expressions;
using System.Net.Mime;
using AutoFixture;
using CSharpFunctionalExtensions;
using FluentAssertions;
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
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProfileSlugGenerator> _slugGeneratorMock;
    private readonly Mock<IQueryRepository<ContactTypeAliasEntity, int>> _contactTypeAliasQueryRepositoryMock;
    private readonly Mock<IContactTypeResolverService> _contactTypeResolverMock;
    private readonly Mock<ICommandRepository<ProfileEntity>> _profileCommandRepositoryMock;
    private readonly Mock<ICommandRepository<PublicProfileIdentifierEntity>> _publicIdentifierCommandRepositoryMock;

    private readonly CvParserHandler _handler;

    public CvParserHandlerShould()
    {
        _rawCvParserMock = new Mock<IRawCvParser>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _slugGeneratorMock = new Mock<IProfileSlugGenerator>();
        _contactTypeResolverMock = new Mock<IContactTypeResolverService>();
        _contactTypeAliasQueryRepositoryMock = new Mock<IQueryRepository<ContactTypeAliasEntity, int>>();
        _profileCommandRepositoryMock = new Mock<ICommandRepository<ProfileEntity>>();
        _publicIdentifierCommandRepositoryMock = new Mock<ICommandRepository<PublicProfileIdentifierEntity>>();

        _unitOfWorkMock.Setup(uow => uow.ProfileRepository).Returns(_profileCommandRepositoryMock.Object);
        _unitOfWorkMock.Setup(uow => uow.PublicProfileIdentifierRepository).Returns(_publicIdentifierCommandRepositoryMock.Object);

        _handler = new CvParserHandler(
            _rawCvParserMock.Object,
            _unitOfWorkMock.Object,
            _slugGeneratorMock.Object,
            _contactTypeResolverMock.Object);
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
            .Build();

        _rawCvParserMock.Setup(p => p.ParseAsync(It.IsAny<Stream>()))
            .ReturnsAsync(profileRaw);

        _contactTypeAliasQueryRepositoryMock
            .Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<ContactTypeAliasEntity, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ContactTypeAliasEntity("Email", new ContactTypeEntity("Email")));

        _slugGeneratorMock.Setup(g => g.GenerateProfileSlug(It.IsAny<ProfileEntity>())).Returns("test-slug");

        _unitOfWorkMock
            .Setup(uow => uow.ExecuteOperationInTransactionAsync(It.IsAny<Func<Task>>(), It.IsAny<CancellationToken>()))
            .Callback<Func<Task>, CancellationToken>((action, cancellationToken) => action().Wait(cancellationToken))
            .Returns(Task.CompletedTask);

        // WHEN
        var result = await _handler.ParseAsync(new CvParserCommand(new MemoryStream(), "fileName", MediaTypeNames.Text.Plain));

        // THEN
        _profileCommandRepositoryMock.Verify(
            repo => repo.CreateAsync(It.IsAny<ProfileEntity>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _publicIdentifierCommandRepositoryMock.Verify(
            repo => repo.CreateAsync(It.IsAny<PublicProfileIdentifierEntity>(), It.IsAny<CancellationToken>()),
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