using WSantosDev.MonolitosModulares.Accounts;
using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Orders;
using WSantosDev.MonolitosModulares.Shared;
using WSantosDev.MonolitosModulares.WebApi.Orders;

namespace WSantosDev.MonolitosModulares.WebApi.Accounts
{
    public sealed class OrderCanceledEventHandler : IEventHandler<OrderCanceledEvent>
    {
        private readonly IList<OrderId> _handledOrderIds = new List<OrderId>();

        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;

        public OrderCanceledEventHandler(IAccountService accountService, 
                                         IOrderService orderService)
        {
            _accountService = accountService;
            _orderService = orderService;
        }

        public void Handle(OrderCanceledEvent @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId))
                return;

            var order = _orderService.GetById(@event.OrderId);
            if (order.Side == OrderSide.Sell)
                return;

            _accountService.Credit(order.AccountId, order.Quantity * order.Price);
            _handledOrderIds.Add(order.Id);
        }
    }
}
