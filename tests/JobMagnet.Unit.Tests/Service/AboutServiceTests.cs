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

namespace JobMagnet.Unit.Tests.Service
{
    public class AboutServiceTests
    {
        private Fixture fixture;
        private readonly Mock<IAboutRepository<AboutEntity>> repositoryMock;
        private readonly AboutService service;

        public AboutServiceTests()
        {
            fixture = new Fixture();
            repositoryMock = new Mock<IAboutRepository<AboutEntity>>();
            service = new AboutService(repositoryMock.Object);
        }

        [Fact]
        public async Task HeShouldReturnTheId_WhenThereIsARecord()
        {
            //Arranger Preparar
            var entity = fixture.Create<AboutEntity>();
            repositoryMock.Setup(repository => repository.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(entity);


            //Act Ejecutar
            var aboutModel = await service.GetById(76);
            var expectedModel = Mappers.MapAboutModel(entity);

            //Assert Asegurar
            aboutModel.Should().BeEquivalentTo(expectedModel);
            aboutModel.Id.Should().Be(entity.Id);
        }

        [Fact]
        public async Task ShouldSaveARecordWhenARecordIsCreated()
        {
            //Arranger Preparar
            var createAbout = fixture.Create<AboutCreateRequest>();
            var expectedEntity = Mappers.MapAboutCreate(createAbout);
            repositoryMock.Setup(r => r.CreateAsync(It.IsAny<AboutEntity>())).Returns(Task.CompletedTask);

            //Act Ejecutar
            var aboutModel = await service.Create(createAbout);

            //Assert Asegurar
            var expectedModel = Mappers.MapAboutModel(expectedEntity);
            repositoryMock.Verify(repository => repository.CreateAsync(It.IsAny<AboutEntity>()), Times.Once());
            aboutModel.Should().BeEquivalentTo(expectedModel, options => options.Excluding(x => x.Id));
            }
        }
    }