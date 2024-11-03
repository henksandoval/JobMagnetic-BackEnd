using JobMagnet.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class SummaryController : ControllerBase
    {
        private readonly ISummaryService _service;
        public SummaryController(ISummaryService Service)
        {
            _service = Service;
        }
        
        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> Get(int id)
        {
           var summaryModel = await _service.GetById(id);
           return Ok(summaryModel);
        }
    }
}
