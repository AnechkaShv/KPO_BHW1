using Domain.Patterns.Facades;

namespace Domain.Patterns.Command;

public class CreateAccountCommand
{
    private readonly FinanceFacade _financeFacade;
    private readonly string _name;
    private readonly decimal _balance;

    public CreateAccountCommand(FinanceFacade financeFacade, string name, decimal balance)
    {
        _financeFacade = financeFacade;
        _name = name;
        _balance = balance;
    }

    public void Execute()
    {
        var account = _financeFacade.CreateBankAccount(_name, _balance);
        Console.WriteLine($"Created account: {account.Name}. Balance: {account.Balance}.");
    }
}