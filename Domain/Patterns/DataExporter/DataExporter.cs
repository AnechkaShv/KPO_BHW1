using Domain.Patterns.Facades;
using Domain.Patterns.Visitor;

namespace Domain.Patterns.DataExporter;

/// <summary>
/// Represents an abstract base class for exporting data.
/// Implements the Template Method pattern to define the export process.
/// </summary>
public abstract class DataExporter
{
    /// <summary>
    /// Exports data from the provided <see cref="FinanceFacade"/> to the specified file path.
    /// </summary>
    /// <param name="financeFacade">The facade containing the data to export.</param>
    /// <param name="path">The file path where the data will be exported.</param>
    public void Export(FinanceFacade financeFacade, string path)
    {
        // Check if the file path is valid
        if (!IsExists(path))
        {
            return;
        }

        // Create a visitor to collect data
        var visitor = CreateVisitor();

        // Collect data from bank accounts
        foreach (var acc in financeFacade.GetBankAccounts())
        {
            acc.Accept(visitor);
        }

        // Collect data from categories
        foreach (var cat in financeFacade.GetCategories())
        {
            cat.Accept(visitor);
        }

        // Collect data from operations
        foreach (var op in financeFacade.GetOperations())
        {
            op.Accept(visitor);
        }

        // Format the collected data
        string data = FormatData(visitor);

        // Write the formatted data to the file
        File.WriteAllText(path, data);
        Console.WriteLine($"Exported to {path}");
    }

    /// <summary>
    /// Creates a visitor instance to collect data for export.
    /// </summary>
    /// <returns>An instance of <see cref="IVisitor"/>.</returns>
    protected abstract IVisitor CreateVisitor();

    /// <summary>
    /// Checks if the provided file path is valid.
    /// </summary>
    /// <param name="path">The file path to validate.</param>
    /// <returns>True if the path is valid; otherwise, false.</returns>
    private bool IsExists(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("Path is empty");
            return false;
        }

        // Check if the directory exists
        var dir = System.IO.Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Console.WriteLine("Wrong file path");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Formats the collected data into the desired output format.
    /// </summary>
    /// <param name="visitor">The visitor containing the collected data.</param>
    /// <returns>A string representing the formatted data.</returns>
    protected abstract string FormatData(IVisitor visitor);
}