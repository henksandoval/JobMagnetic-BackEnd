using Asp.Versioning;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using Microsoft.AspNetCore.Mvc;


namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class AuthController(IAuthUserHandler handler)
{
    [HttpPost("login", Name = "loginUser")]
    public async Task<IResult> LoginAsync([FromBody] UserModelCredentials loginRequest)
    {
        var resultToken = await handler.LoginAsync(loginRequest);
        return resultToken != null ? Results.Ok(resultToken) :  Results.Unauthorized();
    }
    
    [HttpPost("register")]
    public async Task<IResult> RegisterAsync([FromBody] UserModelCredentials  registerRequest)
    {
        try
        {
            await handler.RegisterAsync(registerRequest);
            return Results.Ok("Registration successful. Please check your email to confirm your account.");
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}