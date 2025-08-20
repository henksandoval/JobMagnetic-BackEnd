using System.Text.Json;
using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Host.Controllers.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace JobMagnet.Unit.Tests.AuthControllerShould.cs;

public class AuthControllerShould
{
    private readonly IFixture _fixture;
    private readonly Mock<IAuthUserHandler> _handlerMock; 
    private readonly AuthController _controller;
    
    public AuthControllerShould()
    {
        _fixture = new Fixture();
        _handlerMock = new Mock<IAuthUserHandler>();
        _controller = new AuthController(_handlerMock.Object);
    }

    [Fact]
    public async Task ReturnToken_WhenCredentialsAreValid()
    {
        // --- Given ---
        var loginDTo = _fixture.Create<LoginDto>();
        var expectedToken = _fixture.Create<UserToken>();
        _handlerMock.Setup(h => h.LoginAsync(loginDTo)).ReturnsAsync(expectedToken);
        
        // --- When ---
        var result = await _controller.LoginAsync(loginDTo);
        
        // --- Then ---
        var currentToken = result.Should()
            .BeAssignableTo<Ok<UserToken>>()
            .Subject.Value;
        
        currentToken.Should().BeEquivalentTo(expectedToken);
    }
}