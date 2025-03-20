using Domain.Entities;

namespace Domain.Patterns.Visitor;

public interface IVisitor
{
    void Visit(BankAccount account);
    void Visit(Operation operation);
    void Visit(Category category);
}