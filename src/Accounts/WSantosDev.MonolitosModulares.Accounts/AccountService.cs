using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Accounts
{
    public sealed class AccountService : IAccountService
    {
        private readonly IAccountStore _store;

        public AccountService(IAccountStore store)
        {
            _store = store;
        }
        
        public Result<Account> Create(AccountId accountId, Money initialDeposit)
        {
            var createResult = Account.Create(accountId, initialDeposit);
            if (createResult)
                _store.Add(createResult.Value);

            return createResult;
        }

        public Balance GetBalance(AccountId accountId)
        {
            var account = _store.GetById(accountId);
            return account.Balance;
        }

        public Result Credit(AccountId accountId, Money amount)
        {
            var account = _store.GetById(accountId);
            
            var creditResult = account.Credit(amount);
            if (creditResult)
                _store.Update(account);

            return creditResult;
        }

        public Result Debit(AccountId accountId, Money amount)
        {
            var account = _store.GetById(accountId);
            
            var debitResult = account.Debit(amount);
            if(debitResult)
                _store.Update(account);

            return debitResult;
        }
    }
}
