using System.Security.Claims;
using Asp.Versioning;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class AuthController(IAuthUserHandler handler) : ControllerBase 
{
    
    [HttpPost("register")]
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
    
    [HttpPost("login")]
    // [Authorize(AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> LoginAsync([FromBody] UserModelCredentialsDto loginRequest, CancellationToken cancellationToken)
    {
        try
        {
            var responseToken = await handler.LoginAsync(loginRequest, cancellationToken);
            return responseToken != null ? Results.Ok(responseToken) : Results.Unauthorized();
        }
        catch (InvalidCredentialsAdapterException)
        {
            return Results.Unauthorized();
        }
    }
    
    [HttpPost("refreshToken")]
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> RefreshTokenAsync([FromBody] RefreshTokenDto refreshTokenDto,  CancellationToken cancellationToken)
    {
        var resultToken = await handler.RefreshTokenAsync(refreshTokenDto, cancellationToken);
        return resultToken != null ? Results.Ok(resultToken) :Results.BadRequest("Invalid client request or refresh token.");
    }
    
    [HttpPost("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> LogoutAsync([FromBody] LogoutTokenDto  tokenDto, CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userIdGuid))
            return Results.Unauthorized();
        
        if (string.IsNullOrWhiteSpace(tokenDto?.Token))
            return Results.BadRequest("Refresh token required.");
 
        var command = new LogoutCommand(tokenDto.Token, userIdGuid);
        var result = await handler.LogoutAsync(command,  cancellationToken);
            
        return result ? Results.NoContent() : Results.BadRequest("No active session found.");
    }
    
    private bool TryGetUserId(out Guid userId)
    {
        var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? User.FindFirstValue("sub")
                      ?? User.FindFirstValue("id")
                      ?? User.FindFirstValue("nameid");

        if (!string.IsNullOrEmpty(idClaim) && Guid.TryParse(idClaim, out var guid))
        {
            userId = guid;
            return true;
        }
        userId = default;
        return false;
    }
}