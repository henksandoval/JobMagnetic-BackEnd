using System.Linq.Expressions;
using System.Net.Mime;
using AutoFixture;
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
    private readonly Mock<IQueryRepository<ContactTypeEntity, long>> _contactTypeQueryRepositoryMock;
    private readonly Mock<ICommandRepository<ProfileEntity>> _profileCommandRepositoryMock;
    private readonly Mock<ICommandRepository<PublicProfileIdentifierEntity>> _publicIdentifierCommandRepositoryMock;

    private readonly CvParserHandler _handler;

    public CvParserHandlerShould()
    {
        _rawCvParserMock = new Mock<IRawCvParser>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _slugGeneratorMock = new Mock<IProfileSlugGenerator>();
        _contactTypeQueryRepositoryMock = new Mock<IQueryRepository<ContactTypeEntity, long>>();
        _profileCommandRepositoryMock = new Mock<ICommandRepository<ProfileEntity>>();
        _publicIdentifierCommandRepositoryMock = new Mock<ICommandRepository<PublicProfileIdentifierEntity>>();

        _unitOfWorkMock.Setup(uow => uow.ProfileRepository).Returns(_profileCommandRepositoryMock.Object);
        _unitOfWorkMock.Setup(uow => uow.PublicProfileIdentifierRepository).Returns(_publicIdentifierCommandRepositoryMock.Object);

        _handler = new CvParserHandler(
            _rawCvParserMock.Object,
            _unitOfWorkMock.Object,
            _slugGeneratorMock.Object,
            _contactTypeQueryRepositoryMock.Object);
    }

    [Fact(DisplayName = "Resolve existing contact types, create new ones, and enrich profile within transaction")]
    public async Task ResolveExistingAndCreateNewContactTypesAndEnrichProfile()
    {
        // Given
        var contactInfoRaw = new List<ContactInfoRaw>
        {
            new("Email", "test@test.com" ),
            new("Phone", "123456789" ),
            new("LinkedIn", "linkedin.com/test")
        };

        var profileRawBuilder = new ProfileRawBuilder(_fixture);
        var profileRaw = profileRawBuilder
            .WithResume()
            .WithContactInfo(contactInfoRaw)
            .Build();

        _rawCvParserMock.Setup(p => p.ParseAsync(It.IsAny<Stream>()))
            .ReturnsAsync(profileRaw);

        var existingContactTypesInDb = new List<ContactTypeEntity>
        {
            new(1, "Email", "bx bx-envelope"),
            new(2, "Phone", "bx bx-mobile")
        };

        _contactTypeQueryRepositoryMock
            .Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<ContactTypeEntity, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingContactTypesInDb);

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
}