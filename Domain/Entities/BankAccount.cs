using Domain.Patterns.Visitor;

namespace Domain.Entities;

public class BankAccount : IVisitable
{
    private Guid _id;
    private string _name;
    private decimal _balance;

    public EventHandler onBalanceChanged;

    public BankAccount(string name, decimal balance)
    {
        _id = Guid.NewGuid();
        _name = name;
        Balance = balance;
        
    }
    
    public Guid Id { get => _id; private set => _id = value; }
    public decimal Balance { 
        get => _balance;
        set
        {
            _balance = value;
            onBalanceChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public string Name { get => _name; set => _name = value; }
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}