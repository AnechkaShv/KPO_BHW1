using Domain.Patterns.Visitor;

namespace Domain.Entities;

public class Operation : IVisitable
{
    private Guid _id;
    private EntityType _entityType;
    private Guid _bank_account_id;
    private decimal _amount;
    private DateTime _date;
    private string _description;
    private Category _category_id;

    public Operation() {
    }

    public Operation(EntityType entityType, Guid bank_account_id, decimal amount, DateTime date, string description, Category category_id)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }
        _id = Guid.NewGuid();
        _entityType = entityType;
        _bank_account_id = bank_account_id;
        _amount = amount;
        _date = date;
        _description = description;
        _category_id = category_id;
    }
    
    public Guid BankId { get => _bank_account_id; private set => _bank_account_id = value; }
    public Guid Id { get => _id; private set => _id = value; }
    public decimal Amount { get => _amount; set => _amount = value; }
    public DateTime Date { get => _date; set => _date = value; }
    public string Description { get => _description; set => _description = value; }
    public Category Category { get => _category_id; set => _category_id = value; }
    public EntityType EntityType { get => _entityType; set => _entityType = value; }
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}