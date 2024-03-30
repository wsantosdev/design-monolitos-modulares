using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Accounts.Tests
{
    public class AccountServiceTests
    {
        private readonly AccountStore _accountStore;
        private readonly AccountService _accountService;

        public AccountServiceTests()
        {
            _accountStore = new ();
            _accountService = new (_accountStore);
        }

        [Fact]
        public void CreateAccountSuccessfully()
        { 
            //Arrange
            var accountId = AccountId.New();
            var initialDeposit = (Money) 10_000m;

            //Act
            var createdResult = _accountService.Create(accountId, initialDeposit);

            //Assert
            Assert.True(createdResult);
            Assert.Equal(accountId, createdResult.Value.Id);
            Assert.Equal(initialDeposit.Value, createdResult.Value.Balance.Value);
            Assert.Same(_accountStore.GetById(accountId), createdResult.Value);
        }

        [Fact]
        public void CantCreateAccountDueToInvalidId()
        {
            //Arrange
            var accountId = AccountId.Empty;
            var initialDeposit = (Money) 20_000m;

            //Act
            var createdResult = _accountService.Create(accountId, initialDeposit);

            //Assert
            Assert.False(createdResult);
            Assert.Equal(Errors.EmptyId, createdResult.Error);
            Assert.Throws<KeyNotFoundException>(() => _accountStore.GetById(accountId));
        }

        [Fact]
        public void GetBalanceSuccessfully()
        {
            //Arrange
            var accountId = AccountId.New();
            var initialDeposit = (Money) 30_000m;
            var account = _accountService.Create(accountId, initialDeposit).Value;

            //Act
            var balance = _accountService.GetBalance(accountId);

            //Assert
            Assert.Equal(account.Balance, balance);
        }

        [Fact]
        public void CreditSuccessfully()
        {
            //Arrange
            var accountId = AccountId.New();
            var initialDeposit = (Money) 40_000m;
            var account = _accountService.Create(accountId, initialDeposit).Value;
            var creditAmount = (Money) 500m;

            //Act
            var creditResult = _accountService.Credit(accountId, creditAmount);
            var balance = account.Balance;

            //Assert
            Assert.True(creditResult);
            Assert.Equal(initialDeposit + creditAmount, balance.Value);
        }

        [Fact]
        public void CantCreditDueToZeroedValue()
        {
            //Arrange
            var accountId = AccountId.New();
            var initialDeposit = (Money) 40_000m;
            var account = _accountService.Create(accountId, initialDeposit).Value;
            var creditAmount = Money.Zero;

            //Act
            var creditResult = _accountService.Credit(accountId, creditAmount);
            
            //Assert
            Assert.False(creditResult);
            Assert.Equal(Errors.InvalidAmount, creditResult.Error);
            Assert.Equal(initialDeposit.Value, account.Balance.Value);
        }

        [Fact]
        public void DebitSuccessfully()
        {
            //Arrange
            var accountId = AccountId.New();
            var initialDeposit = (Money) 50_000m;
            var account = _accountService.Create(accountId, initialDeposit).Value;
            var debitAmount = (Money) 10_000m;

            //Act
            var debitResult = _accountService.Debit(accountId, debitAmount);

            //Assert
            Assert.True(debitResult);
            Assert.Equal(initialDeposit - debitAmount, account.Balance.Value);
        }

        [Theory]
        [ClassData(typeof(CantDebitData))]
        public void CantDebit(Money debitAmount, IError expectedError)
        {
            //Arrange
            var accountId = AccountId.New();
            var initialDeposit = (Money) 60_000m;
            var account = _accountService.Create(accountId, initialDeposit).Value;

            //Act
            var debitResult = _accountService.Debit(accountId, debitAmount);

            //Assert
            Assert.False(debitResult);
            Assert.Equal(expectedError, debitResult.Error);
            Assert.Equal(initialDeposit.Value, account.Balance.Value);
        }

        public class CantDebitData : TheoryData<Money, IError>
        { 
            public CantDebitData() 
            {
                Add(Money.Zero, Errors.InvalidAmount);
                Add(80_000, Errors.InsuficientFunds);
            }
        }
    }
}
