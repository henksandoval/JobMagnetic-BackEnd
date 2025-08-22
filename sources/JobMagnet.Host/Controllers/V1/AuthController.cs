using Asp.Versioning;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Domain.Aggregates;
using Microsoft.AspNetCore.Mvc;


namespace JobMagnet.Host.Controllers.V1;

[ApiVersion("1")]
public class AuthController(IAuthUserHandler handler)
{
    [HttpPost("login", Name = "loginUser")]
    public async Task<IResult> LoginAsync([FromBody] LoginDto loginRequest)
    {
        var resultToken = await handler.LoginAsync(loginRequest);
        if (resultToken == null)
        {
            return Results.Unauthorized();
        }
        return Results.Ok(resultToken);
    }
    
    [HttpPost("user-administrator", Name ="createAdminUser")]
    public async Task<IResult> CreateAdminUser(CancellationToken cancellationToken)
    {
        var result = await handler.CreateAdminUserAsync(cancellationToken);
        return Results.Ok(result);
    }
}