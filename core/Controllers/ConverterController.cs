namespace core.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ConverterController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("API is alive");
    }
}


