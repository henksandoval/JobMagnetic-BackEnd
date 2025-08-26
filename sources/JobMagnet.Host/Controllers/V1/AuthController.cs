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
    [ProducesResponseType(typeof(UserToken), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
    public async Task<IResult> CreateAdminUser(CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.CreateAdminUserAsync(cancellationToken);
            return Results.Created(string.Empty, result);
        }
        catch (AdminUserAlreadyExistsException ex)
        {
            return Results.Conflict(new { message = ex.Message });
        }
    }
}