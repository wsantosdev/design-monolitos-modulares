using WSantosDev.MonolitosModulares.Accounts;
using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Shared;
using WSantosDev.MonolitosModulares.WebApi.Orders;

namespace WSantosDev.MonolitosModulares.WebApi.Accounts
{
    public sealed class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly IList<OrderId> _handledOrderIds = new List<OrderId>();

        private readonly IAccountService _accountService;

        public OrderCreatedEventHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public void Handle(OrderCreatedEvent @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId) || @event.Side == OrderSide.Sell)
                return;

            _accountService.Debit(@event.AccountId, @event.Quantity * @event.Price);
            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
