using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WSantosDev.MonolitosModulares.WebApi.Tests
{
    public class PortfoliosControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;

        public PortfoliosControllerTests(WebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory;
        }

        [Fact]
        public async Task Get()
        {
            var url = "api/Portfolios";
            var client = _applicationFactory.CreateClient();

            //Act
            var response = await client.GetAsync(url);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
