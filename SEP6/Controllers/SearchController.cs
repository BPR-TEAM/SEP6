using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FuzzySharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SEP6.Database;
using SEP6.Database.Migrations;
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
        private readonly MoviesContext _moviesContext;

        public SearchController(ILogger<SearchController> logger, TMDbClient client, MoviesContext moviesContext)
        {
            _client = client;
            _moviesContext = moviesContext;
            _logger = logger;
        }

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
            var users = _moviesContext.Users.ToList();
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
            var movies = _moviesContext.Movies.ToList();
            var ratios = new Dictionary<string,int>();
            foreach (var movie in movies)
            {
                var ratio = (int)(Fuzz.Ratio(searchText, movie.Title) * 0.5
                                  + Fuzz.PartialRatio(searchText,movie.Title) * 0.75
                                  + Fuzz.TokenSortRatio(searchText, movie.Title) + 0.75 )/2;
              
                if (ratios.Count < 10)
                {
                    ratios.Add(movie.Title + " " +  movie.Year + ", " + movie.Id,ratio);
                }
                else
                {
                    var lowestRatio = ratios.Aggregate((l, r) => l.Value < r.Value ? l : r);
                    if (ratio > lowestRatio.Value)
                    {
                        if (movie.Title + " " +  movie.Year + ", " + movie.Id == lowestRatio.Key)
                        {
                            throw new Exception("Movies with same title");
                        }
                        ratios.Remove(lowestRatio.Key);
                        ratios.Add(movie.Title + " " +  movie.Year + ", " + movie.Id,ratio);
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