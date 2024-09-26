using JobMagnet.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class AboutController : ControllerBase
    {
        public AboutController() { }

        [HttpGet]
        public ActionResult GetOk()
        {
            var aboutEntity = new AboutEntity();
            return Ok(aboutEntity);
        }
    }
}
