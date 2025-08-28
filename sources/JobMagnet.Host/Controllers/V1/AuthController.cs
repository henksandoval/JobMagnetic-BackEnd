using Asp.Versioning;
using JobMagnet.Application.Exceptions;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;


namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class AuthController(IAuthUserHandler handler)
{
    [HttpPost("login", Name = "loginUser")]
    public async Task<IResult> LoginAsync([FromBody] UserModelCredentials loginRequest)
    {
        var resultToken = await handler.LoginAsync(loginRequest);
        if (resultToken == null)
        {
            return Results.Unauthorized();
        }
        return Results.Ok(resultToken);
    }
    
    [HttpPost("register", Name = "registerUser")]
    public async Task<IResult> RegisterAsync([FromBody] UserModelCredentials  registerRequest)
    {
        try
        {
            var resultToken = await handler.RegisterAsync(registerRequest);
            if (resultToken == null)
            {
                return Results.BadRequest("The user could not be registered.");
            }
            return Results.Ok(resultToken);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}