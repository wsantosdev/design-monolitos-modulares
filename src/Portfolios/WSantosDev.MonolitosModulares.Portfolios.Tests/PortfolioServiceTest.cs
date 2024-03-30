using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Portfolios.Tests
{
    public class PortfolioServiceTest
    {
        private readonly PortfolioStore _portfolioStore;
        private readonly PortfolioService _portfolioService;

        public PortfolioServiceTest()
        {
            _portfolioStore = new ();
            _portfolioService = new (_portfolioStore);
        }

        [Fact]
        public void CreatePortfolioSuccessfully()
        { 
            //Arrange
            var accountId = AccountId.New();

            //Act
            var createResult = _portfolioService.Create(accountId);

            //Assert
            Assert.True(createResult);
            Assert.Same(_portfolioStore.GetByAccount(accountId), createResult.Value);
        }

        [Fact]
        public void CantCreateDueInvalidAccountId()
        {
            //Arrange
            var accountId = AccountId.Empty;

            //Act
            var createResult = _portfolioService.Create(accountId);

            //Assert
            Assert.False(createResult);
            Assert.Equal(Errors.InvalidAccountId, createResult.Error);
            Assert.Throws<KeyNotFoundException>(() => _portfolioStore.GetByAccount(accountId));
        }

        [Fact]
        public void GetByAccountSuccessfully()
        {
            //Arrange
            var accountId = AccountId.New();
            _portfolioService.Create(accountId);

            //Act
            var portfolio = _portfolioService.GetByAccount(accountId);

            //Assert
            Assert.Equal(accountId, portfolio.AccountId);
        }

        [Fact]
        public void AddToPortfolioSuccessfully()
        {
            //Arrange
            var accountId = AccountId.New();
            var symbol = "MODU3";
            var quantity = 100;
            var porftolio = _portfolioService.Create(accountId).Value;

            //Act
            var addResult = _portfolioService.Add(accountId, symbol, quantity);

            //Assert
            Assert.True(addResult);
            Assert.Same(_portfolioStore.GetEntryBySymbol(accountId, symbol),
                        porftolio.Entries.First(e => e.Symbol == symbol));
        }

        [Fact]
        public void AddMoreToPortfolioSuccessfully()
        {
            //Arrange
            var accountId = AccountId.New();
            var symbol = "MODU3";
            var quantity = 200;
            var porftolio = _portfolioService.Create(accountId).Value;
            _portfolioService.Add(accountId, symbol, quantity);
            var quantityToAdd = 50;

            //Act
            var addResult = _portfolioService.Add(accountId, symbol, quantityToAdd);

            //Assert
            Assert.True(addResult);
            Assert.Equal<Quantity>(quantity + quantityToAdd, 
                                   porftolio.Entries.First(e => e.Symbol == symbol).Quantity);
            Assert.Equal<Quantity>(quantity + quantityToAdd, 
                                   _portfolioStore.GetEntryBySymbol(accountId, symbol).Quantity);
        }

        [Theory]
        [ClassData(typeof(CantAddData))]
        public void CantAdd(Symbol symbol, Quantity quantity, IError expectedError)
        {
            //Arrange
            var accountId = AccountId.New();
            var porftolio = _portfolioService.Create(accountId).Value;
            
            //Act
            var addResult = _portfolioService.Add(accountId, symbol, quantity);

            //Assert
            Assert.False(addResult);
            Assert.Equal(expectedError, addResult.Error);
            Assert.DoesNotContain(porftolio.Entries, e => e.Symbol == symbol);
            Assert.Same(Entry.Empty, _portfolioStore.GetEntryBySymbol(accountId, symbol));
        }

        public class CantAddData : TheoryData<Symbol, Quantity, IError>
        {
            public CantAddData()
            {
                Add(Symbol.Empty, 100, Errors.InvalidSymbol);
                Add("MODU3", 0, Errors.InvalidQuantity);
            }
        }

        [Fact]
        public void GetEntrySuccessfully()
        {
            //Arrange
            var accountId = AccountId.New();
            _portfolioService.Create(accountId);
            var symbol = "MODU3";
            var quantity = 100;
            _portfolioService.Add(accountId, symbol, quantity);

            //Act
            var entry = _portfolioService.GetEntryBySymbol(accountId, symbol);

            //Assert
            Assert.Equal<Quantity>(entry.Quantity, quantity);
        }

        [Fact]
        public void CantGetEntry()
        {
            //Arrange
            var accountId = AccountId.New();
            _portfolioService.Create(accountId);
            var symbol = "MODU3";

            //Act
            var entry = _portfolioService.GetEntryBySymbol(accountId, symbol);

            //Assert
            Assert.Same(Entry.Empty, entry);
        }

        [Fact]
        public void SubtractFromPortfolioSuccessfully() 
        {
            //Arrange
            var accountId = AccountId.New();
            var symbol = "MODU3";
            var quantity = 500;
            var porftolio = _portfolioService.Create(accountId).Value;
            _portfolioService.Add(accountId, symbol, quantity);
            var quantityToSubtract = 100;

            //Act
            var subtractResult = _portfolioService.Subtract(accountId, symbol, quantityToSubtract);

            //Assert
            Assert.True(subtractResult);
            Assert.Equal<Quantity>(quantity - quantityToSubtract,
                                   porftolio.Entries.First(e => e.Symbol == symbol).Quantity);
            Assert.Same(_portfolioStore.GetEntryBySymbol(accountId, symbol).Quantity,
                         porftolio.Entries.First(e => e.Symbol == symbol).Quantity);
        }
                
        [Theory]
        [ClassData(typeof(CantSubtractData))]
        public void CantSubtract(Symbol symbolToAdd, Symbol symbolToRetrieve, 
                                 Quantity quantityToSubtract, IError expectedError)
        {
            //Arrange
            var accountId = AccountId.New();
            var quantity = 800;
            var porftolio = _portfolioService.Create(accountId).Value;
            _portfolioService.Add(accountId, symbolToAdd, quantity);
            
            //Act
            var addResult = _portfolioService.Subtract(accountId, symbolToRetrieve, quantityToSubtract);

            //Assert
            Assert.False(addResult);
            Assert.Equal(expectedError, addResult.Error);
            Assert.Equal<Quantity>(quantity, porftolio.Entries.First(e => e.Symbol == symbolToAdd).Quantity);
            Assert.Equal<Quantity>(quantity, _portfolioStore.GetEntryBySymbol(accountId, symbolToAdd).Quantity);
        }

        public class CantSubtractData : TheoryData<Symbol, Symbol, Quantity, IError>
        {
            public CantSubtractData()
            {
                Add("MODU3", Symbol.Empty, 100, Errors.InvalidSymbol);
                Add("MODU3", "MODU3", Quantity.Zero, Errors.InvalidQuantity);
                Add("MODU3", "MODU3", 1000, Errors.InsuficientAssets);
            }
        }
    }
}
