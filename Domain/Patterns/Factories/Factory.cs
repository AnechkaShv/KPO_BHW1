using Domain.Entities;

namespace Domain.Patterns.Factories;

public static class Factory
{
    public static BankAccount CreateBankAccount(string bankName, decimal balance)
    {
        if (balance < 0)
        {
            throw new ArgumentException("Balance cannot be negative");
        }
        return new BankAccount(bankName, balance);
    }

    public static Category CreateCategory(EntityType type, string categoryName)
    {
        return new Category(type, categoryName);
    }

    public static Operation CreateOperation(EntityType entityType, Guid bank_account_id, decimal amount, string description, Category category_id)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }
        return new Operation(entityType, bank_account_id, amount, DateTime.Now, description, category_id);
    }
}