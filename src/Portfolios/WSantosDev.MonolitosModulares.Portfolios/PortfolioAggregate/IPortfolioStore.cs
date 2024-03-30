using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Portfolios
{
    public interface IPortfolioStore
    {
        Portfolio GetByAccount(AccountId accountId);
        Entry GetEntryBySymbol(AccountId accountId, Symbol symbol);
        void Add(Portfolio custody);
        void Update(Portfolio custody);
    }
}
