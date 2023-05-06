using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Orders
{
    public sealed class Order : Entity<OrderId>
    {
        public AccountId AccountId { get; }
        public OrderSide Side { get; }
        public Quantity Quantity { get; }
        public Symbol Symbol { get; }
        public Money Price { get; }
        public OrderStatus Status { get; private set; }
        public Money AveragePrice => _trades.DefaultIfEmpty(Trade.None).Average(t => t.AveragePrice);
        public Quantity AccumulatedQuantity => _trades.DefaultIfEmpty(Trade.None).Sum(t => t.Quantity);
        public Quantity LeavesQuantity => Quantity - AccumulatedQuantity;

        private readonly IList<Trade> _trades;
        public ReadOnlyCollection<Trade> Trades => _trades.AsReadOnly();

        private Order(OrderId id, AccountId accountId, OrderSide side,
                      Quantity quantity, Symbol symbol, Money price,
                      OrderStatus status, IList<Trade> trades) : base(id)
        {
            AccountId = accountId;
            Side = side;
            Symbol = symbol;
            Quantity = quantity;
            Price = price;
            Status = status;

            _trades = trades;
        }

        internal static Result<Order> Create(AccountId accountId, OrderSide side,
                                             Quantity quantity, Symbol symbol,
                                             Money price)
        {
            if (side == OrderSide.Invalid)
                return Result<Order>.Fail(Errors.InvalidSide);
            if (quantity == Quantity.Zero)
                return Result<Order>.Fail(Errors.InvalidQuantity);
            if (symbol == Symbol.Empty)
                return Result<Order>.Fail(Errors.InvalidSymbol);
            if (price == Money.Zero)
                return Result<Order>.Fail(Errors.InvalidPrice);

            return new Order(OrderId.New(), accountId, side, quantity, 
                             symbol, price, OrderStatus.New, new List<Trade>());
        }

        internal Result Execute(Quantity quantity, Money averagePrice)
        {
            if (Status == OrderStatus.Filled)
                return Result.Fail(Errors.AlreadyFilled);
            if (Status == OrderStatus.Canceled)
                return Result.Fail(Errors.AlreadyCanceled);

            var tradeResult = Trade.Create(quantity, averagePrice);
            if (tradeResult)
            {
                _trades.Add(tradeResult.Value);
                Status = OrderStatus.Filled;
            }    

            return tradeResult;
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