using WSantosDev.MonolitosModulares.Exchange;

namespace WSantosDev.MonolitosModulares.WebApi.Exchange
{
    public sealed class OrderViewModel
    {
        public string Id { get; }
        public string Side { get; }
        public int Quantity { get; }
        public string Symbol { get; }
        public decimal Price { get; }
        public string Status { get; }

        private OrderViewModel(string id, string side, int quantity,
                               string symbol, decimal price, string status)
        {
            Id = id;
            Side = side;
            Quantity = quantity;
            Symbol = symbol;
            Price = price;
            Status = status;
        }

        public static OrderViewModel From(Order order) =>
            new(order.Id.ToString(), order.Side, order.Quantity,
                order.Symbol, order.Price, order.Status);
    }
}
