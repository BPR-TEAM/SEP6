using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FuzzySharp;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEP6.DB;
using TMDbLib.Client;
using Process = FuzzySharp.Process;

namespace SEP6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly TMDbClient _client;
        private readonly MoviesDbContext _moviesContext;

        public SearchController(ILogger<SearchController> logger, TMDbClient client, MoviesDbContext moviesContext)
        {
            _client = client;
            _moviesContext = moviesContext;
            _logger = logger;
        }

        /// <summary>
        /// Search for movies/ users
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="searchType">1-Users 2- Movies</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<string>> Search(string searchText, SearchType searchType)
        {
            return searchType switch
            {
                SearchType.User => SearchUser(searchText),
                SearchType.Movies =>  SearchMovie(searchText),
                _ => new List<string>(){"Invalid input"}
            };
        }

        [NonAction]
        public List<string> SearchUser(string searchText)
        {
            var users = _moviesContext.Users.AsParallel().ToList();
            var ratios = new Dictionary<string,int>();
            foreach (var user in users)
            {
                var ratio = Fuzz.Ratio(searchText, user.Username);
                if (ratios.Count < 5)
                {
                    ratios.Add(user.Username,ratio);
                }
                else
                {
                    var lowestRatio = ratios.Aggregate((l, r) => l.Value < r.Value ? l : r);
                    if (ratio > lowestRatio.Value)
                    {
                        ratios.Remove(lowestRatio.Key);
                        ratios.Add(user.Username,ratio);
                    }
                }
            }
            
            return ratios.Keys.ToList();
        }
        
        [NonAction]
        public List<string> SearchMovie(string searchText)
        {
            List<int> movies = _moviesContext.Movies.Select(movie => movie.Id).AsParallel().ToList();
            var ratios = new Dictionary<string,int>();
            string movieTitle;
            foreach (var movieId in movies)
            {
                movieTitle = _moviesContext.Movies.FirstOrDefault(a=> a.Id == movieId).Title;
                var ratio = (int)(Fuzz.Ratio(searchText, movieTitle) * 0.5
                                  + Fuzz.PartialRatio(searchText,movieTitle) * 0.75
                                  + Fuzz.TokenSortRatio(searchText, movieTitle) + 0.75 )/2;
              
                if (ratios.Count < 10)
                {
                    ratios.Add(movieTitle + " " + _moviesContext.Movies.FirstOrDefault(a => a.Id == movieId).Year + ", " + movieId,ratio);
                }
                else
                {
                    var lowestRatio = ratios.Aggregate((l, r) => l.Value < r.Value ? l : r);
                    if (ratio > lowestRatio.Value)
                    {
                        if (movieTitle + " " +  _moviesContext.Movies.FirstOrDefault(a => a.Id == movieId).Year + ", " + movieId == lowestRatio.Key)
                        {
                            throw new Exception("Movies with same title");
                        }
                        ratios.Remove(lowestRatio.Key);
                        ratios.Add(movieTitle + " " +  _moviesContext.Movies.FirstOrDefault(a => a.Id == movieId).Year + ", " + movieId,ratio);
                    }
                }
            }
            
            return ratios.Keys.ToList();
        }

        public enum SearchType
        {
         User = 1,
         Movies = 2
        }
            
    }
}