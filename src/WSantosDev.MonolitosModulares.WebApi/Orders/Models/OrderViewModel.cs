using WSantosDev.MonolitosModulares.Orders;

namespace WSantosDev.MonolitosModulares.WebApi.Orders
{
    public sealed class OrderViewModel
    {
        public string Id { get; }
        public string Side { get; }
        public int Quantity { get; }
        public string Symbol { get; }
        public decimal Price { get; }
        public string Status { get; }
        public decimal AveragePrice { get; }
        public int AccumulatedQuantity { get; }
        public int LeavesQuantity { get; }

        private OrderViewModel(string id, string side, int quantity,
                               string symbol, decimal price, string status,
                               decimal averagePrice, int accumulatedQuantity,
                               int leavesQuantity)
        {
            Id = id;
            Side = side;
            Quantity = quantity;
            Symbol = symbol;
            Price = price;
            Status = status;
            AveragePrice = averagePrice;
            AccumulatedQuantity = accumulatedQuantity;
            LeavesQuantity = leavesQuantity;
        }

        public static OrderViewModel From(Order order) =>
            new(order.Id.ToString(), order.Side, order.Quantity, 
                order.Symbol, order.Price, order.Status, 
                order.AveragePrice, order.AccumulatedQuantity, 
                order.LeavesQuantity);
    }
}
