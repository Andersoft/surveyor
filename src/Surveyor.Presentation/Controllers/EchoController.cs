using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Surveyor.Presentation.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EchoController : Controller
  {
    [HttpGet("")]
    public IActionResult Get([FromServices]IConfiguration config)
    {
      var value = config["test"];
      
      return Ok(value);
    }
  }
}
