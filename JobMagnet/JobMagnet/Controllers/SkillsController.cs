using JobMagnet.Models;
using JobMagnet.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService service;

        public SkillsController(ISkillService service)
        {
            this.service = service;
        }

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult GetOk() 
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SkillCreateRequest skillCreateRequest) 
        {
            var skillModel = await service.Create(skillCreateRequest);
            return CreatedAtRoute("GetById", skillModel.Id);

        }
    }
}
