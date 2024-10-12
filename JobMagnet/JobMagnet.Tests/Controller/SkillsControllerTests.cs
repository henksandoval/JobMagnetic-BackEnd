using FluentAssertions;
using JobMagnet.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
namespace JobMagnet.Tests.Controller
{
    public class SkillsControllerTests
    {
        public SkillsControllerTests()
        {
        }

        [Fact]
        public void MustReturnOk()
        {
            var controller = new SkillsController();

            //Act Ejecutar
            var respuesta = controller.GetOk();

            //Assert Asegurar
            var resultEsperado = respuesta as OkResult;
            resultEsperado.Should().NotBeNull();
            resultEsperado!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            resultEsperado.Should().BeEquivalentTo(respuesta);
        }
    }
}

