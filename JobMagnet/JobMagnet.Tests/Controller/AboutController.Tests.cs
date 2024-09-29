using FluentAssertions;
using JobMagnet.Controllers;
using JobMagnet.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Tests.Controller
{
    public class AboutControllerTests
    {
        [Fact]
        public void ItShouldShowOk()
        {
            //Arranger Preparar
            var controller = new AboutController();

            //Act Ejecutar
            var respuesta = controller.GetOk();

            var okResult = respuesta as OkObjectResult;
            //Assert Asegurar

            Assert.NotNull(okResult); 
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
        }


        [Fact]
        public void ItShouldShowAllTheData()
        {
            //Arranger Preparar
            var controller = new AboutController();
            var about = new AboutEntity()
            {
                Id = 1,
                ImageUrl = "https://bootstrapmade.com/content/demo/MyResume/assets/img/profile-img.jpg" 
            };

            var resultEsperado = new OkResult();


            //Act Ejecutar
            var respuesta = controller.GetByID(about);

            //Assert Asegurar
            var okResult = respuesta as OkObjectResult;
            okResult.Should().BeEquivalentTo(resultEsperado);

        }
    }
}