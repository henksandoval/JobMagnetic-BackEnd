using FluentAssertions;
using JobMagnet.Controllers;
using JobMagnet.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JobMagnet.Tests.Controller
{
    public class SummaryControllerTests
    {
        public SummaryControllerTests()
        {
        }

        [Fact] 
        public void ShouldReturnAnOk()
        {
            //Arranger Preparar
            var controller = new SummaryController();

            //Act Ejecutar
            var response = controller.Get();

            //Assert Asegurar

            var expectedResult = response as OkResult;
            expectedResult.Should().BeEquivalentTo(response);
            expectedResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            expectedResult.Should().BeEquivalentTo(response);
        }

        [Fact]
        public void ShouldReturnAnEntity()
        {
            //Arranger Preparar
            var controller = new SummaryController();
            var summaryEntity = new SummaryEntity { About = "Me llamo Alexandra" };

            //Act Ejecutar
            var actionResult = controller.Get();
            var okResult = actionResult as OkObjectResult;
            var result = okResult.Value as SummaryEntity;

            //Assert Asegurar
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(summaryEntity);
        }
    }
}
