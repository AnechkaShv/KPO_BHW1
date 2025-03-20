namespace Domain.Patterns.DataImporter;

public class CsvImporter : DataImporter
{
    protected override List<Dictionary<string, string>> ParseData(string content)
    {
        var result = new List<Dictionary<string, string>>();
        var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2)
            return result;

        var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();

        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split(',').Select(v => v.Trim()).ToArray();
            if (values.Length != headers.Length)
                continue;

            var record = new Dictionary<string, string>();
            for (int j = 0; j < headers.Length; j++)
            {
                record[headers[j]] = values[j];
            }
            result.Add(record);
        }
        return result;
    }
}