using Domain.Entities;

namespace Domain.Repository;

public class FinanceManager : IFinanceManager
{
    private List<BankAccount> _accounts = new List<BankAccount>();
    private readonly List<Category> _categories = new List<Category>();
    private readonly List<Operation> _operations = new List<Operation>();
    
    public void AddBankAccount(BankAccount account)
    {
        _accounts.Add(account);
    }

    public List<BankAccount> GetAllAccounts()
    {
        return _accounts;
    }

    public void UpdateBankAccount(BankAccount account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account));
        }
        BankAccount oldAccount = _accounts.Find(account => account.Id == account.Id);
        if (oldAccount != null)
        {
            oldAccount.Name = account.Name;
            oldAccount.Balance = account.Balance;
        }
        
    }

    public void RemoveBankAccount(Guid accountId)
    {
        _accounts.RemoveAll(a => a.Id == accountId);
        GetAllOperations().RemoveAll(a => a.BankId  == accountId);
    }

    public void Deposit(Guid accountId, decimal amount, string description, Category categoryId)
    {
        var account = _accounts.FirstOrDefault(a=> a.Id == accountId);
        if (account == null)
        {
            throw new ArgumentException("Account not found");
        }

        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }
        
        account.Balance += amount;
        AddOperation(new Operation(EntityType.Income, accountId, amount, DateTime.Now, description, categoryId));
    }

    public void Withdraw(Guid accountId, decimal amount, string description, Category categoryId)
    {
        var account = _accounts.FirstOrDefault(a=> a.Id == accountId);
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
        
        account.Balance -= amount;
        AddOperation(new Operation(EntityType.Expense, accountId, amount, DateTime.Now, description, categoryId));
    }
    
    public void AddCategory(Category category)
    {
        _categories.Add(category);
        
    }

    public List<Category> GetAllCategories()
    {
        return _categories;
    }

    public void UpdateCategory(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }
        Category oldCategory = _categories.Find(cat => cat.Id == category.Id);
        if (oldCategory != null)
        {
            oldCategory.EntityType = category.EntityType;
            oldCategory.Name = category.Name;
        }

        var operations = _operations.Where(o => o.Category.Id == category.Id).ToList();
        foreach (var operation in operations)
        {
            UpdateOperation(operation);
        }
    }

    public void RemoveCategory(Category category)
    {
        _categories.RemoveAll(c => c.Id == category.Id);
        _operations.RemoveAll(o => o.Category.Id == category.Id); 
    }
    
    public void AddOperation(Operation operation)
    {
        _operations.Add(operation);
    }

    public List<Operation> GetAllOperations()
    {
        return _operations;
    }

    public void UpdateOperation(Operation operation)
    {
        if (operation == null)
        {
            throw new ArgumentNullException(nameof(operation));
        }
        Operation? op = _operations.FirstOrDefault(o => o.Id == operation.Id);
        if (op != null)
        {
            op.Amount = operation.Amount;
            op.Date = operation.Date;
            op.Description = operation.Description;
            op.Category = operation.Category;
            
            
        }
        if (op.EntityType == EntityType.Income)
        {
            Deposit(op.BankId, op.Amount, $"Added {op.Amount} on account", op.Category);
        }
        else if (op.EntityType == EntityType.Expense)
        {
            Withdraw(op.BankId, op.Amount, $"Removed {op.Amount} on account", op.Category);

        }
    }

    public void RemoveOperation(Guid operationId)
    {
        var op = _operations.FirstOrDefault(o => o.Id == operationId);
        if (op != null)
        {
            if (op.EntityType == EntityType.Income)
            {
                Deposit(op.BankId, op.Amount, $"Removed {op.Amount} on account", op.Category);
            }
            else if (op.EntityType == EntityType.Expense)
            {
                Withdraw(op.BankId, op.Amount, $"Removed {op.Amount} on account", op.Category);
            }

            _operations.Remove(op);
        }
    }

}