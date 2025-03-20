using Domain.Patterns.Facades;
using Domain.Patterns.Visitor;

namespace Domain.Patterns.DataExporter;

public abstract class DataExporter
{
    public void Export(FinanceFacade financeFacade, string path)
    {
        if (!IsExists(path))
        {
            return;
        }

        var visitor = CreateVisitor();

        foreach (var acc in financeFacade.GetBankAccounts())
        {
            acc.Accept(visitor);
        }
        foreach (var cat in financeFacade.GetCategories())
        {
            cat.Accept(visitor);
        }
        foreach (var op in financeFacade.GetOperations())
        {
            op.Accept(visitor);
        }
        
        string data = FormatData(visitor);
        File.WriteAllText(path, data);
        Console.WriteLine($"Exported to {path}");
    }

    protected abstract IVisitor CreateVisitor();
    private bool IsExists(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("Path is empty");
            return false;
        }
        
        var dir = System.IO.Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Console.WriteLine("Wrong file path");
            return false;
        }

        return true;
    }
    
    protected abstract string FormatData(IVisitor visitor);
}