using AutoFixture;
using FluentAssertions;
using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repositories.Interface;
using JobMagnet.Service;
using Moq;

namespace JobMagnet.Unit.Tests.Service;

public class SkillServiceTests
{
    private readonly Mock<ICommandRepository<SkillEntity>> _commandRepositoryMock;
    private readonly Fixture _fixture;
    private readonly SkillService _service;

    public SkillServiceTests()
    {
        _fixture = new Fixture();
        _commandRepositoryMock = new Mock<ICommandRepository<SkillEntity>>();
        _service = new SkillService(_commandRepositoryMock.Object);
    }

    [Fact]
    public async Task ShouldSaveAWhenARecordIsCreated()
    {
        //Arranger Preparar
        var createSkill = _fixture.Create<SkillCreateRequest>();
        var expectedEntity = Mappers.MapSkillCreate(createSkill);
        _commandRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<SkillEntity>())).Returns(Task.CompletedTask);

        //Act Ejecutar
        var skillModel = await _service.Create(createSkill);

        //Assert Asegurar
        var expectedModel = Mappers.MapSkillModel(expectedEntity);
        _commandRepositoryMock.Verify(repository => repository.CreateAsync(It.IsAny<SkillEntity>()), Times.Once());
        skillModel.Should().BeEquivalentTo(expectedModel, options => options.Excluding(x => x.Id));
    }
}