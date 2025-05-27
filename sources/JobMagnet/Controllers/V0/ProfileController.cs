using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Asp.Versioning;
using GeminiDotNET;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ClientModels;
using JobMagnet.Controllers.Base;
using JobMagnet.Domain.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers.V0;

[ApiVersion("0.1")]
public class ProfileController : BaseController<ProfileController>
{
    private readonly ICvParser _cvParser;
    private readonly IConfiguration _configuration;
    private readonly string _geminiApiKey;
    private readonly string _flattenedJsonSchema;

    public ProfileController(ICvParser cvParser, ILogger<ProfileController> logger) : base(logger)
    {
        _cvParser = cvParser;
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Multipart.FormData)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> CreateAsync(IFormFile cvFile)
    {
        var result = await _cvParser.ParseAsync(cvFile).ConfigureAwait(false);
        return Results.Ok(result);
    }
}