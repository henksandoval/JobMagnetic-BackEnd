using AutoFixture;
using FluentAssertions;
using JobMagnet.Controllers;
using JobMagnet.Models;
using JobMagnet.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace JobMagnet.Tests.Controller
{
    public class AboutControllerTests
    {
        private readonly Mock<IAboutService> serviceMock;
        private readonly AboutController controller;
        private readonly Fixture fixture;

        public AboutControllerTests()
        {
            fixture = new Fixture();
            serviceMock = new Mock<IAboutService>();
            controller = new AboutController(serviceMock.Object);
        }

        [Fact]
        public async Task ShouldIGetById()
        {
            //Arranger Preparar
            var modelExpected = fixture.Create<AboutModel>();
            serviceMock.Setup(aboutService => aboutService.GetById(modelExpected.Id)).ReturnsAsync(modelExpected);  

            //Act Ejecutar
            var actionResult = await controller.GetById(modelExpected.Id);

            //Assert Asegurar
            var result = actionResult as OkObjectResult;
            var resultModel = result!.Value as AboutModel;

            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            resultModel.Should().BeEquivalentTo(modelExpected);
        }

        [Fact]
        public async Task ShouldResponseNotFoundWhenTheRecordNotExists()
        {
            serviceMock.Setup(aboutService => aboutService.GetById(It.IsAny<int>())).ReturnsAsync(null as AboutModel);

            //Act
            var actionResult = await controller.GetById(1);

            //Assert
            var result = actionResult as NotFoundObjectResult;
            result!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            result.Value.Should().Be("Record [1] not found");
        }
    }
}