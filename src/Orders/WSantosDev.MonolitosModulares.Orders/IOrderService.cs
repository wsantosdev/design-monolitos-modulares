using System.Collections.Generic;
using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Orders
{
    public interface IOrderService
    {
        IReadOnlyCollection<Order> GetAllByAccount(AccountId accountId);
        Order GetById(OrderId orderId);

        Result<Order> Send(AccountId accountId, OrderSide side, Quantity quantity, 
                           Symbol symbol, Money price);
        Result Execute(OrderId orderId, Quantity quantity, Money averagePrice);
        Result Cancel(OrderId orderId);
    }
}