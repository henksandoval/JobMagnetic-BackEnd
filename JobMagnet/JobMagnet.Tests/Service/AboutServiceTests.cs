using AutoFixture;
using AutoMapper;
using FluentAssertions;
using JobMagnet.AutoMapper;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Repository.Interface;
using JobMagnet.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace JobMagnet.Tests.Service
{
    public class AboutServiceTests
    {
        private Fixture fixture;
        private readonly Mapper mapper;
        private readonly Mock<IAboutRepository<AboutEntity>> repositoryMock;
        private readonly AboutService service;

        public AboutServiceTests()
        {
            fixture = new Fixture();
            var configuration = new MapperConfiguration(configure => { configure.AddProfile<MapperProfiles>(); });
            mapper = new Mapper(configuration);
            repositoryMock = new Mock<IAboutRepository<AboutEntity>>();
            service = new AboutService(mapper,repositoryMock.Object);
        }

        [Fact]
        public async Task HeShouldReturnTheId_WhenThereIsARecord()
        {
            //Arranger Preparar
            var entity = fixture.Create<AboutEntity>();
            repositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(entity);
            

            //Act Ejecutar
            var aboutModel = await service.GetById(76);
            var expectedModel = mapper.Map<AboutModel>(entity);

            //Assert Asegurar
            aboutModel.Should().BeEquivalentTo(expectedModel);
            aboutModel.Id.Should().Be(entity.Id);
        }

        [Fact]
        public async Task ShouldSaveARecordWhenARecordIsCreated()
        {
            //Arranger Preparar
            var createAbout = fixture.Create<AboutCreateRequest>();


            //Act Ejecutar
            var aboutModel = service.Create(createAbout);
            var expectedEntity = mapper.Map<AboutEntity>(createAbout);


            //Assert Asegurar
            repositoryMock.Verify(repository => repository.CreateAsync(It.IsAny<AboutEntity>()), Times.Once());
            aboutModel.Should().NotBeEquivalentTo(expectedEntity);
            
        }
    }
}
