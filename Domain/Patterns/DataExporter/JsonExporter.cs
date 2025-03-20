using System.Text.Json;
using Domain.Patterns.Visitor;

namespace Domain.Patterns.DataExporter;

/// <summary>
/// Represents a JSON data exporter that formats data into JSON format.
/// Inherits from <see cref="DataExporter"/> to implement the Template Method pattern.
/// </summary>
public class JsonExporter : DataExporter
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
    /// Formats the collected data into JSON format.
    /// </summary>
    /// <param name="visitor">The visitor containing the collected data.</param>
    /// <returns>A string representing the data in JSON format.</returns>
    /// <exception cref="InvalidCastException">Thrown if the visitor is not of the expected type.</exception>
    protected override string FormatData(IVisitor visitor)
    {
        // Cast the visitor to the expected type
        var jsonVisitor = visitor as Visitor.Visitor;
        if (jsonVisitor == null)
        {
            throw new InvalidCastException("Data exporter is not a JSON visitor");
        }

        // Create an anonymous object to structure the data for JSON serialization
        var exportData = new
        {
            BankAccounts = jsonVisitor.BankAccounts,
            Categories = jsonVisitor.Categories,
            Operations = jsonVisitor.Operations
        };

        // Serialize the data to JSON with indentation and relaxed JSON escaping
        return JsonSerializer.Serialize(exportData, new JsonSerializerOptions
        {
            WriteIndented = true, // Format JSON with indentation for readability
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Allow special characters
        });
    }
}