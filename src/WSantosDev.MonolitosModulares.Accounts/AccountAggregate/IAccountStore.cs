using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Accounts
{
    public interface IAccountStore
    {
        Account GetById(AccountId accountId);
        void Add(Account account);
        void Update(Account account);
    }
}
