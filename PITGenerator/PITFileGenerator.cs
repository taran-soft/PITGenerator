using TaranSoft.PITGenerator.Model;
using TaranSoft.PITGenerator.NbpAPIClient;
using TaranSoft.PITGenerator.Readers;

namespace TaranSoft.PITGenerator;

public class PITFileGenerator
{
    public async Task<IEnumerable<OutputRow>> Generate(string filePath)
    {
        var result = new List<OutputRow>();
        
        Console.WriteLine("Converting file: " + filePath);
        
        var rows = CsvReaderHelper.ReadCsvFile(filePath);

        var filteredRows = rows
            .Where(x => x.Operation == "P2P Trading")
            .OrderBy(x => x.UTC_Time);

        var reportStartDate = new DateTime(2024, 10, 15);
        var buyingRowsOld = filteredRows.Where(x => x.Change > 0 && x.UTC_Time <= reportStartDate).OrderByDescending(x => x.UTC_Time).ToList();
        var buyingRowsNew = filteredRows.Where(x => x.Change > 0 && x.UTC_Time > reportStartDate).OrderBy(x => x.UTC_Time).ToList();
        var sellingRows = filteredRows.Where(x => x.Change < 0 && x.UTC_Time > reportStartDate).ToList();

        decimal balance = 0;
        
        foreach (var row in sellingRows)
        {
            balance += row.Change;

            while (balance < 0)
            {
                var buyRow = buyingRowsNew.FirstOrDefault(x => x.UTC_Time < row.UTC_Time);
                if (buyRow != null)
                {
                    result.Add(await CreateBuyRow(buyRow));
                    buyingRowsNew.Remove(buyRow);
                    
                    balance += buyRow.Change;
                }
                else
                {
                    var buyRowOld = buyingRowsOld.FirstOrDefault(x => x.UTC_Time < row.UTC_Time);
                    result.Add(await CreateBuyRow(buyRowOld));
                    buyingRowsOld.Remove(buyRowOld);
                    
                    balance += buyRowOld.Change;
                }
            }
            
            result.Add(CreateSellRow(row));

        }

        return result.OrderBy(x => x.OperationDate).ToList();
    }

    private static OutputRow CreateSellRow(InputRow row)
    {
        var rowChange = -(row.Change * 4.0m);
        
        return new OutputRow
        {
            OperationDate = row.UTC_Time,
            OperationType = "Sell",
            Currency = row.Coin,
            CurrencyAmount = row.Change,
            FiatPrice = $"{rowChange} PLN",
            UAHAmount = "-",
            NBP = "-",
            AmountPLN = rowChange,
        };
    }

    private async Task<OutputRow> CreateBuyRow(InputRow row)
    {
        var nbp = await GetNBPRate(row.UTC_Time) ?? 0.096m;
        var uahAmount = row.Change * 41.5m;
        
        return new OutputRow
        {
            OperationDate = row.UTC_Time,
            OperationType = "Buy",
            Currency = row.Coin,
            CurrencyAmount = row.Change,
            FiatPrice = $"{uahAmount} UAH",
            UAHAmount = uahAmount.ToString(),
            NBP = nbp.ToString(),
            AmountPLN = -(nbp * uahAmount)
        };
    }

    private async Task<decimal?> GetNBPRate(DateTime date)
    {
        var retryCount = 0;
        decimal? uahRate = 0.0096m;
        
        while (retryCount < 5)
        {
            try
            {
                var rateDate = date.AddDays(-retryCount);
                uahRate = await new NbpApiClient().GetUahRateAsync(rateDate);
                if (uahRate == null)
                {
                    retryCount++;
                }
                else
                {
                    retryCount = 5;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        return uahRate;
    }
}