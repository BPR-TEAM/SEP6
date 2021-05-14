using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SEP6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Login()
        {
            return "Login Done!";
        }
        
    }
}