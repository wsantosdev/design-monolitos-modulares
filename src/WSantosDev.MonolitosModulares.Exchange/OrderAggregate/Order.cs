using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Exchange
{
    public sealed class Order : Entity<OrderId>
    {
        public AccountId AccountId { get; }
        public OrderSide Side { get; }
        public Symbol Symbol { get; }
        public Quantity Quantity { get; }
        public Money Price { get; }
        public OrderStatus Status { get; private set; }

        private Order(OrderId id, AccountId accountId, 
                      OrderSide side, Symbol symbol, 
                      Quantity quantity, Money price,
                      OrderStatus status) : base(id)
        {
            AccountId = accountId;
            Side = side;
            Quantity = quantity;
            Symbol = symbol;
            Price = price;
            Status = status;
        }

        internal static Order Create(OrderId orderId, AccountId accountId,
                                     OrderSide side, Symbol symbol,
                                     Quantity quantity, Money price)
        { 
            return new(orderId, accountId, side, symbol, quantity, price, OrderStatus.New);
        }

        internal Result Execute()
        {
            if (Status == OrderStatus.Filled)
                return Result.Fail(Errors.AlreadyFilled);
            
            if (Status == OrderStatus.Canceled)
                return Result.Fail(Errors.AlreadyCanceled);

            Status = OrderStatus.Filled;
            return Result.Success;
        }

        internal Result Cancel()
        {
            if (Status == OrderStatus.Canceled)
                return Result.Fail(Errors.AlreadyCanceled);

            if (Status == OrderStatus.Filled)
                return Result.Fail(Errors.AlreadyFilled);

            Status = OrderStatus.Canceled;
            return Result.Success;
        }
    }
}