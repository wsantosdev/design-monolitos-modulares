using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Orders;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.WebApi.Orders
{
    public sealed class OrderCreatedEvent : IEvent
    {
        public DateTime When { get; }
        public OrderId OrderId { get; }
        public AccountId AccountId { get; }
        public OrderSide Side { get; }
        public Quantity Quantity { get; }
        public Symbol Symbol { get; }
        public Money Price { get; }

        private OrderCreatedEvent(DateTime when, OrderId orderId,
                                  AccountId accountId, OrderSide side,
                                  Quantity quantity, Symbol symbol,
                                  Money price)
        {
            When = when;
            OrderId = orderId;
            AccountId = accountId;
            Side = side;
            Quantity = quantity;
            Symbol = symbol;
            Price = price;
        }

        public static OrderCreatedEvent From(Order order) =>
            new(DateTime.UtcNow, order.Id, order.AccountId,
                order.Side, order.Quantity, order.Symbol,
                order.Price);
    }
}
