using System.Net.Mime;
using Asp.Versioning;
using JobMagnet.Controllers.Base;
using JobMagnet.Extensions.ConfigSections;
using JobMagnet.Infrastructure.Context;
using JobMagnet.Infrastructure.Seeders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JobMagnet.Controllers.V0;

[ApiVersion("0.1")]
public class AdminController(
    ILogger<AdminController> logger,
    JobMagnetDbContext dbContext,
    ISeeder Seeder) : BaseController<AdminController>(logger)
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
    public async Task<IActionResult> DestroyDatabase()
    {
        await dbContext.Database.EnsureDeletedAsync();
        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDatabase()
    {
        await dbContext.Database.EnsureCreatedAsync();
        return Ok();
    }

    [HttpPost("seedMasterTables")]
    public async Task<IResult> SeedMasterTables()
    {
        if (dbContext.ContactTypes.Any())
        {
            throw new InvalidOperationException("Contact types are filled");
        }

        await Seeder.RegisterMasterTablesAsync();
        return Results.Accepted();
    }

    [HttpPost("seedProfile")]
    public async Task<IResult> SeedProfile()
    {
        if (!dbContext.ContactTypes.Any())
        {
            throw new InvalidOperationException("Contact types are not yet implemented");
        }

        await Seeder.RegisterProfileAsync();
        return Results.Accepted();
    }
}