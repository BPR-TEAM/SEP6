using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEP6.DB;
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
        private readonly ILogger<UserController> _logger;
        private readonly MoviesDbContext _dbContext;

        public UserController(ILogger<UserController> logger, MoviesDbContext db)
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

            if (!verified)
            {
                _logger.Log(LogLevel.Information, $"Token expired {token}");
                return Unauthorized("Token expired");
            }

            var userTo = _dbContext.Users.Include(a=>a.Followers).FirstOrDefault(a => a.Username == userToFollow);
            if (userTo == null)
            {
                _logger.Log(LogLevel.Information, $"User {userToFollow} not found");
                return NotFound($"User {userToFollow} not found");
            }

            if (userTo.Followers.Contains(userFollow))
            {
                userTo.Followers.Remove(userFollow);
                _dbContext.SaveChanges();
                _logger.Log(LogLevel.Information, $"{userFollow.Username} doesn't follow {userToFollow} anymore!");
                return  Ok($"{userFollow.Username} doesn't follow {userToFollow} anymore!");
            }
            
            userTo.Followers.Add(userFollow);
            _dbContext.SaveChanges();
            
            _logger.Log(LogLevel.Information, $"{userFollow.Username} now follows {userToFollow}!");
            return Ok($"{userFollow.Username} now follows {userToFollow}!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ObjectResult GetUser([FromHeader] string fromToken, string toUser)
        {
            ControllerUtilities.TokenVerification(fromToken, _dbContext, out var fromUser, out var verified);

            if(!verified)
            return Unauthorized("Malformed token");

            if(String.IsNullOrEmpty(toUser))
            {
                return Ok(_dbContext.Users.Include(a=>a.TopLists).ThenInclude(toplist=>toplist.Movies).AsParallel().FirstOrDefault(a => a.Username == fromUser.Username));
            }
            else
            {
                return Ok(_dbContext.Users.Include(a=>a.TopLists).ThenInclude(toplist=>toplist.Movies).Include(followers => followers.Followers).AsParallel().FirstOrDefault(a => a.Username == toUser));
            }
        }
    }
}