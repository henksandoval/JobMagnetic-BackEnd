using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers
{
    [ApiController]

    [Route("api/controller")]
    public class SummaryController : ControllerBase
    {
        public SummaryController() { }

        public IActionResult GetOk()
        {
            return Ok();
        }
    }
}
