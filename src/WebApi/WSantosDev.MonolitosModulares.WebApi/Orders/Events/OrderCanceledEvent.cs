using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.WebApi.Orders
{
    public sealed class OrderCanceledEvent : IEvent
    {
        public DateTime When { get; }
        public OrderId OrderId { get; }

        private OrderCanceledEvent(DateTime when, OrderId orderId)
        {
            When = when;
            OrderId = orderId;
        }

        internal static OrderCanceledEvent Create(OrderId orderId) =>
            new(DateTime.UtcNow, orderId);
    }
}
