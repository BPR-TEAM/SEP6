using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SEP6.Controllers;
using SEP6.Tests.Integration.Utilities;
using Xunit;
using SearchType = SEP6.Controllers.SearchController.SearchType;

namespace SEP6.Tests.Integration.Controllers
{
    public class SearchControllerTests : IClassFixture<CustomApplicationFactory<SEP6.Startup>>
    {
        private readonly CustomApplicationFactory<Startup> _factory;

        public SearchControllerTests(CustomApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        
        [InlineData(SearchType.Movies, "Avengers","Avengers: Age of Ultron 2015, 2395427")]
        [InlineData(SearchType.User, "string","string")]
        [Theory]
        public async Task Search(SearchType searchType,string search,string expectedResult)
        {
            var client = _factory.CreateClient();
            var url = $"/Search?searchText={search}&searchType={searchType}";
            
            var response = await client.GetAsync(url);
            var message = ResponseHandler<List<string>>.GetObject(response);
            
            Assert.Equal(message[0],expectedResult);
        }
        
    }
}