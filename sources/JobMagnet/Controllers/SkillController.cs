using System.Net.Mime;
using JobMagnet.Models;
using JobMagnet.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class SkillController(ISkillService service) : ControllerBase
{
    [HttpGet("{id:int}", Name = nameof(GetSkillByIdAsync))]
    public async Task<IResult> GetSkillByIdAsync(int id)
    {
        await Task.Delay(1);
        throw new NotImplementedException();
    }

    [HttpPost]
    [ProducesResponseType(typeof(SkillModel), StatusCodes.Status201Created)]
    public async Task<IResult> CreateAsync([FromBody] SkillCreateRequest createRequest)
    {
        var newRecord = await service.Create(createRequest);
        return Results.CreatedAtRoute(nameof(GetSkillByIdAsync), new { id = newRecord.Id }, newRecord);
    }
}