using JobMagnet.Models;
using JobMagnet.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers
{
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
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var aboutModel = await _service.GetById(id);

            if (aboutModel is null) 
            {
                return NotFound($"Record [{id}] not found");
            }

            return Ok(aboutModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AboutCreateRequest aboutCreateRequest)
        {
            var aboutModel = await _service.Create(aboutCreateRequest);
            return CreatedAtRoute("GetById", aboutModel.Id);
        }
    }
}
