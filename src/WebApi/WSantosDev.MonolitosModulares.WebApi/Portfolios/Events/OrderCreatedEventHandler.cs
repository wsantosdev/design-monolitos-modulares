using WSantosDev.MonolitosModulares.Commons.Events;
using WSantosDev.MonolitosModulares.Portfolios;
using WSantosDev.MonolitosModulares.Shared;
using WSantosDev.MonolitosModulares.WebApi.Orders;

namespace WSantosDev.MonolitosModulares.WebApi.Portfolios
{
    public sealed class OrderCreatedEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly IList<OrderId> _handledOrderIds = new List<OrderId>();

        private readonly IPortfolioService _portfolioService;

        public OrderCreatedEventHandler(IPortfolioService porfolioService)
        {
            _portfolioService = porfolioService;
        }

        public void Handle(OrderCreatedEvent @event)
        {
            if (_handledOrderIds.Contains(@event.OrderId) || @event.Side == OrderSide.Buy)
                return;

            _portfolioService.Subtract(@event.AccountId, @event.Symbol, @event.Quantity);
            _handledOrderIds.Add(@event.OrderId);
        }
    }
}
