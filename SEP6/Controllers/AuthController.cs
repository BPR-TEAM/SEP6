using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEP6.Database;

namespace SEP6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly MoviesContext _dbContext;

        public AuthController(ILogger<AuthController> logger, MoviesContext db)
        {
            _logger = logger;
            _dbContext = db;
        }

        [HttpPost]
        [Route("Register")]
        public ObjectResult SignUp([FromBody] User user)
        {
            var password = user.Password;
            
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            user.Password = HashPassword(salt, password);
            user.PasswordSalt =salt;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return Ok("Registration complete!");
        }
        
        [HttpPost]
        [Route("Login")]
        public ObjectResult Login([FromBody] User user)
        {
            if (user.Password == null || user.Email == null)
            {
                return Unauthorized("Fill the fields");
            }
            
            var dbUser = _dbContext.Users
                .First(a => a.Email == user.Email);

            if (dbUser == null)
            {
                return Unauthorized("User does not exist");
            }
            
            var salt = dbUser.PasswordSalt;
            var givenPassword = HashPassword(salt, user.Password);

            if (dbUser.Password == givenPassword)
            {
                return Ok(user.Id);
            }
            return Unauthorized("Wrong credentials");
        }

        [NonAction]
        public string HashPassword(byte[] salt, string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

    }
}