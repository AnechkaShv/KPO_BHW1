using System.Text;
using Domain.Patterns.Visitor;

namespace Domain.Patterns.DataExporter;

/// <summary>
/// Represents a CSV data exporter that formats data into CSV format.
/// Inherits from <see cref="DataExporter"/> to implement the Template Method pattern.
/// </summary>
public class CsvExporter : DataExporter
{
    /// <summary>
    /// Creates a visitor instance to collect data for export.
    /// </summary>
    /// <returns>An instance of <see cref="IVisitor"/>.</returns>
    protected override IVisitor CreateVisitor()
    {
        return new Visitor.Visitor();
    }

    /// <summary>
    /// Formats the collected data into CSV format.
    /// </summary>
    /// <param name="visitor">The visitor containing the collected data.</param>
    /// <returns>A string representing the data in CSV format.</returns>
    /// <exception cref="InvalidCastException">Thrown if the visitor is not of the expected type.</exception>
    protected override string FormatData(IVisitor visitor)
    {
        // Cast the visitor to the expected type
        var csvVisitor = visitor as Visitor.Visitor;
        if (csvVisitor == null)
        {
            throw new InvalidCastException("Data exporter is not a CSV format");
        }

        // Use StringBuilder to efficiently build the CSV content
        StringBuilder sb = new StringBuilder();

        // Export BankAccounts
        sb.AppendLine("BankAccounts:");
        sb.AppendLine("Id,Name,Balance");
        foreach (var account in csvVisitor.BankAccounts)
        {
            sb.AppendLine($"{account.Id},{account.Name},{account.Balance}");
        }
        sb.AppendLine();

        // Export Categories
        sb.AppendLine("Categories: ");
        sb.AppendLine("Id,Type,Name");
        foreach (var category in csvVisitor.Categories)
        {
            sb.AppendLine($"{category.Id},{category.EntityType},{category.Name}");
        }
        sb.AppendLine();

        // Export Operations
        sb.AppendLine("Operations:");
        sb.AppendLine("Id,Type,BankAccountId,Amount,Date,Description,CategoryId");
        foreach (var op in csvVisitor.Operations)
        {
            sb.AppendLine($"{op.Id},{op.EntityType},{op.BankId},{op.Amount},{op.Date:yyyy-MM-dd},{op.Description},{op.Category.Id}");
        }

        // Return the formatted CSV data
        return sb.ToString();
    }
}