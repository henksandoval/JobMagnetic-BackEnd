using System.Net;
using AutoFixture;
using AwesomeAssertions;
using System.Security.Claims;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Host.Controllers.V1;
using Microsoft.AspNetCore.Http;
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
    
    // [Fact]
    // public async Task ReturnUnauthorized_WhenCredentialsAreInvalid()
    // {
    //     // --- Given ---
    //     var loginDto = _fixture.Create<UserModelCredentials>();
    //     _handlerMock.Setup(h => h.LoginAsync(loginDto)).ReturnsAsync((UserToken)null);
    //
    //     // --- When ---
    //     var result = await _controller.LoginAsync(loginDto);
    //
    //     // --- Then ---
    //     result.Should().BeOfType<UnauthorizedHttpResult>();
    // }
    //
    // [Fact]
    // public async Task ReturnToken_WhenCredentialsAreValid()
    // {
    //     // --- Given ---
    //     var loginDTo = _fixture.Create<UserModelCredentials>();
    //     var expectedToken = _fixture.Create<UserToken>();
    //     _handlerMock.Setup(h => h.LoginAsync(loginDTo)).ReturnsAsync(expectedToken);
    //     
    //     // --- When ---
    //     var result = await _controller.LoginAsync(loginDTo);
    //     
    //     // --- Then ---
    //     var currentToken = result.Should()
    //         .BeAssignableTo<Ok<UserToken>>()
    //         .Subject.Value;
    //     
    //     currentToken.Should().BeEquivalentTo(expectedToken);
    // }

    [Fact]
    public async Task LogoutAsync_WithValidTokenAndAuthenticatedUser_ReturnsNoContent()
    {
        // --- Given ---
        var testUserId = Guid.NewGuid();
        var tokenDto = new LogoutTokenDto { Token = "sbLXdvL72K8Hcs6IKUe288qlCyNbEBDnVwkC3kAefXpCuIq2iQOW9GA7VISMp7yVWbAmDGxY9lTzsqZNXDXCTg==" };
        var cancellationToken = CancellationToken.None;
        
        var userClaims = new[] 
        {
            new Claim(ClaimTypes.NameIdentifier, testUserId.ToString()) 
        };
        var identity = new ClaimsIdentity(userClaims, "Sebas123456");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        
        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal 
        };
        
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
        
        _handlerMock.Setup(h => h.LogoutAsync(It.Is<LogoutCommand>(cmd => cmd.RefreshToken == tokenDto.Token && cmd.UserId == testUserId), 
                cancellationToken))
            .ReturnsAsync(true);
        
        // --- When ---
        
        var result = await _controller.LogoutAsync(tokenDto, cancellationToken);
        
        result.Should().NotBeNull();
        var statusCodeResult = result.Should().BeAssignableTo<IStatusCodeHttpResult>().Subject;
        statusCodeResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        
        // --- Then ---
        _handlerMock.Verify(h => h.LogoutAsync( It.Is<LogoutCommand>(cmd => cmd.RefreshToken == tokenDto.Token && cmd.UserId == testUserId), cancellationToken),
            Times.Once
        );
    }
    
    [Fact]
    public async Task LogoutAsync_WhenUserIsNotAuthenticated_ReturnsUnauthorized()
    {
        // --- Given ---
        var tokenDto = new LogoutTokenDto { Token = "un-token" };
        
        var unauthenticatedIdentity = new ClaimsIdentity(); 
        
        var unauthenticatedPrincipal = new ClaimsPrincipal(unauthenticatedIdentity);
        
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = unauthenticatedPrincipal }
        };
        
        // --- When ---
        var result = await _controller.LogoutAsync(tokenDto, CancellationToken.None);

        // --- Then ---
        var statusCodeResult = result.Should().BeAssignableTo<IStatusCodeHttpResult>().Subject;
        statusCodeResult.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        
        _handlerMock.Verify(
            h => h.LogoutAsync(It.IsAny<LogoutCommand>(), It.IsAny<CancellationToken>()), 
            Times.Never);
    }
}