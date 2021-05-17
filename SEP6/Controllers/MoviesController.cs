using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TMDbLib.Client;

namespace SEP6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly TMDbClient _client;

        public MoviesController(ILogger<AuthController> logger, TMDbClient client)
        {
            _client = client;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<string> Get()
        {
            var result = await _client.GetMovieAsync(47964);
            return result.Title;
        }
    }
}