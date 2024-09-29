using JobMagnet.Entities;
using JobMagnet.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class AboutController : ControllerBase
    {
        public AboutController() { }

        [HttpGet("GetById", Name = "GetByid")]
        public IActionResult GetByID(AboutEntity about)
        {
            return Ok(about);
        }

        [HttpGet("GetTrue", Name = "GetTrue")]
        public ActionResult GetOk()
        {
            var aboutEntity = new AboutEntity();
            return Ok(aboutEntity);
        }

        [HttpPost]
        public ActionResult CreateAbout(AboutCreateRequest aboutCreateRequest)
        {
            return Ok(aboutCreateRequest);
        }
    }
}
