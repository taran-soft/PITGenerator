using ClosedXML.Excel;
using TaranSoft.PITGenerator.Model;

namespace TaranSoft.PITGenerator.Readers;

public class ExcelReader
{
    public static List<InputRow> ReadExcelFile(string filePath)
    {
        var rows = new List<InputRow>();

        using (var workbook = new XLWorkbook(filePath))
        {
            var worksheet = workbook.Worksheet(1); // or use name: workbook.Worksheet("Sheet1")
            var rowsUsed = worksheet.RangeUsed().RowsUsed();

            foreach (var row in rowsUsed.Skip(1)) // Skip header
            {
                rows.Add(new InputRow
                {
                    // User_ID = row.Cell(1).GetValue<string>(),
                    // UTC_Time = row.Cell(2).GetValue<DateTime>(),
                    // Account = row.Cell(3).GetValue<string>(),
                    // Operation = row.Cell(4).GetValue<string>(),
                    // Coin = row.Cell(5).GetValue<string>(),
                    // Change = row.Cell(6).GetValue<decimal>(),
                    // Remark = row.Cell(7).GetValue<string>(),
                });
            }
        }

        return rows;
    }
}