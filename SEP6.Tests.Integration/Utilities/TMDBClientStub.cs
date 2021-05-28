using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;

namespace SEP6.Tests.Integration.Utilities
{
    public class TMDBClientStub : TMDbClient
    {
        public TMDBClientStub(string apiKey, bool useSsl = true, string baseUrl = "api.themoviedb.org", JsonSerializer serializer = null, IWebProxy proxy = null) : base(apiKey, useSsl, baseUrl, serializer, proxy)
        {
        }

        public Task<Movie> GetMovieAsync(string imdb)
        {
            return  Task.FromResult(new Movie(){Title = "yo"});
        }
            
    }
}