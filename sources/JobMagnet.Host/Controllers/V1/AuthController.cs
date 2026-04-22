using System.Security.Claims;
using Asp.Versioning;
using JobMagnet.Application.Exceptions;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.DTO.ForgotPassword;
using JobMagnet.Application.UseCases.Auth.DTO.GoogleLogin;
using JobMagnet.Application.UseCases.Auth.DTO.Logout;
using JobMagnet.Application.UseCases.Auth.DTO.UserProfile;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Host.Extensions;
using JobMagnet.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class AuthController(IAuthUserHandler handler) : ControllerBase 
{
    [HttpPost("/auth/register")]
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> RegisterAsync([FromBody] UserModelCredentialsDto  registerRequest, CancellationToken cancellationToken)
    {
        try
        {
            await handler.RegisterAsync(registerRequest, cancellationToken);
            return Results.Ok("Registration successful. Please check your email to confirm your account.");
            
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
    
    [HttpPost("auth/login")]
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> LoginAsync([FromBody] UserModelCredentialsDto loginRequest, CancellationToken cancellationToken)
    {
        try
        {
            var resultToken  = await handler.LoginAsync(loginRequest, cancellationToken);
            if (resultToken.RefreshToken != null)
                HttpContext.Response.AppendRefreshTokenCookie(resultToken.RefreshToken);
            return Results.Ok(resultToken );
        }
        catch (InvalidCredentialsAdapterException)
        {
            return Results.Unauthorized();
        }
    }
    
    [HttpPost("/auth/google-login")]
    public async Task<IResult> LoginGoogleAsync([FromBody] GoogleLoginCommand loginCommand, CancellationToken cancellationToken)
    {
        try
        {
            var command = new GoogleLoginCommand { IdToken = loginCommand.IdToken };
            var token = await handler.LoginGoogleAsync(command, cancellationToken);
            return Results.Ok(token);
        }
        catch (UnauthorizedException)
        {
            return Results.Unauthorized(); 
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("/auth/me")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public Task<IResult> MeAsync()
    {
        if (!TryGetUserId(out var userIdGuid))
            return Task.FromResult(Results.Unauthorized());

        var email = User.FindFirstValue(ClaimTypes.Email) ?? User.FindFirstValue("email") ?? string.Empty;
        var displayName = User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue("name") ?? email;

        var profile = new UserProfileDto
        {
            Id = userIdGuid.ToString(),
            Email = email,
            DisplayName = displayName,
            Roles = [], 
            Permissions = [],
            avatarUrl = User.FindFirstValue("avatar")
        };
        return Task.FromResult(Results.Ok(profile));
    }
    
    [HttpPost("/auth/refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> RefreshTokenAsync(CancellationToken cancellationToken)
    {
        var refreshToken = HttpContext.Request.Cookies["refreshToken"];
        
        if (string.IsNullOrEmpty(refreshToken))
            return Results.BadRequest("No refresh token found.");
        var resultToken =
            await handler.RefreshTokenAsync(new RefreshTokenDto { RefreshToken = refreshToken }, cancellationToken);
        
        if (string.IsNullOrEmpty(resultToken.AccessToken))
            return Results.BadRequest("Invalid or expired refresh token.");
        
        if (resultToken.RefreshToken != null) HttpContext.Response.AppendRefreshTokenCookie(resultToken.RefreshToken);
        
        return Results.Ok(resultToken);
    }
    
    [HttpPost("/auth/logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> LogoutAsync(CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userIdGuid))
            return Results.Unauthorized();
        
        var refreshToken = Request.Cookies["refreshToken"];
        
        if (string.IsNullOrWhiteSpace(refreshToken))
            return Results.BadRequest("Refresh token required.");

        var command = new LogoutCommand(refreshToken, userIdGuid);
        var result = await handler.LogoutAsync(command, cancellationToken);

        if (!result)
            return Results.BadRequest("No active session found.");
        
        HttpContext.Response.DeleteRefreshTokenCookie();

        return Results.NoContent();
    }
    
    private bool TryGetUserId(out Guid userId)
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? User.FindFirstValue("sub")
                      ?? User.FindFirstValue("id")
                      ?? User.FindFirstValue("nameIdentifier");

        if (!string.IsNullOrEmpty(idClaim) && Guid.TryParse(idClaim, out var guid))
        {
            userId = guid;
            return true;
        }
        userId = Guid.Empty;
        return false;
    }
    
    [HttpPost("ForgotPassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IResult> ForgotPasswordAsync([FromBody] ForgotPasswordCommand  command, CancellationToken cancellationToken)
    {
        await handler.ForgotPasswordAsync(command, cancellationToken);
        return Results.Ok("If an account with this email exists, a password reset link has been sent.");
    }
}