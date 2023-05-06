using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Accounts
{
    public interface IAccountService
    {
        Result<Account> Create(AccountId accountId, Money initialDeposit);
        Balance GetBalance(AccountId accountId);
        Result Debit(AccountId accountId, Money amount);
        Result Credit(AccountId accountId, Money amount);
    }
}