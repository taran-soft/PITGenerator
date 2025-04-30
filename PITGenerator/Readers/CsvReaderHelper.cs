using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TaranSoft.PITGenerator.Model;

namespace TaranSoft.PITGenerator.Readers;

public class CsvReaderHelper
{
    public static List<InputRow> ReadCsvFile(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            IgnoreBlankLines = true,
            TrimOptions = TrimOptions.Trim,
        });

        var records = csv.GetRecords<InputRow>();
        return new List<InputRow>(records);
    }
}