namespace Domain.Patterns.DataImporter;

/// <summary>
/// Represents a CSV data importer that parses CSV content into a list of dictionaries.
/// Inherits from <see cref="DataImporter"/> to implement the Template Method pattern.
/// </summary>
public class CsvImporter : DataImporter
{
    /// <summary>
    /// Parses the CSV content into a list of dictionaries, where each dictionary represents a row.
    /// </summary>
    /// <param name="content">The CSV content to parse.</param>
    /// <returns>A list of dictionaries, where each dictionary contains key-value pairs representing a row.</returns>
    protected override List<Dictionary<string, string>> ParseData(string content)
    {
        // Initialize the result list
        var result = new List<Dictionary<string, string>>();

        // Split the content into lines
        var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        // If there are fewer than 2 lines (header + at least one row), return an empty list
        if (lines.Length < 2)
            return result;

        // Extract headers from the first line
        var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();

        // Process each line (row) after the header
        for (int i = 1; i < lines.Length; i++)
        {
            // Split the line into values
            var values = lines[i].Split(',').Select(v => v.Trim()).ToArray();

            // Skip rows where the number of values does not match the number of headers
            if (values.Length != headers.Length)
                continue;

            // Create a dictionary to represent the row
            var record = new Dictionary<string, string>();

            // Map each header to its corresponding value
            for (int j = 0; j < headers.Length; j++)
            {
                record[headers[j]] = values[j];
            }

            // Add the row to the result list
            result.Add(record);
        }

        // Return the parsed data
        return result;
    }
}