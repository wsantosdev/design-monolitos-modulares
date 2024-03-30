using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WSantosDev.MonolitosModulares.WebApi.Tests
{
    public class AccountsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;

        public AccountsControllerTests(WebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory;
        }

        [Fact]
        public async Task CanGetBallance()
        {
            //Arrange
            var url = "api/Accounts/Balance";
            var client = _applicationFactory.CreateClient();

            //Act
            var response = await client.GetAsync(url);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData(1, HttpStatusCode.OK)]
        [InlineData(0, HttpStatusCode.BadRequest)]
        public async Task TryCredit(decimal amount, HttpStatusCode expected)
        {
            //Arrange
            var url = $"api/Accounts/Credit/?amount={amount}";
            var client = _applicationFactory.CreateClient();

            //Act
            var response = await client.PostAsync(url, default);

            //Assert
            Assert.Equal(expected, response.StatusCode);
        }
    }
}
