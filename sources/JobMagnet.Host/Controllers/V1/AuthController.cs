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
    public async Task<IResult> RegisterAsync([FromBody] UserModelCredentials  registerRequest, CancellationToken cancellationToken)
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
    
    [HttpPost("login", Name = "loginUser")]
    public async Task<IResult> LoginAsync([FromBody] UserModelCredentials loginRequest, CancellationToken cancellationToken)
    {
        var responseToken = await handler.LoginAsync(loginRequest, cancellationToken);
        return responseToken != null ? Results.Ok(responseToken) :  Results.Unauthorized();
    }
    
    [HttpPost("refreshToken")]
    public async Task<IResult> RefreshTokenAsync([FromBody] RefreshToken request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Token))
            return  Results.BadRequest("Refresh token is required.");
        
        var userToken = await handler.RefreshTokenAsync(request);
        
        return userToken == null ? Results.Unauthorized() : Results.Ok(userToken);
    }
}