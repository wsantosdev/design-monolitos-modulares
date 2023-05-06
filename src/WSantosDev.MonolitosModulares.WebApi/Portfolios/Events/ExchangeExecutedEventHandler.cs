using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Portfolios;
using WSantosDev.MonolitosModulares.Orders;
using WSantosDev.MonolitosModulares.Shared;
using WSantosDev.MonolitosModulares.WebApi.Exchange;

namespace WSantosDev.MonolitosModulares.WebApi.Portfolios
{
    public class ExchangeExecutedEventHandler : IEventHandler<ExchangeExecutedEvent>
    {
        private readonly IList<OrderId> _handledOrderIds = new List<OrderId>();

        private readonly IOrderService _orderService;
        private readonly IPortfolioService _portfolioService;

        public ExchangeExecutedEventHandler(IOrderService orderService,
                                            IPortfolioService portfolioService)
        {
            _orderService = orderService;
            _portfolioService = portfolioService;
        }

        public void Handle(ExchangeExecutedEvent @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId))
                return;

            var order = _orderService.GetById(@event.OrderId);
            if (order.Side == OrderSide.Sell)
                _portfolioService.Subtract(order.AccountId, order.Symbol, order.Quantity);

            if(order.Side == OrderSide.Buy)
                _portfolioService.Add(order.AccountId, order.Symbol, order.Quantity);

            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
