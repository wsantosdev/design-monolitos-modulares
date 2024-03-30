using WSantosDev.MonolitosModulares.Accounts;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.WebApi.Accounts
{
    public sealed class AccountStore : IAccountStore
    {
        private readonly Dictionary<AccountId, Account> _accounts = new();

        public Account GetById(AccountId accountId)
        {
            return _accounts[accountId];
        }
        
        public void Add(Account account)
        {
            _accounts.Add(account.Id, account);
        }

        public void Update(Account account)
        {
            _accounts[account.Id] = account;
        }      
    }
}
