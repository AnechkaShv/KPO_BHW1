using Domain.Patterns.Facades;

namespace Domain.Patterns.Command;

/// <summary>
/// Represents a command to create a new bank account.
/// Implements the Command pattern to encapsulate the account creation logic.
/// </summary>
public class CreateAccountCommand
{
    private readonly FinanceFacade _financeFacade; // Facade to interact with the finance system
    private readonly string _name; // Name of the account to be created
    private readonly decimal _balance; // Initial balance of the account

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateAccountCommand"/> class.
    /// </summary>
    /// <param name="financeFacade">The facade used to interact with the finance system.</param>
    /// <param name="name">The name of the account to be created.</param>
    /// <param name="balance">The initial balance of the account.</param>
    public CreateAccountCommand(FinanceFacade financeFacade, string name, decimal balance)
    {
        _financeFacade = financeFacade ?? throw new ArgumentNullException(nameof(financeFacade));
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _balance = balance;
    }

    /// <summary>
    /// Executes the command to create a new bank account.
    /// </summary>
    public void Execute()
    {
        // Use the facade to create the bank account
        var account = _financeFacade.CreateBankAccount(_name, _balance);

        // Output the result to the console
        Console.WriteLine($"Created account: {account.Name}. Balance: {account.Balance}.");
    }
}