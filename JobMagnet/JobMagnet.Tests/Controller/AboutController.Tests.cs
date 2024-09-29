using FluentAssertions;
using JobMagnet.Controllers;
using JobMagnet.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

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
                ImageUrl = "https://bootstrapmade.com/content/demo/MyResume/assets/img/profile-img.jpg",
                Description = "description",
                Text = "UI/UX ",
                Hobbies = "In my free",
                Birthday = "16/051995",
                WebSite = "www.example.com",
                PhoneNumber = 641051233,
                City = "Zaragoza, España",
                Age = 29,
                Degree = "Master",
                Email = "alexandrai.marvala@gmail.com",
                Freelance = "Available",
                WorkExperience = "Developed and maintained web applications for various clients"
            };

            var resultEsperado = new OkResult();


            //Act Ejecutar
            var respuesta = controller.GetByID(about);

            //Assert Asegurar
            var okResult = respuesta as OkObjectResult;
            okResult.Should().BeEquivalentTo(resultEsperado);
        }

        //[Fact]
        //public void WhenADataIsCreated_ItShouldReturnTrue() 
        //{

        //}
    }
  
}