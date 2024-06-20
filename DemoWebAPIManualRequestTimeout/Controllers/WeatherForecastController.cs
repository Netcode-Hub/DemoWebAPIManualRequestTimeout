using DemoWebAPIManualRequestTimeout.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAPIManualRequestTimeout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(IService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await service.GetString(HttpContext.RequestAborted);
            return Ok(response);
        }

       
    }
}
