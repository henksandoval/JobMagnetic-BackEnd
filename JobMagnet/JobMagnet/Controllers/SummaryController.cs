using JobMagnet.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers
{
    [ApiController]

    [Route("api/controller")]
    public class SummaryController : ControllerBase
    {
        public SummaryController() { }


        [HttpGet]
        public IActionResult Get()
        {
            var summaryEntity = new SummaryEntity { About = "Me llamo Alexandra" };
            return Ok(summaryEntity);
        }
    }
}
