using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Exchange
{
    public interface IOrderStore
    {
        IReadOnlyCollection<Order> GetAll();
        Order GetById(OrderId orderId);
        
        void Add(Order order);
        void Update(Order order);
    }
}