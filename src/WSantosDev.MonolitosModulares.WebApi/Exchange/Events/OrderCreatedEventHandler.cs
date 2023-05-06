using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Exchange;
using WSantosDev.MonolitosModulares.Shared;
using WSantosDev.MonolitosModulares.WebApi.Orders;

namespace WSantosDev.MonolitosModulares.WebApi.Exchange
{
    public class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly IList<OrderId> _handledOrderIds = new List<OrderId>();

        private readonly IExchangeService _exchangeService;

        public OrderCreatedEventHandler(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        public void Handle(OrderCreatedEvent @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId))
                return;
            
            _exchangeService.Store(@event.OrderId, @event.AccountId,
                                   @event.Side, @event.Symbol,
                                   @event.Quantity, @event.Price);

            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
