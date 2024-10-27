using FluentAssertions;
using JobMagnet.Controllers;
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
            var response = controller.GetOk();

            //Assert Asegurar

            var expectedResult = response as OkResult;
            expectedResult.Should().BeEquivalentTo(response);
            expectedResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            expectedResult.Should().BeEquivalentTo(response);
        }
    }
}
