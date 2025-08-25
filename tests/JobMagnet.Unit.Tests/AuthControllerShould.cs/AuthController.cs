using AutoFixture;
using AwesomeAssertions;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Host.Controllers.V1;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
    public async Task ReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // --- Given ---
        var loginDto = _fixture.Create<LoginDto>();
        _handlerMock.Setup(h => h.LoginAsync(loginDto)).ReturnsAsync((UserToken)null);

        // --- When ---
        var result = await _controller.LoginAsync(loginDto);

        // --- Then ---
        result.Should().BeOfType<UnauthorizedHttpResult>();
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
    
    [Fact]
    public async Task CreateAdminUser_ReturnsOk_WhenSuccess()
    {
        // --- Given ---
        var expectedToken =  _fixture.Create<UserToken>();
        _handlerMock.Setup(h => h.CreateAdminUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedToken);
        
        // --- When ---
        var controller =await  _controller.CreateAdminUser(CancellationToken.None);

        // --- Then ---
        var okToken = controller.Should().BeAssignableTo<Ok<UserToken>>()
            .Subject.Value;
        
        okToken.Should().BeEquivalentTo(expectedToken);
    }
}