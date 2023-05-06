using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.WebApi.Exchange
{
    public sealed class ExchangeExecutedEvent : IEvent
    {
        public DateTime When { get; }
        public OrderId OrderId { get; }

        private ExchangeExecutedEvent(DateTime when, OrderId orderId)
        {
            When = when;
            OrderId = orderId;
        }

        public static ExchangeExecutedEvent Create(OrderId orderId) =>
            new(DateTime.UtcNow, orderId);
    }
}
