using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SEP6.Tests.Integration.Utilities;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using Xunit;
using Movie = SEP6.DB.Movie;

namespace SEP6.Tests.Integration.Controllers
{
    public class MoviesControllerTests: IClassFixture<CustomApplicationFactory<SEP6.Startup>>
    {
        private readonly CustomApplicationFactory<Startup> _factory;

        public MoviesControllerTests(CustomApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_OK()
        {
            var client = _factory.CreateClient();
            var id = "2395427";
            var url = $"/Movies?id={id}";

            var response = await client.GetAsync(url);
            var message = ResponseHandler<Dictionary<string,Object>>.GetObject(response);
            var movieFromDb = (message["movie"] as JObject)?.ToObject<Movie>();
            var tmdbMovie = (message["tmdbMovie"] as JObject)?.ToObject<TMDbLib.Objects.Movies.Movie>();

            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            Assert.Equal(movieFromDb.Title,tmdbMovie.Title);
        }
        
        [Fact]
        public async Task Get_NotFound()
        {
            var client = _factory.CreateClient();
            var id = "1";
            
            var url = $"/Movies?id={id}";

            var response = await client.GetAsync(url);
            var message = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
            Assert.Equal("ID does not correspond to any movie",message);
        }

        [Fact]
        public async Task GetTopRatedMovies()
        {
            var client = _factory.CreateClient();
            var id = "1";
            var url = "/Movies/topRated";
            
            var response = await client.GetAsync(url);
            var message = ResponseHandler<List<SearchMovie>>.GetObject(response);
            
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            Assert.NotEmpty(message);
        }
        
        [Fact]
        public async Task GetMostPopular()
        {
            var client = _factory.CreateClient();
            var id = "1";
            var url = "/Movies/mostPopular";
            
            var response = await client.GetAsync(url);
            var message = ResponseHandler<List<SearchMovie>>.GetObject(response);
            
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            Assert.NotEmpty(message);
        }
        
        [Fact]
        public async Task GetDirectors_NotFound()
        {
            var client = _factory.CreateClient();
            var id = "45";
            var url = $"/Movies/directors?id={id}";
            
            var response = await client.GetAsync(url);
            var message = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
            Assert.Equal("ID does not correspond to any movie",message);
        }
        
        
        [Fact]
        public async Task GetDirectors_OK()
        {
            var client = _factory.CreateClient();
            var id = "2395427";
            var url = $"/Movies/directors?id={id}";
            
            var response = await client.GetAsync(url);
            var message = ResponseHandler<List<TMDbLib.Objects.People.Person>>.GetObject(response);
            
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            Assert.Equal("nm"+"751648".PadLeft(7, '0'),message[0].ImdbId);
        }
    }
}