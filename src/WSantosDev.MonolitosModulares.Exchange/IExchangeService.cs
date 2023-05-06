using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Exchange
{
    public interface IExchangeService
    {
        void Store(OrderId orderId, AccountId accountId, OrderSide side, 
                   Symbol symbol, Quantity quantity, Money price);
        IReadOnlyCollection<Order> GetAll();
        Order Get(OrderId orderId);
        Result Execute(OrderId orderId);
        Result Cancel(OrderId orderId);
    }
}