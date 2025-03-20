using Domain.Entities;

namespace Domain.Abstractions;

public interface ICategoryManager
{
    void AddCategory(Category category);
    List<Category> GetAllCategories();
    void UpdateCategory(Category category);
    void RemoveCategory(Category category);
}