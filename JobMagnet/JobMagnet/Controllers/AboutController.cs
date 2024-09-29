using JobMagnet.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class AboutController : ControllerBase
    {
        public AboutController() { }

        public IActionResult GetByID(AboutEntity about)
        {
            return Ok(about);
        }

        [HttpGet]
        public ActionResult GetOk()
        {
            var aboutEntity = new AboutEntity();
            return Ok(aboutEntity);
        }
    }
}
