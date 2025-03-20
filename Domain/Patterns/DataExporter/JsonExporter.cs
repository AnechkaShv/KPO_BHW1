using System.Text.Json;
using Domain.Patterns.Visitor;

namespace Domain.Patterns.DataExporter;

public class JsonExporter : DataExporter
{
    protected override IVisitor CreateVisitor()
    {
        return new Visitor.Visitor();
    }

    protected override string FormatData(IVisitor visitor)
    {
        var jsonVisitor = visitor as Visitor.Visitor;
        if (jsonVisitor == null)
        {
            throw new InvalidCastException("Data exporter is not a JSON visitor");
        }
        var exportData = new
        {
            BankAccounts = jsonVisitor.BankAccounts,
            Categories = jsonVisitor.Categories,
            Operations = jsonVisitor.Operations
        };
        
        return JsonSerializer.Serialize(exportData, new JsonSerializerOptions 
        { 
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
        });
    }
}