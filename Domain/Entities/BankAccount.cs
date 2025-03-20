using Domain.Patterns.Visitor;

namespace Domain.Entities;

/// <summary>
/// Represents a bank account.
/// Implements the <see cref="IVisitable"/> interface to support the Visitor pattern.
/// </summary>
public class BankAccount : IVisitable
{
    private Guid _id; // Unique identifier for the account
    private string _name; // Name of the account
    private decimal _balance; // Current balance of the account

    /// <summary>
    /// Event triggered when the account balance changes.
    /// </summary>
    public EventHandler onBalanceChanged;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public BankAccount()
    {
    }

    /// <summary>
    /// Constructor to create a new bank account.
    /// </summary>
    /// <param name="name">The name of the account.</param>
    /// <param name="balance">The initial balance of the account.</param>
    public BankAccount(string name, decimal balance)
    {
        _id = Guid.NewGuid(); // Generate a unique identifier
        _name = name;
        Balance = balance; // Set the initial balance
    }

    /// <summary>
    /// Unique identifier of the account.
    /// </summary>
    public Guid Id { get => _id; private set => _id = value; }

    /// <summary>
    /// Current balance of the account.
    /// Triggers the <see cref="onBalanceChanged"/> event when the balance is updated.
    /// </summary>
    public decimal Balance
    {
        get => _balance;
        set
        {
            _balance = value;
            // Trigger the event when the balance changes
            onBalanceChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Name of the account.
    /// </summary>
    public string Name { get => _name; set => _name = value; }

    /// <summary>
    /// Accepts a visitor (implementation of the Visitor pattern).
    /// </summary>
    /// <param name="visitor">The visitor instance that will process this object.</param>
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this); // Call the Visit method of the visitor
    }
}