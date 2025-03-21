using System.Net.Mime;
using JobMagnet.Models;
using JobMagnet.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class AboutController(IAboutService service) : ControllerBase
{
    [HttpGet("{id:int}", Name = nameof(GetAboutByIdAsync))]
    [ProducesResponseType(typeof(AboutModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IResult> GetAboutByIdAsync(int id)
    {
        var aboutModel = await service.GetByIdAsync(id);

        if (aboutModel is null)
            return Results.NotFound($"Record [{id}] not found");

        return Results.Ok(aboutModel);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AboutModel),StatusCodes.Status201Created)]
    public async Task<IResult> Create([FromBody] AboutCreateRequest createRequest)
    {
        var newRecord = await service.Create(createRequest);
        return Results.CreatedAtRoute(nameof(GetAboutByIdAsync), new { id = newRecord.Id }, newRecord);
    }
}