using System.Text.Json;

namespace Domain.Patterns.DataImporter;

public class JsonImporter : DataImporter
{
    protected override List<Dictionary<string, string>> ParseData(string content)
    {
        var records = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(content);
        return records ?? new List<Dictionary<string, string>>();
    }
}