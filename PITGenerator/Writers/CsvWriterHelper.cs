using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TaranSoft.PITGenerator.Model;

namespace TaranSoft.PITConverter.Writers;

public class CsvWriterHelper
{
    public static void WriteCsvFile(string filePath, List<OutputRow> data)
    {
        using var writer = new StreamWriter(filePath);
        using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        });

        csv.WriteHeader<OutputRow>();
        csv.NextRecord();
        csv.WriteRecords(data);
        
        csv.WriteRecord(new TotalRow{ Name = "Upcoming", Total = data.Where(x => x.AmountPLN > 0).Sum(x => x.AmountPLN)});
        csv.WriteRecord(new TotalRow{ Name = "Outcome", Total = data.Where(x => x.AmountPLN < 0).Sum(x => x.AmountPLN)});
    }
}