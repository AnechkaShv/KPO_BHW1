using Domain.Patterns.Visitor;

namespace Domain.Entities;

/// <summary>
/// Represents a financial operation (e.g., income or expense).
/// Implements the <see cref="IVisitable"/> interface to support the Visitor pattern.
/// </summary>
public class Operation : IVisitable
{
    private Guid _id; // Unique identifier for the operation
    private EntityType _entityType; // Type of the operation (Income or Expense)
    private Guid _bank_account_id; // ID of the bank account associated with the operation
    private decimal _amount; // Amount of the operation
    private DateTime _date; // Date of the operation
    private string _description; // Description of the operation
    private Category _category_id; // Category associated with the operation

    /// <summary>
    /// Default constructor.
    /// </summary>
    public Operation()
    {
    }

    /// <summary>
    /// Constructor to create a new financial operation.
    /// </summary>
    /// <param name="entityType">The type of the operation (Income or Expense).</param>
    /// <param name="bank_account_id">The ID of the bank account associated with the operation.</param>
    /// <param name="amount">The amount of the operation. Must be non-negative.</param>
    /// <param name="date">The date of the operation.</param>
    /// <param name="description">The description of the operation.</param>
    /// <param name="category_id">The category associated with the operation.</param>
    /// <exception cref="ArgumentException">Thrown if the amount is negative.</exception>
    public Operation(EntityType entityType, Guid bank_account_id, decimal amount, DateTime date, string description, Category category_id)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }
        _id = Guid.NewGuid(); // Generate a unique identifier
        _entityType = entityType;
        _bank_account_id = bank_account_id;
        _amount = amount;
        _date = date;
        _description = description;
        _category_id = category_id;
    }

    /// <summary>
    /// ID of the bank account associated with the operation.
    /// </summary>
    public Guid BankId { get => _bank_account_id; private set => _bank_account_id = value; }

    /// <summary>
    /// Unique identifier of the operation.
    /// </summary>
    public Guid Id { get => _id; private set => _id = value; }

    /// <summary>
    /// Amount of the operation.
    /// </summary>
    public decimal Amount { get => _amount; set => _amount = value; }

    /// <summary>
    /// Date of the operation.
    /// </summary>
    public DateTime Date { get => _date; set => _date = value; }

    /// <summary>
    /// Description of the operation.
    /// </summary>
    public string Description { get => _description; set => _description = value; }

    /// <summary>
    /// Category associated with the operation.
    /// </summary>
    public Category Category { get => _category_id; set => _category_id = value; }

    /// <summary>
    /// Type of the operation (Income or Expense).
    /// </summary>
    public EntityType EntityType { get => _entityType; set => _entityType = value; }

    /// <summary>
    /// Accepts a visitor (implementation of the Visitor pattern).
    /// </summary>
    /// <param name="visitor">The visitor instance that will process this object.</param>
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this); // Call the Visit method of the visitor
    }
}