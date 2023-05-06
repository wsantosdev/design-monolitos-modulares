using WSantosDev.MonolitosModulares.Portfolios;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.WebApi.Portfolios
{
    public class PortfolioStore : IPortfolioStore
    {
        private readonly Dictionary<AccountId, Portfolio> _portfolios = new();

        public Portfolio GetByAccount(AccountId accountId)
        {
            return _portfolios[accountId];
        }

        public Entry GetEntryBySymbol(AccountId accountId, Symbol symbol)
        {
            var portfolio = _portfolios[accountId];
            return portfolio.Entries.FirstOrDefault(e => e.Symbol == symbol, Entry.Empty);
        }

        public void Add(Portfolio custody)
        {
            _portfolios.Add(custody.AccountId, custody);
        }

        public void Update(Portfolio portfolio)
        {
            _portfolios[portfolio.AccountId] = portfolio;
        }
    }
}
