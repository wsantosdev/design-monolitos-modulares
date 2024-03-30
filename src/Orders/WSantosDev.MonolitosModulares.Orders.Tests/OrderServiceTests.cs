using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Orders.Tests
{
    public class OrderServiceTests
    {
        private readonly OrderStore _orderStore;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderStore = new ();
            _orderService = new (_orderStore);
        }

        [Fact]
        public void SendSuccessfully()
        {
            //Arrange
            var accountId = AccountId.New();

            //Act
            var sendResult = _orderService.Send(accountId, OrderSide.Buy, 100, "MODU3", 10);

            //Assert
            Assert.True(sendResult);
            Assert.Equal(sendResult.Value, _orderStore.GetById(sendResult.Value.Id));
        }

        [Theory]
        [ClassData(typeof(CantSendData))]
        public void CantSend(OrderSide side, Quantity quantity, Symbol symbol, 
                             Money price, IError expectedError)
        {
            //Arrange
            var accountId = AccountId.New();

            //Act
            var sendResult = _orderService.Send(accountId, side, quantity, symbol, price);

            //Assert
            Assert.False(sendResult);
            Assert.Equal(expectedError, sendResult.Error);
            Assert.Empty(_orderStore.GetAllByAccount(accountId));
        }

        public class CantSendData : TheoryData<OrderSide, Quantity, Symbol, Money, IError>
        {
            public CantSendData()
            {
                Add(OrderSide.Invalid, 100, "MODU3", 10, Errors.InvalidSide);
                Add(OrderSide.Buy, Quantity.Zero, "MODU3", 20, Errors.InvalidQuantity);
                Add(OrderSide.Sell, 300, Symbol.Empty, 40, Errors.InvalidSymbol);
                Add(OrderSide.Buy, 400, "MODU3", Money.Zero, Errors.InvalidPrice);
            }
        }

        [Fact]
        public void ExecuteSuccessfully()
        {
            //Arrange
            var accountId = AccountId.New();
            var side = OrderSide.Buy;
            var quantity = 100;
            var symbol = "MODU3";
            var price = 10m;

            var order = _orderService.Send(accountId, side, quantity, symbol, price).Value;

            //Act
            var executeResult = _orderService.Execute(order.Id, quantity, price);

            //Assert
            Assert.True(executeResult);
            Assert.Equal(OrderStatus.Filled, _orderStore.GetById(order.Id).Status);
        }

        [Fact]
        public void CantExecuteDueToPreviousExecution()
        {
            //Arrange
            var accountId = AccountId.New();
            var side = OrderSide.Buy;
            var quantity = 100;
            var symbol = "MODU3";
            var price = 10m;

            var order = _orderService.Send(accountId, side, quantity, symbol, price).Value;
            _orderService.Execute(order.Id, quantity, price);

            //Act
            var executeResult = _orderService.Execute(order.Id, quantity, price);

            //Assert
            Assert.False(executeResult);
            Assert.Equal(Errors.AlreadyFilled, executeResult.Error);
        }

        [Fact]
        public void CantExecuteDueToPreviousCancel()
        {
            //Arrange
            var accountId = AccountId.New();
            var side = OrderSide.Buy;
            var quantity = 200;
            var symbol = "MODU3";
            var price = 20m;

            var order = _orderService.Send(accountId, side, quantity, symbol, price).Value;
            _orderService.Cancel(order.Id);

            //Act
            var executeResult = _orderService.Execute(order.Id, quantity, price);

            //Assert
            Assert.False(executeResult);
            Assert.Equal(Errors.AlreadyCanceled, executeResult.Error);
            Assert.Equal(OrderStatus.Canceled, _orderStore.GetById(order.Id).Status);
        }

        [Fact]
        public void CancelSuccessfully()
        {
            //Arrange
            var order = _orderService.Send(AccountId.New(), OrderSide.Buy, 100, "MODU3", 10m).Value;

            //Act
            _orderService.Cancel(order.Id);

            //Assert
            Assert.Equal(OrderStatus.Canceled, order.Status);
            Assert.Equal(OrderStatus.Canceled, _orderStore.GetById(order.Id).Status);
        }

        [Fact]
        public void CantCancelDueToPreviousExecution()
        {
            //Arrange
            var accountId = AccountId.New();
            var side = OrderSide.Buy;
            var quantity = 100;
            var symbol = "MODU3";
            var price = 10m;
            
            var order = _orderService.Send(accountId, side, quantity, symbol, price).Value;
            _orderService.Execute(order.Id, quantity, price);

            //Act
            var cancelResult = _orderService.Cancel(order.Id);

            //Assert
            Assert.False(cancelResult);
            Assert.Equal(Errors.AlreadyFilled, cancelResult.Error);
            Assert.Equal(OrderStatus.Filled, _orderStore.GetById(order.Id).Status);
        }

        [Fact]
        public void CantCancelDueToPreviousCancel()
        {
            //Arrange
            var accountId = AccountId.New();
            var side = OrderSide.Buy;
            var quantity = 200;
            var symbol = "MODU3";
            var price = 20m;

            var order = _orderService.Send(accountId, side, quantity, symbol, price).Value;
            _orderService.Cancel(order.Id);

            //Act
            var cancelResult = _orderService.Cancel(order.Id);

            //Assert
            Assert.False(cancelResult);
            Assert.Equal(Errors.AlreadyCanceled, cancelResult.Error);
            Assert.Equal(OrderStatus.Canceled, _orderStore.GetById(order.Id).Status);
        }
    }
}
