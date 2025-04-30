using System.Text.Json;

namespace TaranSoft.PITGenerator.NbpAPIClient;

public class NbpApiClient
{
    private static readonly HttpClient httpClient = new HttpClient();

    public async Task<decimal?> GetUahRateAsync(DateTime date)
    {
        var formatedDate = date.ToString("yyyy-MM-dd");
        
        Console.WriteLine($"GetUahRateAsync fetching rate for date: {formatedDate}");
        
        var url = $"https://api.nbp.pl/api/exchangerates/rates/A/UAH/{formatedDate}?format=json";

        try
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<NbpRateResponse>(json);

            return result?.rates?[0].mid;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching rate: {ex.Message}");
            return null;
        }
    }
}