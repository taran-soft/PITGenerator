namespace TaranSoft.PITGenerator.NbpAPIClient;

public class NbpRateResponse
{
    public string table { get; set; }
    public string currency { get; set; }
    public string code { get; set; }
    public List<NbpRate> rates { get; set; }
}