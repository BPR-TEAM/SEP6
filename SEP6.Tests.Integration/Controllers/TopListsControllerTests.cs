using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SEP6.DB;
using SEP6.Tests.Integration.Utilities;
using Xunit;

namespace SEP6.Tests.Integration.Controllers
{
    public class TopListsControllerTests: IClassFixture<CustomApplicationFactory<SEP6.Startup>>
    {
        private readonly CustomApplicationFactory<Startup> _factory;

        public TopListsControllerTests(CustomApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Add_OK()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
            
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token","1=ssss");
            var url = "/TopLists";

            var topLists = new TopLists() {Name = "Coolest 50", UserId = 1,Movies = new List<Movie>()};
            topLists.Movies.Add(new Movie() {Title = "Avengers: Age of Ultron", Id = 2395427, Year = 2015});
            
            var response = await client.PostAsync(url,ResponseHandler<TopLists>.SerializeObject(topLists));
            var message = await response.Content.ReadAsStringAsync();
            var toplist = db.TopLists.FirstOrDefault(a => a.UserId == 1);
            
            
            Assert.Equal(topLists.Name,toplist.Name);
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            Assert.Equal("Toplist added successfully",message);
        }
        
        [Fact]
        public async Task Edit_OK()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
            
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token","1=ssss");
            var url = "/TopLists";

            var topLists = new TopLists() {Name = "Coolest 50",Id = 1,UserId = 1,Movies = new List<Movie>()};
            topLists.Movies.Add(new Movie() {Title = "Avengers: Age of Ultron", Id = 2395427, Year = 2015});
            
            var response = await client.PutAsync(url,ResponseHandler<TopLists>.SerializeObject(topLists));
            var message = await response.Content.ReadAsStringAsync();
            var toplist = db.TopLists.FirstOrDefault(a => a.UserId == 1);
            
            
            Assert.Equal(topLists.Name,toplist.Name);
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            Assert.Equal("Toplist added successfully",message);
        }
        
        [Fact]
        public async Task Get_OK()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
            
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token","1=ssss");
            var username = "string";
            var url = $"/TopLists?username={username}";

            
            var response = await client.GetAsync(url);
            var message = ResponseHandler<List<TopLists>>.GetObject(response);
            var toplist = db.TopLists
                .Where(a => a.User.Username == username)
                .AsParallel()
                .ToList();
            
            
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            Assert.Equal(toplist[0].Name,message[0].Name);
        }
    }
}