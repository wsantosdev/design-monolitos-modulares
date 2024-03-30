using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Exchange.Tests
{
    public sealed class OrderStore : IOrderStore
    {
        private readonly Dictionary<OrderId, Order> _orders = new();
        
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
        }
    }
}
