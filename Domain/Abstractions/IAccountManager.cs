using Domain.Entities;

namespace Domain.Abstractions;

public interface IAccountManager
{
    void AddBankAccount(BankAccount account);
    List<BankAccount> GetAllAccounts();
    void UpdateBankAccount(BankAccount account);
    void RemoveBankAccount(Guid accountId);
    
    void Deposit(Guid accountId, decimal amount, string description, Category categoryId);
    void Withdraw(Guid accountId, decimal amount, string description, Category categoryId);
    

    //decimal GetBalance(int accountId);
   // void AttachObserver(IObserver observer);
}