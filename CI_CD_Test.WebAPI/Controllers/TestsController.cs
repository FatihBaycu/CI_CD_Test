using Microsoft.AspNetCore.Mvc;

namespace CI_CD_Test.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Message = "CI/CD is working 🚀",
                ServerTime = DateTime.Now,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            });
        }
    }
}
