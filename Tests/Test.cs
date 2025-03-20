using Domain.Entities;
using Domain.Patterns.Facades;
using Domain.Repository;
using Moq;
using Xunit;

public class FinanceFacadeTests
{
    private readonly Mock<IFinanceManager> _financeManagerMock;
    private readonly FinanceFacade _financeFacade;

    public FinanceFacadeTests()
    {
        _financeManagerMock = new Mock<IFinanceManager>();
        _financeFacade = new FinanceFacade(_financeManagerMock.Object);
    }

    [Fact]
    public void CreateBankAccount_ShouldAddAccountToManager()
    {
        // Arrange
        var account = new BankAccount { Name = "Test Account", Balance = 100 };
        _financeManagerMock.Setup(m => m.AddBankAccount(It.IsAny<BankAccount>())).Verifiable();

        // Act
        var result = _financeFacade.CreateBankAccount("Test Account", 100);

        // Assert
        _financeManagerMock.Verify(m => m.AddBankAccount(It.IsAny<BankAccount>()), Times.Once);
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal("Test Account", result.Name);
        Xunit.Assert.Equal(100, result.Balance);
    }

    [Fact]
    public void GetBankAccounts_ShouldReturnAllAccounts()
    {
        // Arrange
        var accounts = new List<BankAccount>
        {
            new BankAccount { Name = "Account 1", Balance = 100 },
            new BankAccount {Name = "Account 2", Balance = 200 }
        };
        _financeManagerMock.Setup(m => m.GetAllAccounts()).Returns(accounts);

        // Act
        var result = _financeFacade.GetBankAccounts();

        // Assert
        Xunit.Assert.Equal(2, result.Count);
        Xunit.Assert.Contains(result, a => a.Name == "Account 1");
        Xunit.Assert.Contains(result, a => a.Name == "Account 2");
    }
    
    [Fact]
    public void CreateOperation_Income_ShouldDeposit()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var category = new Category { EntityType = EntityType.Income, Name = "Salary" };
        _financeManagerMock.Setup(m => m.Deposit(accountId, 100, "Salary", category)).Verifiable();

        // Act
        var result = _financeFacade.CreateOperation(EntityType.Income, accountId, 100, "Salary", category);

        // Assert
        _financeManagerMock.Verify(m => m.Deposit(accountId, 100, "Salary", category), Times.Once);
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(EntityType.Income, result.EntityType);
        Xunit.Assert.Equal(100, result.Amount);
    }

    [Fact]
    public void CreateOperation_Expense_ShouldWithdraw()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var category = new Category { EntityType = EntityType.Expense, Name = "Cafe" };
        _financeManagerMock.Setup(m => m.Withdraw(accountId, 50, "Cafe", category)).Verifiable();

        // Act
        var result = _financeFacade.CreateOperation(EntityType.Expense, accountId, 50, "Cafe", category);

        // Assert
        _financeManagerMock.Verify(m => m.Withdraw(accountId, 50, "Cafe", category), Times.Once);
        Xunit.Assert.NotNull(result);
        Xunit.Assert.Equal(EntityType.Expense, result.EntityType);
        Xunit.Assert.Equal(50, result.Amount);
    }
    
    [Fact]
    public void CalculateNetBalance_ShouldReturnCorrectDifference()
    {
        // Arrange
        var operations = new List<Operation>
        {
            new Operation { EntityType = EntityType.Income, Amount = 100, Date = DateTime.Now },
            new Operation { EntityType = EntityType.Expense, Amount = 50, Date = DateTime.Now }
        };
        _financeManagerMock.Setup(m => m.GetAllOperations()).Returns(operations);

        // Act
        var result = _financeFacade.CalculateNetBalance(DateTime.MinValue, DateTime.MaxValue);

        // Assert
        Xunit.Assert.Equal(50, result); 
    }
}