using WSantosDev.MonolitosModulares.Accounts;
using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Orders;
using WSantosDev.MonolitosModulares.Shared;
using WSantosDev.MonolitosModulares.WebApi.Exchange;

namespace WSantosDev.MonolitosModulares.WebApi.Accounts
{
    public sealed class ExchangeExecutedEventHandler : IEventHandler<ExchangeExecutedEvent>
    {
        private readonly IList<OrderId> _handledOrderIds = new List<OrderId>();

        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;

        public ExchangeExecutedEventHandler(IAccountService accountService, 
                                            IOrderService orderService)
        {
            _accountService = accountService;
            _orderService = orderService;
        }

        public void Handle(ExchangeExecutedEvent @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId))
                return;

            var order = _orderService.GetById(@event.OrderId);
            if (order.Side == OrderSide.Buy)
                return;

            _accountService.Credit(order.AccountId, order.Quantity * order.Price);
            _handledOrderIds.Add(order.Id);
        }
    }
}
