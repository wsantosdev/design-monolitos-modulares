using System.Collections.Generic;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Orders
{
    public interface IOrderStore
    {
        IReadOnlyCollection<Order> GetAllByAccount(AccountId accountId);
        Order GetById(OrderId orderId);

        void AddCreated(Order order);
        void UpdateFilled(Order order);
        void UpdateCanceled(Order order);
    }
}
