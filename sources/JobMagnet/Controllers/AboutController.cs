using System.Net.Mime;
using JobMagnet.Models;
using JobMagnet.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AboutController(IAboutService service) : ControllerBase
{
    [HttpGet]
    [Route("{id:int}", Name = nameof(GetById))]
    public async Task<IResult> GetById(int id)
    {
        var aboutModel = await service.GetById(id);

        if (aboutModel is null)
            return Results.NotFound($"Record [{id}] not found");

        return Results.Ok(aboutModel);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AboutModel), StatusCodes.Status201Created, MediaTypeNames.Application.Json)]
    public async Task<IResult> Create([FromBody] AboutCreateRequest aboutCreateRequest)
    {
        var newAbout = await service.Create(aboutCreateRequest);
        return Results.CreatedAtRoute(nameof(GetById), new { id = newAbout.Id }, newAbout);
    }
}