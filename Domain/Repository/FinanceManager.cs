using Domain.Entities;

namespace Domain.Repository;

/// <summary>
/// Manages financial data, including bank accounts, categories, and operations.
/// Implements the <see cref="IFinanceManager"/> interface to provide core financial operations.
/// </summary>
public class FinanceManager : IFinanceManager
{
    private List<BankAccount> _accounts = new List<BankAccount>(); // List of bank accounts
    private readonly List<Category> _categories = new List<Category>(); // List of categories
    private readonly List<Operation> _operations = new List<Operation>(); // List of operations

    /// <summary>
    /// Adds a new bank account to the list of accounts.
    /// </summary>
    /// <param name="account">The bank account to add.</param>
    public void AddBankAccount(BankAccount account)
    {
        _accounts.Add(account);
    }

    /// <summary>
    /// Retrieves all bank accounts.
    /// </summary>
    /// <returns>A list of all bank accounts.</returns>
    public List<BankAccount> GetAllAccounts()
    {
        return _accounts;
    }

    /// <summary>
    /// Updates an existing bank account.
    /// </summary>
    /// <param name="account">The bank account to update.</param>
    /// <exception cref="ArgumentNullException">Thrown if the account is null.</exception>
    public void UpdateBankAccount(BankAccount account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account));
        }

        // Find the existing account by ID
        BankAccount oldAccount = _accounts.Find(a => a.Id == account.Id);
        if (oldAccount != null)
        {
            // Update the account properties
            oldAccount.Name = account.Name;
            oldAccount.Balance = account.Balance;
        }
    }

    /// <summary>
    /// Removes a bank account by its ID.
    /// </summary>
    /// <param name="accountId">The ID of the bank account to remove.</param>
    public void RemoveBankAccount(Guid accountId)
    {
        _accounts.RemoveAll(a => a.Id == accountId); // Remove the account
        GetAllOperations().RemoveAll(a => a.BankId == accountId); // Remove associated operations
    }

    /// <summary>
    /// Deposits an amount into a bank account.
    /// </summary>
    /// <param name="accountId">The ID of the bank account.</param>
    /// <param name="amount">The amount to deposit. Must be non-negative.</param>
    /// <param name="description">The description of the deposit.</param>
    /// <param name="categoryId">The category associated with the deposit.</param>
    /// <exception cref="ArgumentException">Thrown if the account is not found or the amount is negative.</exception>
    public void Deposit(Guid accountId, decimal amount, string description, Category categoryId)
    {
        var account = _accounts.FirstOrDefault(a => a.Id == accountId);
        if (account == null)
        {
            throw new ArgumentException("Account not found");
        }

        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }

        // Update the account balance and add the operation
        account.Balance += amount;
        AddOperation(new Operation(EntityType.Income, accountId, amount, DateTime.Now, description, categoryId));
    }

    /// <summary>
    /// Withdraws an amount from a bank account.
    /// </summary>
    /// <param name="accountId">The ID of the bank account.</param>
    /// <param name="amount">The amount to withdraw. Must be non-negative.</param>
    /// <param name="description">The description of the withdrawal.</param>
    /// <param name="categoryId">The category associated with the withdrawal.</param>
    /// <exception cref="ArgumentException">Thrown if the account is not found, the amount is negative, or the balance is insufficient.</exception>
    public void Withdraw(Guid accountId, decimal amount, string description, Category categoryId)
    {
        var account = _accounts.FirstOrDefault(a => a.Id == accountId);
        if (account == null)
        {
            throw new ArgumentException("Account not found");
        }

        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }

        if (account.Balance < amount)
        {
            throw new ArgumentException("Insufficient balance");
        }

        // Update the account balance and add the operation
        account.Balance -= amount;
        AddOperation(new Operation(EntityType.Expense, accountId, amount, DateTime.Now, description, categoryId));
    }

    /// <summary>
    /// Adds a new category to the list of categories.
    /// </summary>
    /// <param name="category">The category to add.</param>
    public void AddCategory(Category category)
    {
        _categories.Add(category);
    }

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <returns>A list of all categories.</returns>
    public List<Category> GetAllCategories()
    {
        return _categories;
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="category">The category to update.</param>
    /// <exception cref="ArgumentNullException">Thrown if the category is null.</exception>
    public void UpdateCategory(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        // Find the existing category by ID
        Category oldCategory = _categories.Find(cat => cat.Id == category.Id);
        if (oldCategory != null)
        {
            // Update the category properties
            oldCategory.EntityType = category.EntityType;
            oldCategory.Name = category.Name;
        }

        // Update all operations associated with this category
        var operations = _operations.Where(o => o.Category.Id == category.Id).ToList();
        foreach (var operation in operations)
        {
            UpdateOperation(operation);
        }
    }

    /// <summary>
    /// Removes a category by its ID.
    /// </summary>
    /// <param name="category">The category to remove.</param>
    public void RemoveCategory(Category category)
    {
        _categories.RemoveAll(c => c.Id == category.Id); // Remove the category
        _operations.RemoveAll(o => o.Category.Id == category.Id); // Remove associated operations
    }

    /// <summary>
    /// Adds a new operation to the list of operations.
    /// </summary>
    /// <param name="operation">The operation to add.</param>
    public void AddOperation(Operation operation)
    {
        _operations.Add(operation);
    }

    /// <summary>
    /// Retrieves all operations.
    /// </summary>
    /// <returns>A list of all operations.</returns>
    public List<Operation> GetAllOperations()
    {
        return _operations;
    }

    /// <summary>
    /// Updates an existing operation.
    /// </summary>
    /// <param name="operation">The operation to update.</param>
    /// <exception cref="ArgumentNullException">Thrown if the operation is null.</exception>
    public void UpdateOperation(Operation operation)
    {
        if (operation == null)
        {
            throw new ArgumentNullException(nameof(operation));
        }

        // Find the existing operation by ID
        Operation? op = _operations.FirstOrDefault(o => o.Id == operation.Id);
        if (op != null)
        {
            // Update the operation properties
            op.Amount = operation.Amount;
            op.Date = operation.Date;
            op.Description = operation.Description;
            op.Category = operation.Category;

            // Update the account balance based on the operation type
            if (op.EntityType == EntityType.Income)
            {
                Deposit(op.BankId, op.Amount, $"Added {op.Amount} on account", op.Category);
            }
            else if (op.EntityType == EntityType.Expense)
            {
                Withdraw(op.BankId, op.Amount, $"Removed {op.Amount} on account", op.Category);
            }
        }
    }

    /// <summary>
    /// Removes an operation by its ID.
    /// </summary>
    /// <param name="operationId">The ID of the operation to remove.</param>
    public void RemoveOperation(Guid operationId)
    {
        var op = _operations.FirstOrDefault(o => o.Id == operationId);
        if (op != null)
        {
            // Update the account balance based on the operation type
            if (op.EntityType == EntityType.Income)
            {
                Deposit(op.BankId, op.Amount, $"Removed {op.Amount} on account", op.Category);
            }
            else if (op.EntityType == EntityType.Expense)
            {
                Withdraw(op.BankId, op.Amount, $"Removed {op.Amount} on account", op.Category);
            }

            _operations.Remove(op); // Remove the operation
        }
    }
}