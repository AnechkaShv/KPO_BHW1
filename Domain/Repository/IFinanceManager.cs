using Domain.Entities;

namespace Domain.Repository;

public interface IFinanceManager
{
    void AddBankAccount(BankAccount account);
    List<BankAccount> GetAllAccounts();
    void UpdateBankAccount(BankAccount account);
    void RemoveBankAccount(Guid accountId);
    
    void Deposit(Guid accountId, decimal amount, string description, Category categoryId);
    void Withdraw(Guid accountId, decimal amount, string description, Category categoryId);
    
    void AddCategory(Category category);
    List<Category> GetAllCategories();
    void UpdateCategory(Category category);
    void RemoveCategory(Category category);
    
    void AddOperation(Operation operation);
    List<Operation> GetAllOperations();
    void UpdateOperation(Operation operation);
    void RemoveOperation(Guid operationId);
}