using AutoFixture;
using AutoMapper;
using FluentAssertions;
using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repository.Interface;
using JobMagnet.Service;
using Moq;


namespace JobMagnet.Tests.Service
{
    public class SkillServiceTests
    {
        private readonly Fixture fixture;
        private readonly Mapper mapper;
        private readonly Mock<ISkillRepository<SkillEntity>> repositoryMock;
        private readonly SkillService service;

        public SkillServiceTests()
        {
            fixture = new Fixture();
            var configuration = new MapperConfiguration(configure => { configure.AddProfile<MapperProfiles>(); });
            mapper = new Mapper(configuration);
            repositoryMock = new Mock<ISkillRepository<SkillEntity>>();
            service = new SkillService(mapper, repositoryMock.Object);

        }

        [Fact]
        public async Task ShouldSaveAWhenARecordIsCreated() 
        {
            //Arranger Preparar
            var createSkill = fixture.Create<SkillCreateRequest>();
            //Act Ejecutar
            var skillModel = await service.Create(createSkill);

            //Assert Asegurar
            var expectedEntity = mapper.Map<SkillEntity>(createSkill);
            repositoryMock.Verify(reposity => reposity.CreateAsync(It.IsAny<SkillEntity>()), Times.Once());
            skillModel.Should().BeEquivalentTo(expectedEntity);
        }
    }
}
