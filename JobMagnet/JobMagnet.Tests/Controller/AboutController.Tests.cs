using JobMagnet.Controllers;
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
    }
}