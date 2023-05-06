using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Portfolios
{
    public interface IPortfolioService
    {
        Result<Portfolio> Create(AccountId accountId);
        Portfolio GetByAccount(AccountId accountId);
        Entry GetEntryBySymbol(AccountId accountId, Symbol symbol);
        Result Add(AccountId accountId, Symbol symbol, Quantity quantity);
        Result Subtract(AccountId accountId, Symbol symbol, Quantity quantity);
    }
}