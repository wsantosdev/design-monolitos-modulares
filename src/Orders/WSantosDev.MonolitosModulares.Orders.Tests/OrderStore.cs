using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Orders.Tests
{
    public sealed class OrderStore : IOrderStore
    {
        private readonly Dictionary<OrderId, Order> _orders = new();
        
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
        }

        public void UpdateFilled(Order order)
        {
            _orders[order.Id] = order;
        }

        public void UpdateCanceled(Order order)
        {
            _orders[order.Id] = order;
        }
    }
}
