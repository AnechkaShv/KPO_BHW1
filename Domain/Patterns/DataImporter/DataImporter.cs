using Domain.Entities;
using Domain.Patterns.Facades;

namespace Domain.Patterns.DataImporter;

/// <summary>
/// Represents an abstract base class for importing data from files.
/// Implements the Template Method pattern to define the import process.
/// </summary>
public abstract class DataImporter
{
    /// <summary>
    /// Imports data from the specified file path and processes it using the provided <see cref="FinanceFacade"/>.
    /// </summary>
    /// <param name="path">The file path from which to import data.</param>
    /// <param name="financeFacade">The facade used to process the imported data.</param>
    public void Import(string path, FinanceFacade financeFacade)
    {
        // Check if the file exists and is valid
        if (!IsExists(path)) return;

        // Read the file content
        string content = File.ReadAllText(path);

        // Parse the content into a list of dictionaries
        var records = ParseData(content);

        // Process the parsed data
        ProcessData(records, financeFacade);
    }

    /// <summary>
    /// Checks if the specified file path is valid and accessible.
    /// </summary>
    /// <param name="filePath">The file path to validate.</param>
    /// <returns>True if the file path is valid; otherwise, false.</returns>
    private bool IsExists(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            Console.WriteLine("File path is empty.");
            return false;
        }

        // Check if the directory exists
        var directory = System.IO.Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Console.WriteLine("Directory does not exist.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Parses the file content into a list of dictionaries, where each dictionary represents a record.
    /// </summary>
    /// <param name="content">The file content to parse.</param>
    /// <returns>A list of dictionaries representing the parsed records.</returns>
    protected abstract List<Dictionary<string, string>> ParseData(string content);

    /// <summary>
    /// Processes the parsed records and updates the finance system using the provided <see cref="FinanceFacade"/>.
    /// </summary>
    /// <param name="records">The list of parsed records.</param>
    /// <param name="facade">The facade used to update the finance system.</param>
    protected virtual void ProcessData(List<Dictionary<string, string>> records, FinanceFacade facade)
    {
        foreach (var record in records)
        {
            try
            {
                // Extract and parse the operation type
                string typeStr = record["Type"];
                var opType = typeStr.ToLower() == "income" ? EntityType.Income : EntityType.Expense;

                // Find or create the bank account
                string accountName = record["AccountName"];
                var account = facade.GetBankAccounts().Find(a => a.Name == accountName);
                if (account == null)
                {
                    account = facade.CreateBankAccount(accountName);
                }

                // Extract and parse the amount and date
                decimal amount = decimal.Parse(record["Amount"]);
                DateTime date = DateTime.Parse(record["Date"]);

                // Extract the description (if available)
                string description = record.ContainsKey("Description") ? record["Description"] : "";

                // Find or create the category
                string categoryName = record["CategoryName"];
                string categoryTypeStr = record["CategoryType"];
                var catType = categoryTypeStr.ToLower() == "income" ? EntityType.Income : EntityType.Expense;

                var category = facade.GetCategories().Find(c => c.Name == categoryName && c.EntityType == catType);
                if (category == null)
                {
                    category = facade.CreateCategory(catType, categoryName);
                }

                // Create the operation
                facade.CreateOperation(opType, account.Id, amount, description, category);
            }
            catch (Exception ex)
            {
                // Log any errors that occur during processing
                Console.WriteLine(ex.Message);
            }
        }
    }
}