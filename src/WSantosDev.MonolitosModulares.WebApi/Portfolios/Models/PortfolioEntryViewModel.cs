using WSantosDev.MonolitosModulares.Portfolios;

namespace WSantosDev.MonolitosModulares.WebApi.Portfolios
{
    public sealed class PortfolioEntryViewModel
    {
        public string Symbol { get; }
        public int Quantity { get; }

        private PortfolioEntryViewModel(string symbol, int quantity)
        { 
            Symbol = symbol;
            Quantity = quantity;
        }

        public static PortfolioEntryViewModel From(Entry entry) 
        { 
            return new(entry.Symbol, entry.Quantity);
        }
    }
}
