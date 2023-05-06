using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WSantosDev.MonolitosModulares.WebApi.Orders
{
    public sealed class SendOrderRequest
    {
        [DefaultValue("Buy")]
        [RegularExpression("Buy|Sell")]
        public string Side { get; init; }
        
        [DefaultValue(100)]
        public int Quantity { get; init; }

        [DefaultValue("MODU3")]
        public string Symbol { get; init; }

        [DefaultValue(10)]
        public decimal Price { get; init; }

        public SendOrderRequest(string side, int quantity,
                                string symbol, decimal price) =>
            (Side, Quantity, Symbol, Price) = (side, quantity, symbol, price);
    }
}