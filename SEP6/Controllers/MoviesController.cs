using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEP6.DB;
using TMDbLib.Client;

namespace SEP6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly TMDbClient _client;
        private readonly MoviesDbContext _moviesContext;

        public MoviesController(ILogger<AuthController> logger, TMDbClient client, MoviesDbContext moviesContext)
        {
            _client = client;
            _moviesContext = moviesContext;
            _logger = logger;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(Dictionary<string, Movie>),200)]
        public async Task<ObjectResult> Get(string id)
        {
            var longId = Int64.Parse(id);
            var dbMovie = _moviesContext.Movies.First(a => a.Id == longId);

            if (dbMovie == null)
                return NotFound("ID does not correspond to any movie");
            
            var movie = await _client.GetMovieAsync("tt"+id);

            var dictionary = new Dictionary<string,Object>();
            dictionary.Add("tmdbMovie",movie);
            dictionary.Add("movie",dbMovie);
            
            return Ok(dictionary);
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(List<Director>),200)]
        [Route("directors")]
        public async Task<ObjectResult> GetDirectors(string id)
        {
            var longId = Int64.Parse(id);
            var dbMovie = _moviesContext.Directors.Include(a=>a.Person)
                .Where(a => a.MovieId == longId).ToList();

            if (dbMovie.Count < 1)
                return NotFound("ID does not correspond to any movie");
            
            return Ok(dbMovie);
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(Star),200)]
        [Route("stars")]
        public async Task<ObjectResult> GetStars(string id)
        {
            var longId = Int64.Parse(id);
            var dbMovie = _moviesContext.Stars.Include(a=> a.Person)
                .Where(a => a.MovieId == longId).ToList();

            if (dbMovie.Count < 1)
                return NotFound("ID does not correspond to any movie");
            
            return Ok(dbMovie);
        }
        
        
        [HttpGet]
        [ProducesResponseType(typeof(Rating),200)]
        [Route("ratings")]
        public async Task<ObjectResult> GetRating(string id)
        {
            var longId = Int64.Parse(id);
            var dbMovie = _moviesContext.Ratings
                .First(a => a.MovieId == longId);

            if (dbMovie == null)
                return NotFound("ID does not correspond to any movie");
            
            return Ok(dbMovie);
        }
    }
}