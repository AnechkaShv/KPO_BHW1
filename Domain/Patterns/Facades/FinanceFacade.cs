using Domain.Abstractions;
using Domain.Entities;
using Domain.Patterns.Factories;
using Domain.Patterns.Observer;
using Domain.Repository;

namespace Domain.Patterns.Facades;

/// <summary>
/// Represents a facade for managing financial operations, such as bank accounts, operations, and categories.
/// Provides a simplified interface for interacting with the finance system.
/// </summary>
public class FinanceFacade
{
    private readonly IFinanceManager _financeManager; // Manages financial data and operations
    private readonly IEnumerable<IAccountObserver> _observers; // Observers for account balance changes

    /// <summary>
    /// Initializes a new instance of the <see cref="FinanceFacade"/> class.
    /// </summary>
    /// <param name="financeManager">The finance manager to use for data operations.</param>
    /// <param name="observers">The observers to notify about account balance changes.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="financeManager"/> is null.</exception>
    public FinanceFacade(IFinanceManager? financeManager = null, IEnumerable<IAccountObserver>? observers = null)
    {
        _financeManager = financeManager ?? throw new ArgumentNullException(nameof(financeManager));
        _observers = observers ?? new List<IAccountObserver>();
    }

    /// <summary>
    /// Creates a new bank account with the specified name and balance.
    /// </summary>
    /// <param name="name">The name of the bank account.</param>
    /// <param name="balance">The initial balance of the bank account.</param>
    /// <returns>The created bank account.</returns>
    public BankAccount CreateBankAccount(string name, decimal balance = 0)
    {
        var account = Factory.CreateBankAccount(name, balance);

        // Attach observers to the account's balance change event
        foreach (var observer in _observers)
        {
            account.onBalanceChanged += observer.HandleBalanceChange;
        }

        // Add the account to the finance manager
        _financeManager.AddBankAccount(account);
        return account;
    }

    /// <summary>
    /// Retrieves all bank accounts.
    /// </summary>
    /// <returns>A list of all bank accounts.</returns>
    public List<BankAccount> GetBankAccounts()
    {
        return _financeManager.GetAllAccounts();
    }

    /// <summary>
    /// Creates a new financial operation (income or expense).
    /// </summary>
    /// <param name="entityType">The type of the operation (Income or Expense).</param>
    /// <param name="bankAccount">The ID of the bank account associated with the operation.</param>
    /// <param name="amount">The amount of the operation.</param>
    /// <param name="description">The description of the operation.</param>
    /// <param name="category">The category associated with the operation.</param>
    /// <returns>The created operation.</returns>
    public Operation CreateOperation(EntityType entityType, Guid bankAccount, decimal amount, string description, Category category)
    {
        var operation = Factory.CreateOperation(entityType, bankAccount, amount, description, category);

        // Add the operation to the finance manager
        _financeManager.AddOperation(operation);

        // Update the account balance based on the operation type
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

    /// <summary>
    /// Updates an existing financial operation.
    /// </summary>
    /// <param name="operation">The operation to update.</param>
    public void UpdateOperation(Operation operation)
    {
        _financeManager.UpdateOperation(operation);
    }

    /// <summary>
    /// Retrieves all financial operations.
    /// </summary>
    /// <returns>A list of all financial operations.</returns>
    public List<Operation> GetOperations()
    {
        return _financeManager.GetAllOperations();
    }

    /// <summary>
    /// Removes a financial operation.
    /// </summary>
    /// <param name="operation">The operation to remove.</param>
    public void RemoveOperation(Operation operation)
    {
        _financeManager.RemoveOperation(operation.Id);
    }

    /// <summary>
    /// Calculates the net balance (income minus expenses) for a specified time period.
    /// </summary>
    /// <param name="start">The start date of the period.</param>
    /// <param name="end">The end date of the period.</param>
    /// <returns>The net balance for the specified period.</returns>
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

    /// <summary>
    /// Recalculates the balance of a bank account based on its associated operations.
    /// </summary>
    /// <param name="account">The bank account to recalculate the balance for.</param>
    public void RecalculateBalance(BankAccount account)
    {
        decimal newBalance = _financeManager.GetAllOperations()
            .Where(o => o.BankId == account.Id)
            .Sum(o => o.EntityType == EntityType.Income ? o.Amount : -o.Amount);

        account.Balance = newBalance;
    }

    /// <summary>
    /// Creates a new category for financial operations.
    /// </summary>
    /// <param name="type">The type of the category (Income or Expense).</param>
    /// <param name="name">The name of the category.</param>
    /// <returns>The created category.</returns>
    public Category CreateCategory(EntityType type, string name)
    {
        Category category = Factory.CreateCategory(type, name);
        _financeManager.AddCategory(category);
        return category;
    }

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <returns>A list of all categories.</returns>
    public List<Category> GetCategories()
    {
        return _financeManager.GetAllCategories();
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="category">The category to update.</param>
    public void UpdateCategory(Category category)
    {
        _financeManager.UpdateCategory(category);
    }

    /// <summary>
    /// Removes a category.
    /// </summary>
    /// <param name="category">The category to remove.</param>
    public void RemoveCategory(Category category)
    {
        _financeManager.RemoveCategory(category);
    }
}