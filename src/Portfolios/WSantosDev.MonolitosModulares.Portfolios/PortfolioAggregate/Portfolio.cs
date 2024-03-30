using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Portfolios
{
    public sealed class Portfolio : Entity<PortfolioId>
    {
        public AccountId AccountId { get; }

        private readonly Dictionary<Symbol, Entry> _entries;
        public IReadOnlyCollection<Entry> Entries => 
            _entries.Values.ToList().AsReadOnly();

        private Portfolio(PortfolioId id, AccountId accountId,
                        Dictionary<Symbol, Entry> entries) : base(id)
        { 
            AccountId = accountId;
            _entries = entries;
        }

        internal static Result<Portfolio> Create(AccountId accountId)
        {
            if (accountId == AccountId.Empty)
                return Result<Portfolio>.Fail(Errors.InvalidAccountId);

            return new Portfolio(PortfolioId.New(), accountId, new Dictionary<Symbol, Entry>());
        }

        internal Result Add(Symbol symbol, Quantity quantity)
        {
            if (_entries.TryGetValue(symbol, out var currentEntry))
                return currentEntry.Add(quantity);

            var entryResult = Entry.Create(symbol, quantity);
            if(entryResult)
                _entries.Add(symbol, entryResult.Value);

            return entryResult;
        }

        internal Result Subtract(Symbol symbol, Quantity quantity)
        {
            if(!_entries.TryGetValue(symbol, out var currentEntry))
                return Result.Fail(Errors.InvalidSymbol);

            if(currentEntry.Quantity < quantity)
                return Result.Fail(Errors.InsuficientAssets);

            var subtractResult = currentEntry.Subtract(quantity);

            if (currentEntry.Quantity == 0)
                _entries.Remove(symbol);

            return subtractResult;
        }
    }
}
