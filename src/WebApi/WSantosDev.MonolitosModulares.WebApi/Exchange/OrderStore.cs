using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Exchange;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.WebApi.Exchange
{
    public sealed class OrderStore : IOrderStore
    {
        private readonly Dictionary<OrderId, Order> _orders = new();
        private readonly IEventBus _eventBus;

        public OrderStore(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Add(Order order)
        {
            _orders.Add(order.Id, order);
        }

        public IReadOnlyCollection<Order> GetAll()
        {
            return _orders.Values.ToList().AsReadOnly();
        }

        public Order GetById(OrderId orderId)
        {
            return _orders[orderId];
        }

        public void Update(Order order)
        {
            _orders[order.Id] = order;
            if(order.Status == OrderStatus.Filled)
                _eventBus.Publish(ExchangeExecutedEvent.Create(order.Id));
        }
    }
}
