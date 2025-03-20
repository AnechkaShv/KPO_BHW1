using Domain.Patterns.Visitor;

namespace Domain.Entities;

/// <summary>
/// Represents a category for grouping financial operations (e.g., income or expense categories).
/// Implements the <see cref="IVisitable"/> interface to support the Visitor pattern.
/// </summary>
public class Category : IVisitable
{
    private Guid _id; // Unique identifier for the category
    private EntityType _entityType; // Type of the category (e.g., Income or Expense)
    private string _name; // Name of the category

    /// <summary>
    /// Default constructor.
    /// </summary>
    public Category()
    {
    }

    /// <summary>
    /// Constructor to create a new category.
    /// </summary>
    /// <param name="entityType">The type of the category (Income or Expense).</param>
    /// <param name="name">The name of the category.</param>
    public Category(EntityType entityType, string name)
    {
        _id = Guid.NewGuid(); // Generate a unique identifier
        _entityType = entityType;
        _name = name;
    }

    /// <summary>
    /// Unique identifier of the category.
    /// </summary>
    public Guid Id { get => _id; private set => _id = value; }

    /// <summary>
    /// Type of the category (Income or Expense).
    /// </summary>
    public EntityType EntityType { get => _entityType; set => _entityType = value; }

    /// <summary>
    /// Name of the category.
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