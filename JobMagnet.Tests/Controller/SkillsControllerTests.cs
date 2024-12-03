using AutoFixture;
using FluentAssertions;
using JobMagnet.Controllers;
using JobMagnet.Models;
using JobMagnet.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
namespace JobMagnet.Tests.Controller
{
    public class SkillsControllerTests
    {
        private Mock<ISkillService> serviceMock;
        private readonly Fixture fixture;
        private SkillsController controller;

        public SkillsControllerTests()
        {
            fixture = new Fixture();
            serviceMock = new Mock<ISkillService>();
            controller = new SkillsController(serviceMock.Object);
        }

        [Fact]
        public void MustReturnOk()
        {
            //Arranger Preparar

            //Act Ejecutar
            var respuesta = controller.GetOk();

            //Assert Asegurar
            var resultEsperado = respuesta as OkResult;
            resultEsperado.Should().NotBeNull();
            resultEsperado!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            resultEsperado.Should().BeEquivalentTo(respuesta);
        }

        [Fact]
        public async Task WhenYouCreateASkillYouShouldSaveItAndReturnOk()
        {
            //Arranger Preparar
            var skillCreateRequest = fixture.Create<SkillCreateRequest>();
            var skillModel = fixture.Create<SkillModel>();
            serviceMock.Setup(skillService => skillService.Create(skillCreateRequest)).ReturnsAsync(skillModel);

            //Act Ejecutar

            var actionResult = await controller.Create(skillCreateRequest);

            //Assert Asegurar
            var result = actionResult as CreatedAtRouteResult;
            result!.StatusCode.Should().Be((int)HttpStatusCode.Created);
            result.Value.Should().Be(skillModel.Id);

        }
    }
}

