using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEP6.Database;
using SEP6.Utilities;

namespace SEP6.Controllers
{
    /// <summary>
    /// User actions
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly MoviesContext _dbContext;

        public UserController(ILogger<AuthController> logger, MoviesContext db)
        {
            _logger = logger;
            _dbContext = db;
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userToFollow"></param>
        /// <param name="userFollowing"></param>
        /// <returns></returns>
        [HttpPost]
        public ObjectResult Follow([FromHeader] string token, string userToFollow)
        {
            ControllerUtilities.TokenVerification(token, _dbContext, out var userFollow, out var verified);
            
            if(!verified)
                return Unauthorized("Token expired");

            var userTo = _dbContext.Users.Include(a=>a.Followers).First(a => a.Username == userToFollow);
            if (userTo == null)
                return NotFound($"User {userToFollow} not found");
            
            
            userTo.Followers.Add(userFollow);
            _dbContext.SaveChanges();
            
            return Ok($"{userFollow.Username} now follows {userToFollow}!");
        }
        
    }
}