using Domain.Abstractions;
using Domain.Entities;

namespace Domain.Patterns.Observer;

public class Balance : IAccountObserver
{
    public void HandleBalanceChange(object sender, EventArgs e)
    {
        if (sender is BankAccount bankAccount)
        {
            Console.WriteLine($"Balance of {bankAccount.Name} changed: {bankAccount.Balance}");
        }
    }
}