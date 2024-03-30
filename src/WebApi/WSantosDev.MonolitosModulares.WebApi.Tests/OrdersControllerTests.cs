using System.Net;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WSantosDev.MonolitosModulares.Accounts;
using WSantosDev.MonolitosModulares.Portfolios;
using WSantosDev.MonolitosModulares.Shared;
using WSantosDev.MonolitosModulares.WebApi.Orders;
using Ex = WSantosDev.MonolitosModulares.WebApi.Exchange;

namespace WSantosDev.MonolitosModulares.WebApi.Tests
{
    public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;

        public OrdersControllerTests(WebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory;
        }

        static StringContent CreateRequestAsStringContent(string orderSide, int quantity = 100, string symbol = "MODU4") =>
            new(JsonConvert.SerializeObject(new SendOrderRequest(orderSide, quantity, symbol, 10m)),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);

        public async Task<Ex.OrderViewModel[]> GetExchangeOrders()
        {
            var client = _applicationFactory.CreateClient();
            var url = "api/Exchange/Orders";

            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Ex.OrderViewModel[]>(json)!;
        }

        public async Task SendSampleOrder()
        {
            //Arrange
            var client = _applicationFactory.CreateClient();
            var url = "api/Orders/Send";

            //Act
            _ = await client.PostAsync(url, CreateRequestAsStringContent(OrderSide.Buy, 10, "SAMP3"));
        }

        [Fact]
        public async Task CanSendBuyOrder()
        {
            //Arrange
            var client = _applicationFactory.CreateClient();
            var url = "api/Orders/Send";

            //Act
            var response = await client.PostAsync(url, CreateRequestAsStringContent(OrderSide.Buy, 10, "BUY11"));

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var exchangeOrders = await GetExchangeOrders();
            var orderSent = exchangeOrders.Single((o) => o.Side == OrderSide.Buy && o.Symbol == "BUY11");
            
            Assert.Equal(10, orderSent.Quantity);
            Assert.Equal(OrderStatus.New, orderSent.Status);
        }

        [Fact]
        public async Task CanSendSellOrder()
        {
            //Arrange
            using var scope = _applicationFactory.Services.CreateScope();
            var portfolioService = scope.ServiceProvider.GetRequiredService<IPortfolioService>();
            portfolioService.Add(Constants.DefaultAccountId, "SELL3", 100);

            var client = _applicationFactory.CreateClient();
            var url = "api/Orders/Send";

            //Act
            var response = await client.PostAsync(url, CreateRequestAsStringContent(OrderSide.Sell, 20, "SELL3"));

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var exchangeOrders = await GetExchangeOrders();
            var orderSent = exchangeOrders.Single((o) => o.Side == OrderSide.Sell && o.Symbol == "SELL3");

            Assert.Equal(20, orderSent.Quantity);
            Assert.Equal(OrderStatus.New, orderSent.Status);
        }

        [Fact]
        public async Task CanCancelOrder()
        {
            //Arrange
            var client = _applicationFactory.CreateClient();
            await SendSampleOrder();

            var url = "api/Orders/Cancel?";
            var exchangeOrders = await GetExchangeOrders();
            var expected = exchangeOrders.Single((o) => o.Side == OrderSide.Buy 
                                                     && o.Symbol == "SAMP3"
                                                     && o.Status == OrderStatus.New);
            var orderIdParameter = $"orderId={expected.Id}";

            //Act
            var response = await client.PostAsync(url + orderIdParameter, default);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            exchangeOrders = await GetExchangeOrders();
            expected = exchangeOrders.SingleOrDefault((o) => o.Side == OrderSide.Buy 
                                                          && o.Symbol == "SAMP3"
                                                          && o.Status == OrderStatus.Canceled);            
            Assert.NotNull(expected);
        }

        [Fact]
        public async Task CantSendOrderDueToInvalidOrderSide()
        {
            //Arrange
            var client = _applicationFactory.CreateClient();
            var url = "api/Orders/Send";

            //Act
            var response = await client.PostAsync(url, CreateRequestAsStringContent(string.Empty));

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CantSendOrderDueToInvalidQuantity()
        {
            //Arrange
            var client = _applicationFactory.CreateClient();
            var url = "api/Orders/Send";

            //Act
            var response = await client.PostAsync(url, CreateRequestAsStringContent(OrderSide.Buy, 0));

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CantSendOrderDueToInvalidSymbol()
        {
            //Arrange
            var client = _applicationFactory.CreateClient();
            var url = "api/Orders/Send";

            //Act
            var response = await client.PostAsync(url, CreateRequestAsStringContent(OrderSide.Buy, symbol: string.Empty));

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CantSendBuyOrderDueToInsuficientFunds()
        {
            //Arrange
            using var scope = _applicationFactory.Services.CreateScope();
            var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
            var balance = accountService.GetBalance(Constants.DefaultAccountId);
            accountService.Debit(Constants.DefaultAccountId, balance.Value);

            var client = _applicationFactory.CreateClient();
            var url = "api/Orders/Send";

            //Act
            var response = await client.PostAsync(url, CreateRequestAsStringContent(OrderSide.Buy));

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }
        
        [Fact]
        public async Task CantSendSellOrderDueToInsuficientPortfolio()
        {
            //Arrange
            var client = _applicationFactory.CreateClient();
            var url = "api/Orders/Send";

            //Act
            var response = await client.PostAsync(url, CreateRequestAsStringContent(OrderSide.Sell));

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        public void Dispose()
        {
            //Just resets the account balance after each test
            using var scope = _applicationFactory.Services.CreateScope();
            var accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
            accountService.Credit(Constants.DefaultAccountId, 1_000_000m);
        }
    }
}