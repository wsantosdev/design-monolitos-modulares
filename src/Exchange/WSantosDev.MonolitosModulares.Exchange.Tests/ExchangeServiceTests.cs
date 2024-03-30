using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Exchange.Tests
{
    public class ExchangeServiceTests
    {
        private readonly OrderStore _orderStore;
        private readonly ExchangeService _exchangeService;

        public ExchangeServiceTests()
        {
            _orderStore = new ();
            _exchangeService = new(_orderStore);
        }

        [Fact]
        public void StoreSuccessfully()
        {
            //Arrange
            var orderId = OrderId.New();
            var accountId = AccountId.New();

            //Act
            _exchangeService.Store(orderId, accountId, OrderSide.Buy, "MODU3", 100, 10m);

            //Assert
            Assert.IsType<Order>(_orderStore.GetById(orderId));
        }

        [Fact]
        public void GetAllOrdersSuccessfully()
        {
            //Arrange
            var accountId = AccountId.New();
            _exchangeService.Store(OrderId.New(), accountId, OrderSide.Buy, "MODU3", 200, 20m);
            _exchangeService.Store(OrderId.New(), accountId, OrderSide.Sell, "MODU3", 200, 20m);

            //Act
            var orders = _exchangeService.GetAll();

            //Assert
            Assert.Equal(2, orders.Count);
        }

        [Fact]
        public void GetOrderByIdSuccessfully()
        {
            //Arrange
            var orderId = OrderId.New();
            var accountId = AccountId.New();
            _exchangeService.Store(orderId, accountId, OrderSide.Buy, "MODU3", 300, 30m);

            //Act
            var order = _exchangeService.Get(orderId);

            //Assert
            Assert.Equal(orderId, order.Id);
        }

        [Fact]
        public void ExecuteOrderSuccessfully()
        {
            //Arrange
            var orderId = OrderId.New();
            var accountId = AccountId.New();
            _exchangeService.Store(orderId, accountId, OrderSide.Buy, "MODU3", 400, 40m);
            var order = _exchangeService.Get(orderId);

            //Act
            var executeResult = _exchangeService.Execute(orderId);

            //Assert
            Assert.True(executeResult);
            Assert.Equal(OrderStatus.Filled, order.Status);
            Assert.Equal(_orderStore.GetById(orderId).Status, order.Status);
        }

        [Fact]
        public void CancelOrderSuccessfully()
        {
            //Arrange
            var orderId = OrderId.New();
            var accountId = AccountId.New();
            _exchangeService.Store(orderId, accountId, OrderSide.Buy, "MODU3", 500, 50m);
            var order = _exchangeService.Get(orderId);

            //Act
            var cancelResult = _exchangeService.Cancel(orderId);

            //Assert
            Assert.True(cancelResult);
            Assert.Equal(OrderStatus.Canceled, order.Status);
            Assert.Equal(_orderStore.GetById(orderId).Status, order.Status);
        }
    }
}
