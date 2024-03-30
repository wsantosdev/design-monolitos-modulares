using System.Collections.Generic;
using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Orders
{
    public sealed class OrderService : IOrderService
    {
        private readonly IOrderStore _orderStorage;
        
        public OrderService(IOrderStore orderStorage)
        {
            _orderStorage = orderStorage;
        }

        public IReadOnlyCollection<Order> GetAllByAccount(AccountId accountId)
        { 
            return _orderStorage.GetAllByAccount(accountId);
        }

        public Order GetById(OrderId orderId)
        {
            return _orderStorage.GetById(orderId);
        }

        public Result<Order> Send(AccountId accountId, OrderSide side, 
                                  Quantity quantity, Symbol symbol, 
                                  Money price)
        {
            var createResult = Order.Create(accountId, side, quantity, symbol, price);
            if (createResult)
                _orderStorage.AddCreated(createResult.Value);

            return createResult;
        }

        public Result Execute(OrderId orderId, Quantity quantity, Money price)
        {
            var order = _orderStorage.GetById(orderId);
            
            var executionResult = order.Execute(quantity, price);
            if (executionResult)
                _orderStorage.UpdateFilled(order);

            return executionResult;
        }

        public Result Cancel(OrderId orderId)
        {
            var order = _orderStorage.GetById(orderId);
            
            var cancelResult = order.Cancel();
            if (cancelResult)
                _orderStorage.UpdateCanceled(order);

            return cancelResult;
        }
    }
}
