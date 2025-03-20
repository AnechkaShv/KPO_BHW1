using Domain.Abstractions;
using Domain.Entities;
using Domain.Patterns.Factories;
using Domain.Patterns.Observer;
using Domain.Repository;

namespace Domain.Patterns.Facades;

public class FinanceFacade
{
    private IFinanceManager _financeManager = new FinanceManager();
    private IEnumerable<IAccountObserver> observers = new List<IAccountObserver>();
    

    public BankAccount CreateBankAccount(string name, decimal balance = 0)
    {
        var account = Factory.CreateBankAccount(name, balance);
        foreach (var observer in observers)
        {
            account.onBalanceChanged += observer.HandleBalanceChange;
        }
        _financeManager.AddBankAccount(account);
        return account;
    }

    public List<BankAccount> GetBankAccounts()
    {
        return _financeManager.GetAllAccounts();
    }

    public Operation CreateOperation(EntityType entityType, Guid bankAccount, decimal amount, string description, Category category)
    {
        var operation = Factory.CreateOperation(entityType, bankAccount, amount, description, category);
        _financeManager.AddOperation(operation);
        if (entityType == EntityType.Income)
        {
            _financeManager.Deposit(bankAccount, amount, description, category);
        }
        else
        {
            _financeManager.Withdraw(bankAccount, amount, description, category);
        }
        return operation;
    }

    public void UpdateOperation(Operation operation)
    {
        _financeManager.UpdateOperation(operation);
    }

    public List<Operation> GetOperations()
    {
        return _financeManager.GetAllOperations();
    }

    public void RemoveOperation(Operation operation)
    {
        _financeManager.RemoveOperation(operation.Id);
    }
    
    public decimal CalculateNetBalance(DateTime start, DateTime end)
    {
        var operations = _financeManager.GetAllOperations()
            .Where(o => o.Date >= start && o.Date <= end);
        decimal income = operations
            .Where(o => o.EntityType == EntityType.Income)
            .Sum(o => o.Amount);
        decimal expense = operations
            .Where(o => o.EntityType == EntityType.Expense)
            .Sum(o => o.Amount);
        return income - expense;
    }
    
    public void RecalculateBalance(BankAccount account)
    {
        decimal newBalance = _financeManager.GetAllOperations()
            .Where(o => o.BankId == account.Id)
            .Sum(o => o.EntityType == EntityType.Income ? o.Amount : -o.Amount);
        account.Balance = newBalance;
    }
    
    public Category CreateCategory(EntityType type, string name)
    {
        Category category = Factories.Factory.CreateCategory(type, name);
        _financeManager.AddCategory(category);
        return category;
    }

    public List<Category> GetCategories()
    {
        return _financeManager.GetAllCategories();
    }

    public void UpdateCategory(Category category)
    {
        _financeManager.UpdateCategory(category);
    }

    public void RemoveCategory(Category category)
    {
        _financeManager.RemoveCategory(category);
    }
}