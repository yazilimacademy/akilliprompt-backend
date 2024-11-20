using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AkilliPrompt.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecretsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SecretsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            var secret = _configuration["ConnectionStrings:DefaultConnection"];

            return Ok(secret ?? "No secret found");
        }
        [HttpGet("redis")]
        public IActionResult GetRedisSecret()
        {
            var secret = _configuration["ConnectionStrings:RedisConnection"];

            return Ok(secret ?? "No secret found");
        }
    }
}
