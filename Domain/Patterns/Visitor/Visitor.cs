using Domain.Entities;

namespace Domain.Patterns.Visitor;

public class Visitor : IVisitor
{
    public List<BankAccount> BankAccounts { get; } = new List<BankAccount>();
    public List<Category> Categories { get; } = new List<Category>();
    public List<Operation> Operations { get; } = new List<Operation>();
    
    public void Visit(BankAccount account)
    {
        BankAccounts.Add(account);
    }

    public void Visit(Operation operation)
    {
        Operations.Add(operation);
    }

    public void Visit(Category category)
    {
        Categories.Add(category);
    }
}