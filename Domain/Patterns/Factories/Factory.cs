using Domain.Entities;

namespace Domain.Patterns.Factories;

/// <summary>
/// Provides factory methods for creating domain entities such as bank accounts, categories, and operations.
/// Ensures that created entities are valid and consistent.
/// </summary>
public static class Factory
{
    /// <summary>
    /// Creates a new bank account with the specified name and balance.
    /// </summary>
    /// <param name="bankName">The name of the bank account.</param>
    /// <param name="balance">The initial balance of the bank account. Must be non-negative.</param>
    /// <returns>A new instance of <see cref="BankAccount"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if the balance is negative.</exception>
    public static BankAccount CreateBankAccount(string bankName, decimal balance)
    {
        if (balance < 0)
        {
            throw new ArgumentException("Balance cannot be negative");
        }
        return new BankAccount(bankName, balance);
    }

    /// <summary>
    /// Creates a new category with the specified type and name.
    /// </summary>
    /// <param name="type">The type of the category (Income or Expense).</param>
    /// <param name="categoryName">The name of the category.</param>
    /// <returns>A new instance of <see cref="Category"/>.</returns>
    public static Category CreateCategory(EntityType type, string categoryName)
    {
        return new Category(type, categoryName);
    }

    /// <summary>
    /// Creates a new financial operation with the specified details.
    /// </summary>
    /// <param name="entityType">The type of the operation (Income or Expense).</param>
    /// <param name="bank_account_id">The ID of the bank account associated with the operation.</param>
    /// <param name="amount">The amount of the operation. Must be non-negative.</param>
    /// <param name="description">The description of the operation.</param>
    /// <param name="category_id">The category associated with the operation.</param>
    /// <returns>A new instance of <see cref="Operation"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if the amount is negative.</exception>
    public static Operation CreateOperation(EntityType entityType, Guid bank_account_id, decimal amount, string description, Category category_id)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }
        return new Operation(entityType, bank_account_id, amount, DateTime.Now, description, category_id);
    }
}