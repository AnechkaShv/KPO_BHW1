using Domain.Patterns.Visitor;

namespace Domain.Entities;


public class Category : IVisitable
{
    private Guid _id; 
    private EntityType _entityType;
    private string _name;

    public Category(EntityType entityType, string name)
    {
        _id = Guid.NewGuid();
        _entityType = entityType;
        _name = name;
    }
    public Guid Id { get => _id; private set => _id = value; }
    public EntityType EntityType { get => _entityType; set => _entityType = value; }
    public string Name { get => _name; set => _name = value; }
    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}