using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SEP6.Tests.Integration.Utilities;
using TMDbLib.Objects.Movies;
using Xunit;

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
        public async Task Get()
        {
            var client = _factory.CreateClient();
            var id = "1";
            var url = $"/Movies?id={id}";

            var response = await client.GetAsync(url);
            var message = ResponseHandler<Dictionary<string,Object>>.GetObject(response);

            Assert.Empty(message);
        }
    }
}