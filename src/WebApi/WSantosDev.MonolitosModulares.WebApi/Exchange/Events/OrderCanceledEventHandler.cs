using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Exchange;
using WSantosDev.MonolitosModulares.Shared;
using WSantosDev.MonolitosModulares.WebApi.Orders;

namespace WSantosDev.MonolitosModulares.WebApi.Exchange
{
    public class OrderCanceledEventHandler : IEventHandler<OrderCanceledEvent>
    {
        private readonly IList<OrderId> _handledOrderIds = new List<OrderId>();

        private readonly IExchangeService _exchangeService;

        public OrderCanceledEventHandler(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        public void Handle(OrderCanceledEvent @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId))
                return;

            _exchangeService.Cancel(@event.OrderId);
            _handledOrderIds.Add(@event.OrderId);
        }
    }
}

