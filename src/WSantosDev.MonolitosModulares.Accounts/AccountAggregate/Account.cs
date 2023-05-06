using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Accounts
{
    public sealed class Account : Entity<AccountId>
    {
        public static readonly Account Default = new(AccountId.Empty, new List<Entry>());
        
        public Balance Balance => _entries.DefaultIfEmpty(Entry.Empty)
                                          .Sum(e => e.Value);

        private readonly IList<Entry> _entries;
        public IReadOnlyCollection<Entry> Entries => _entries.AsReadOnly();

        private Account(AccountId id, IList<Entry> entries) : base(id)
        {
            _entries = entries;
        }

        internal static Result<Account> Create(AccountId accountId, Money initialDeposit)
        {
            if (accountId == AccountId.Empty)
                return Result<Account>.Fail(Errors.EmptyId);

            var account = new Account(accountId, new List<Entry>());
            
            if (initialDeposit > Money.Zero)
                account.Credit(initialDeposit);

            return account;
        }

        internal Result Credit(Money amount)
        {
            var creditResult = Entry.Credit(amount);
            if (creditResult)
                _entries.Add(creditResult.Value);

            return creditResult;
        }

        internal Result Debit(Money amount)
        {
            if (Balance < amount)
                return Result.Fail(Errors.InsuficientFunds);

            var debitResult = Entry.Debit(amount);
            if (debitResult)
                _entries.Add(debitResult.Value);

            return debitResult;
        }
    }
}
