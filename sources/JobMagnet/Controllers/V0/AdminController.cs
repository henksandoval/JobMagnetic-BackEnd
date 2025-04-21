using System.Net.Mime;
using Asp.Versioning;
using JobMagnet.Controllers.Base;
using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Seeders;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers.V0;

[ApiVersion("0.1")]
public class AdminController(
    ILogger<AdminController> logger,
    JobMagnetDbContext dbContext,
    ISeeder seeder) : BaseController<AdminController>(logger)
{
    private const string PongMessage = "Pong";

    [HttpGet("ping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Text.Plain)]
    public IResult Ping()
    {
        Logger.LogInformation(PongMessage);
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

    [HttpPost("seedMasterTables")]
    public async Task<IResult> SeedMasterTables(CancellationToken cancellationToken)
    {
        if (dbContext.ContactTypes.Any())
        {
            throw new InvalidOperationException("Contact types are filled");
        }

        await seeder.RegisterMasterTablesAsync(cancellationToken);
        return Results.Accepted();
    }

    [HttpPost("seedProfile")]
    public async Task<IResult> SeedProfile(CancellationToken cancellationToken)
    {
        if (!dbContext.ContactTypes.Any())
        {
            throw new InvalidOperationException("Contact types are not yet implemented");
        }

        await seeder.RegisterProfileAsync(cancellationToken);
        return Results.Accepted();
    }
}