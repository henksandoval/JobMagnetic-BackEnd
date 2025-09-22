using Asp.Versioning;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Domain.Aggregates.Auth.Entities;
using Microsoft.AspNetCore.Mvc;


namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class AuthController(IAuthUserHandler handler)
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
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IResult> LoginAsync([FromBody] UserModelCredentialsDto loginRequest, CancellationToken cancellationToken)
    {
        var responseToken = await handler.LoginAsync(loginRequest, cancellationToken);
        return responseToken != null ? Results.Ok(responseToken) :  Results.Unauthorized();
    }
    
    [HttpPost("refreshToken")]
    [ProducesResponseType(typeof(UserTokenDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IResult> RefreshTokenAsync([FromBody] RefreshTokenDto refreshTokenDto,  CancellationToken cancellationToken)
    {
        var resultToken = await handler.RefreshTokenAsync(refreshTokenDto, cancellationToken);
        return resultToken != null ? Results.Ok(resultToken) :Results.BadRequest("Invalid client request or refresh token.");
    }
}