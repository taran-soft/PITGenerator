namespace TaranSoft.PITGenerator.Model;

public class OutputRow
{
    public DateTime OperationDate { get; set; }
    public string OperationType { get; set; }
    public string Currency { get; set; }
    public decimal CurrencyAmount { get; set; }
    public string FiatPrice { get; set; }
    public string UAHAmount { get; set; }
    public string NBP { get; set; }
    public decimal AmountPLN { get; set; }
}