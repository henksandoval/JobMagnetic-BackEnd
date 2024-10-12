using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class SkillsController : ControllerBase
    {
        public SkillsController()
        {          
        }

        [HttpGet]
        public IActionResult GetOk() 
        {
            return Ok();
        }
    }
}
