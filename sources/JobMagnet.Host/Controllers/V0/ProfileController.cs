using System.Net.Mime;
using Asp.Versioning;
using JobMagnet.Domain.Domain.Services;
using JobMagnet.Host.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.V0;

[ApiVersion("0.1")]
public class ProfileController(ICvParser cvParser, ILogger<ProfileController> logger)
    : BaseController<ProfileController>(logger)
{
    [HttpPost]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> CreateAsync(IFormFile cvFile)
    {
        var result = await cvParser.ParseAsync(cvFile.OpenReadStream()).ConfigureAwait(false);
        return Results.Ok(result);
    }
}