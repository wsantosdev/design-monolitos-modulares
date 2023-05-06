using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Exchange
{
    public sealed class ExchangeService : IExchangeService
    {
        private readonly IOrderStore _store;

        public ExchangeService(IOrderStore store)
        {
            _store = store;
        }

        public void Store(OrderId orderId, AccountId accountId, 
                          OrderSide side, Symbol symbol, 
                          Quantity quantity, Money price)
        {
            _store.Add(Order.Create(orderId, accountId, side, symbol, quantity, price));
        }
        
        public IReadOnlyCollection<Order> GetAll()
        { 
            return _store.GetAll();
        }

        public Order Get(OrderId orderId)
        {
            return _store.GetById(orderId);
        }

        public Result Execute(OrderId orderId)
        {
            var order = _store.GetById(orderId);

            var executeResult = order.Execute();
            if(executeResult)
                _store.Update(order);

            return executeResult;
        }

        public Result Cancel(OrderId orderId)
        {
            var order = _store.GetById(orderId);

            var cancelResult = order.Cancel();
            if(cancelResult)
                _store.Update(order);

            return cancelResult;
        }
    }
}