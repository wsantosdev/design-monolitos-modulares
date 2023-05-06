using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Orders;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.WebApi.Orders
{
    public sealed class OrderStore : IOrderStore
    {
        private readonly Dictionary<OrderId, Order> _orders = new();
        private readonly IEventBus _eventBus;

        public OrderStore(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public IReadOnlyCollection<Order> GetAllByAccount(AccountId accountId)
        {
            return _orders.Values.Where(o => o.AccountId == accountId)
                                 .ToList()
                                 .AsReadOnly();
        }

        public Order GetById(OrderId orderId)
        {
            return _orders[orderId];
        }

        public void AddCreated(Order order)
        {
            _orders.Add(order.Id, order);
            _eventBus.Publish(OrderCreatedEvent.From(order));
        }

        public void UpdateFilled(Order order) 
        {
            _orders[order.Id] = order;
        }

        public void UpdateCanceled(Order order)
        {
            _orders[order.Id] = order;
            _eventBus.Publish(OrderCanceledEvent.Create(order.Id));
        }
    }
}
