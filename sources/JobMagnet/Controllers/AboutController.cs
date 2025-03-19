using System.Net.Mime;
using JobMagnet.Models;
using JobMagnet.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AboutController : ControllerBase
{
    private readonly IAboutService _service;

    public AboutController(IAboutService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{id}", Name = nameof(GetById))]
    public async Task<IActionResult> GetById(int id)
    {
        var aboutModel = await _service.GetById(id);

        if (aboutModel is null) return NotFound($"Record [{id}] not found");

        return Ok(aboutModel);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AboutModel), StatusCodes.Status201Created, MediaTypeNames.Application.Json)]
    public async Task<IResult> Create([FromBody] AboutCreateRequest aboutCreateRequest)
    {
        var newAbout = await _service.Create(aboutCreateRequest);
        return Results.CreatedAtRoute(nameof(GetById), new { id = newAbout.Id }, newAbout);
    }
}