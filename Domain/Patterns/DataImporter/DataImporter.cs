using Domain.Entities;
using Domain.Patterns.Facades;

namespace Domain.Patterns.DataImporter;

public abstract class DataImporter
{
    public void Import(string path, FinanceFacade financeFacade)
    {
        if (!IsExists(path)) return;
        string content = File.ReadAllText(path);
        var records = ParseData(content);
        ProcessData(records, financeFacade);
    }
    
    private bool IsExists(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("File path is empty.");
            return false;
        }
    
        var directory = System.IO.Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Console.WriteLine("Directory does not exist.");
            return false;
        }

        return true;
    }
    
    protected abstract List<Dictionary<string, string>> ParseData(string content);
    
    protected virtual void ProcessData(List<Dictionary<string, string>> records, FinanceFacade facade)
    {
        foreach (var record in records)
        {
            try
            {
                string typeStr = record["Type"];
                var opType = typeStr.ToLower() == "income" ? EntityType.Income : EntityType.Expense;
                
                string accountName = record["AccountName"];
                var account = facade.GetBankAccounts().Find(a => a.Name == accountName);
                if (account == null)
                {
                    account = facade.CreateBankAccount(accountName);
                }
                
                decimal amount = decimal.Parse(record["Amount"]);
                DateTime date = DateTime.Parse(record["Date"]);
                string description = record.ContainsKey("Description") ? record["Description"] : "";
                
                string categoryName = record["CategoryName"];
                string categoryTypeStr = record["CategoryType"];
                var catType = categoryTypeStr.ToLower() == "income" ? EntityType.Income : EntityType.Expense;
                
                var category = facade.GetCategories().Find(c => c.Name == categoryName && c.EntityType == catType);
                if (category == null)
                {
                    category = facade.CreateCategory(catType, categoryName);
                }
                
                facade.CreateOperation(opType, account.Id, amount, description, category);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}