using System.Net;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using WSantosDev.MonolitosModulares.WebApi.Orders;
using Ex = WSantosDev.MonolitosModulares.WebApi.Exchange;

namespace WSantosDev.MonolitosModulares.WebApi.Tests
{
    public class ExchangeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;

        public ExchangeControllerTests(WebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory;
        }

        [Fact]
        public async Task GetAllOrders()
        {
            //Arrange
            var url = "api/Exchange/Orders";
            var client = _applicationFactory.CreateClient();

            //Act
            var response = await client.GetAsync(url);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        static StringContent CreateRequestAsStringContent() =>
            new (JsonConvert.SerializeObject(new SendOrderRequest("Buy", 100, "MODU4", 10m)), 
                 Encoding.UTF8, 
                 MediaTypeNames.Application.Json);

        static async Task<Ex.OrderViewModel> ExtractOrder(HttpResponseMessage httpResponseMessage) =>
            JsonConvert.DeserializeObject<Ex.OrderViewModel>(await httpResponseMessage.Content.ReadAsStringAsync())!;

        [Fact]
        public async Task CanExecute()
        {
            //Arrange
            var client = _applicationFactory.CreateClient();
            var orderSendResponse = await client.PostAsync("api/Orders/Send", CreateRequestAsStringContent());
            var order = await ExtractOrder(orderSendResponse);
            var url = $"api/Exchange/Execute?orderId={order.Id}";

            //Act
            var response = await client.PostAsync(url, default);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CantExecuteDueToFilledStatus()
        {
            //Arrange
            var client = _applicationFactory.CreateClient();
            var orderSendResponse = await client.PostAsync("api/Orders/Send", CreateRequestAsStringContent());
            var order = await ExtractOrder(orderSendResponse);
            var url = $"api/Exchange/Execute?orderId={order!.Id}";
            await client.PostAsync(url, default);

            //Act
            var response = await client.PostAsync(url, default);

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task CantExecuteDueToCancelledStatus()
        {
            //Arrange
            var client = _applicationFactory.CreateClient();
            var orderSendResponse = await client.PostAsync("api/Orders/Send", CreateRequestAsStringContent());
            var order = await ExtractOrder(orderSendResponse);
            await client.PostAsync($"api/Orders/Cancel?orderId={order!.Id}", default);
            var url = $"api/Exchange/Execute?orderId={order!.Id}";

            //Act
            var response = await client.PostAsync(url, default);

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
    }
}
