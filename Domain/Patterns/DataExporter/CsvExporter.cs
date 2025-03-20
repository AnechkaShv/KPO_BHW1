using System.Text;
using Domain.Patterns.Visitor;

namespace Domain.Patterns.DataExporter;

public class CsvExporter : DataExporter
{
    protected override IVisitor CreateVisitor()
    {
        return new Visitor.Visitor();
    }

    protected override string FormatData(IVisitor visitor)
    {
        var csvVisitor = visitor as Visitor.Visitor;
        if (csvVisitor == null)
        {
            throw new InvalidCastException("Data exporter is not a CSV format");
        }
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("BankAccounts:");
        sb.AppendLine("Id,Name,Balance");
        foreach (var account in csvVisitor.BankAccounts)
        {
            sb.AppendLine($"{account.Id},{account.Name},{account.Balance}");
        }
        sb.AppendLine();

        sb.AppendLine("Categories: ");
        sb.AppendLine("Id,Type,Name");
        foreach (var category in csvVisitor.Categories)
        {
            sb.AppendLine($"{category.Id},{category.EntityType},{category.Name}");
        }
        sb.AppendLine();

        sb.AppendLine("Operations:");
        sb.AppendLine("Id,Type,BankAccountId,Amount,Date,Description,CategoryId");
        foreach (var op in csvVisitor.Operations)
        {
            sb.AppendLine($"{op.Id},{op.EntityType},{op.BankId},{op.Amount},{op.Date:yyyy-MM-dd},{op.Description},{op.Category.Id}");
        }

        return sb.ToString();
    }
}