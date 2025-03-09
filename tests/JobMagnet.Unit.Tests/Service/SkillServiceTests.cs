using AutoFixture;
using AutoMapper;
using FluentAssertions;
using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repository.Interface;
using JobMagnet.Service;
using Moq;


namespace JobMagnet.Unit.Tests.Service
{
    public class SkillServiceTests
    {
        private readonly Fixture fixture;
        private readonly Mock<ISkillRepository<SkillEntity>> repositoryMock;
        private readonly SkillService service;
        public SkillServiceTests()
        {
            fixture = new Fixture();
            repositoryMock = new Mock<ISkillRepository<SkillEntity>>();
            service = new SkillService(repositoryMock.Object);
        }

        [Fact]
        public async Task ShouldSaveAWhenARecordIsCreated() 
        {
            //Arranger Preparar
            var createSkill = fixture.Create<SkillCreateRequest>();
            var expectedEntity = Mappers.MapSkillCreate(createSkill);
            repositoryMock.Setup(r => r.CreateAsync(It.IsAny<SkillEntity>())).Returns(Task.CompletedTask);

            //Act Ejecutar
            var skillModel = await service.Create(createSkill);

            //Assert Asegurar
            var expectedModel = Mappers.MapSkillModel(expectedEntity);
            repositoryMock.Verify(reposity => reposity.CreateAsync(It.IsAny<SkillEntity>()), Times.Once());
            skillModel.Should().BeEquivalentTo(expectedModel, options => options.Excluding(x => x.Id));
        }
    }
}
