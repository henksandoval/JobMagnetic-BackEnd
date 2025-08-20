using Asp.Versioning;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using Microsoft.AspNetCore.Mvc;


namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class AuthController(IAuthUserHandler handler)
{
    [HttpPost("login", Name = "loginUser")]
    public async Task<IResult> LoginAsync([FromBody] LoginDto loginRequest)
    {
        var token = await handler.LoginAsync(loginRequest);
        return Results.Ok(token);
    }
}