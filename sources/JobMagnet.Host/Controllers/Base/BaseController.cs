using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace JobMagnet.Host.Controllers.Base;

[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class BaseController<TController> : ControllerBase
    where TController : BaseController<TController>
{
    protected readonly ILogger<TController> Logger;

    protected BaseController(ILogger<TController> logger)
    {
        Logger = logger;
    }
}