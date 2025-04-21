using System.Net.Mime;
using Asp.Versioning;
using JobMagnet.Controllers.Base;
using JobMagnet.Extensions.ConfigSections;
using JobMagnet.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JobMagnet.Controllers.V0;

[ApiVersion("0.1")]
public class AdminController(
    ILogger<AdminController> logger,
    JobMagnetDbContext dbContext,
    IOptions<ClientSettings> options) : BaseController<AdminController>(logger)
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

    [HttpPost("seed")]
    public async Task<IActionResult> SeedData()
    {
        var clientSettings = options.Value;
        if (!clientSettings.SeedData)
        {
            const string message = "Seeding data is disabled in the configuration.";

            Logger.LogInformation(message);
            return BadRequest(message);
        }

        await dbContext.Database.EnsureCreatedAsync();

        return Ok();
    }
}