using AutoFixture;
using FluentAssertions;
using JobMagnet.Controllers;
using JobMagnet.Entities;
using JobMagnet.Models;
using JobMagnet.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace JobMagnet.Tests.Controller
{
    public class SummaryControllerTests
    {
        private readonly Mock<ISummaryService> serviceMock;
        private readonly SummaryController controller;
        private readonly Fixture fixture;
        public SummaryControllerTests()
        {
            fixture = new Fixture();
            serviceMock = new Mock<ISummaryService>();
            controller = new SummaryController(serviceMock.Object);
        }

        [Fact] 
        public async Task ShouldReturnAnOk()
        {
            //Arranger Preparar
            var modelExpected = fixture.Create<SummaryModel>();
            
            //Act Ejecutar
            var response = await controller.Get(modelExpected.Id);

            //Assert Asegurar

            var expectedResult = response as OkResult;
            expectedResult.Should().BeEquivalentTo(response);
            expectedResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            expectedResult.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task ShouldReturnAnEntity()
        {
            //Arranger Preparar
            
            var summaryEntity = new SummaryEntity { About = "Me llamo Alexandra" };

            //Act Ejecutar
            var actionResult = await controller.Get(summaryEntity.Id);
            var okResult = actionResult as OkObjectResult;
            var result = okResult!.Value as SummaryEntity;

            //Assert Asegurar
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(summaryEntity);
        }

        [Fact]
        public async Task ShouldReturnAllRecords()
        {
            //Arranger Preparar
            var modelExpected = fixture.Create<SummaryModel>();
            serviceMock.Setup(summaryService => summaryService.GetById(modelExpected.Id)).ReturnsAsync(modelExpected);
            
            //Act Ejecutar
            var actionResult = await controller.Get(modelExpected.Id);

            //Assert Asegurar
            var okResult = actionResult as OkObjectResult;
            var resultModel = okResult!.Value as SummaryModel;
            resultModel!.Id.Should().Be(modelExpected.Id);
            resultModel.Should().BeEquivalentTo(modelExpected);
        }
    }
}
