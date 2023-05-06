using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Portfolios
{
    public sealed class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioStore _portfolioStore;

        public PortfolioService(IPortfolioStore portfolio)
        {
            _portfolioStore = portfolio;
        }

        public Result<Portfolio> Create(AccountId accountId)
        {
            var createResult = Portfolio.Create(accountId);
            if(createResult)
                _portfolioStore.Add(createResult.Value);

            return createResult;
        }

        public Portfolio GetByAccount(AccountId accountId)
        {
            return _portfolioStore.GetByAccount(accountId);
        }

        public Entry GetEntryBySymbol(AccountId accountId, Symbol symbol)
        {
            return _portfolioStore.GetEntryBySymbol(accountId, symbol);
        }

        public Result Add(AccountId accountId, Symbol symbol, Quantity quantity)
        {
            var portfolio = _portfolioStore.GetByAccount(accountId);
            
            var addResult = portfolio.Add(symbol, quantity);
            if (addResult)
                _portfolioStore.Update(portfolio);

            return addResult;
        }

        public Result Subtract(AccountId accountId, Symbol symbol, Quantity quantity)
        {
            var portfolio =_portfolioStore.GetByAccount(accountId);
            
            var subtractResult = portfolio.Subtract(symbol, quantity);
            if (subtractResult)
                _portfolioStore.Update(portfolio);

            return subtractResult;
        }
    }
}
