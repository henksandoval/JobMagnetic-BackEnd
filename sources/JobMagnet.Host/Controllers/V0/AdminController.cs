using System.Net.Mime;
using Asp.Versioning;
using JobMagnet.Application.Exceptions;
using JobMagnet.Application.UseCases.Auth.DTO;
using JobMagnet.Application.UseCases.Auth.Interface;
using JobMagnet.Host.Controllers.Base;
using JobMagnet.Infrastructure.Persistence.Context;
using JobMagnet.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Mvc;
using Light.GuardClauses;

namespace JobMagnet.Host.Controllers.V0;

[ApiVersion("0.1")]
public class AdminController( ILogger<AdminController> logger, JobMagnetDbContext dbContext,
    ISeeder seeder, IAuthUserHandler _handler) : BaseController<AdminController>()
{
    private readonly ILogger<AdminController> _logger = logger.MustNotBeNull();
    private const string PongMessage = "Pong";

    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Text.Plain)]
    public IResult Ping()
    {
        _logger.LogInformation(PongMessage);
        return Results.Text(PongMessage);
    }

    [HttpDelete]
    public async Task<IActionResult> DestroyDatabase(CancellationToken cancellationToken)
    {
        await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDatabase(CancellationToken cancellationToken)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        return Ok();
    }

    [HttpPost("seedProfile")]
    public async Task<IResult> SeedProfile(CancellationToken cancellationToken)
    {
        if (!dbContext.ContactTypes.Any()) throw new InvalidOperationException("Contact types are not yet implemented");

        var profileId = await seeder.RegisterProfileAsync(cancellationToken);

        return profileId.HasValue ? Results.Ok(profileId.Value.Value) : Results.Problem();
    }
    
    [HttpPost("user-administrator", Name ="createAdminUser")]
    [ProducesResponseType(typeof(UserToken), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
    public async Task<IResult> CreateAdminUser(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _handler.CreateAdminUserAsync(cancellationToken);
            return Results.Created(string.Empty, result);
        }
        catch (AdminUserAlreadyExistsException ex)
        {
            return Results.Conflict(new { message = ex.Message });
        }
    }
}