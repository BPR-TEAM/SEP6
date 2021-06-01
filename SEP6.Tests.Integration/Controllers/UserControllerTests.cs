using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SEP6.DB;
using SEP6.Tests.Integration.Utilities;

namespace SEP6.Tests.Integration.Controllers
{
    public class UserControllerTests : IClassFixture<CustomApplicationFactory<SEP6.Startup>>
    {
        private readonly CustomApplicationFactory<Startup> _factory;

        public UserControllerTests(CustomApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Follow_Unathorized_TokenNotVerified()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token","1=ghjgss");
            
            var url = "/User";
            var userToFollow = ResponseHandler<string>.SerializeObject("manel");

            var response = await client.PostAsync(url,userToFollow);
            var message = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(HttpStatusCode.Unauthorized,response.StatusCode);
            Assert.Equal("Token expired",message);
        }
        
        [Fact]
        public async Task Follow_UserToFollow_NotFound()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token","1=ssss");
            
            var userToFollow ="manel";
            var url = $"/User?userToFollow={userToFollow}";

            var response = await client.PostAsync(url,null);
            var message = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
            Assert.Equal($"User {userToFollow} not found",message);
        }
        
        [Fact]
        public async Task Follow_OK()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
            
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token","1=ssss");
            
            var userToFollow ="stringy";
            var url = $"/User?userToFollow={userToFollow}";

            var response = await client.PostAsync(url,null);
            var message = await response.Content.ReadAsStringAsync();

            var userFollow = db.Users
                .Include(u=>u.Follows)
                .FirstOrDefault(u => u.Id == 1);
            
            Assert.Equal(userToFollow,userFollow?.Follows.FirstOrDefault()?.Username);
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
            Assert.Equal($"{userFollow?.Username} now follows {userToFollow}!",message);
        }
        
        [Fact]
        public async Task Follow_OK_Unfollow()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("token","2=hhhh");
            
            var userToUnfollow ="string";
            var url = $"/User?userToFollow={userToUnfollow}";

            var unfollowResponse = await client.PostAsync(url,null);
            var message = await unfollowResponse.Content.ReadAsStringAsync();
            
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
            var userFollow = db.Users
                .Include(u=>u.Follows)
                .FirstOrDefault(u => u.Id == 2);
            
            Assert.Empty(userFollow.Follows);
            Assert.Equal(HttpStatusCode.OK,unfollowResponse.StatusCode);
            Assert.Equal($"{userFollow?.Username} doesn't follow {userToUnfollow} anymore!",message);
        }
    }
}