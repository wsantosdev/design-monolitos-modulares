using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Orders;
using WSantosDev.MonolitosModulares.Shared;
using WSantosDev.MonolitosModulares.WebApi.Exchange;

namespace WSantosDev.MonolitosModulares.WebApi.Orders
{
    public class ExchangeExecutedEventHandler : IEventHandler<ExchangeExecutedEvent>
    {
        private readonly IList<OrderId> _handledOrderIds = new List<OrderId>();

        private readonly IOrderService _orderService;

        public ExchangeExecutedEventHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public void Handle(ExchangeExecutedEvent @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId))
                return;

            var order = _orderService.GetById(@event.OrderId);
            _orderService.Execute(order.Id, order.Quantity, order.Price);

            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
