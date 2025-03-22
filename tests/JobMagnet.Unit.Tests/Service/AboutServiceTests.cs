using AutoFixture;
using FluentAssertions;
using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repositories.Interface;
using JobMagnet.Service;
using Moq;

namespace JobMagnet.Unit.Tests.Service;

public class AboutServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IQueryRepository<AboutEntity>> _queryRepositoryMock;
    private readonly AboutService _service;
    private readonly Mock<ICommandRepository<AboutEntity>> _commandRepositoryMock;

    public AboutServiceTests()
    {
        _fixture = new Fixture();
        _queryRepositoryMock = new Mock<IQueryRepository<AboutEntity>>();
        _commandRepositoryMock = new Mock<ICommandRepository<AboutEntity>>();
        _service = new AboutService(_queryRepositoryMock.Object, _commandRepositoryMock.Object);
    }

    [Fact]
    public async Task HeShouldReturnTheId_WhenThereIsARecord()
    {
        //Arranger Preparar
        var entity = _fixture.Create<AboutEntity>();
        _queryRepositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(entity);


        //Act Ejecutar
        var aboutModel = await _service.GetByIdAsync(76);
        var expectedModel = Mappers.MapAboutModel(entity);

        //Assert Asegurar
        aboutModel.Should().BeEquivalentTo(expectedModel);
        aboutModel.Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task ShouldSaveARecordWhenARecordIsCreated()
    {
        //Arranger Preparar
        var createAbout = _fixture.Create<AboutCreateRequest>();
        var expectedEntity = Mappers.MapAboutCreate(createAbout);
        _commandRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<AboutEntity>())).Returns(Task.CompletedTask);

        //Act Ejecutar
        var aboutModel = await _service.Create(createAbout);

        //Assert Asegurar
        var expectedModel = Mappers.MapAboutModel(expectedEntity);
        _commandRepositoryMock.Verify(repository => repository.CreateAsync(It.IsAny<AboutEntity>()), Times.Once());
        aboutModel.Should().BeEquivalentTo(expectedModel, options => options.Excluding(x => x.Id));
    }
}