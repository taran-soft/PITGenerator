namespace TaranSoft.PITGenerator.Model;

public class InputRow
{
    public string User_ID { get; set; }
    public DateTime UTC_Time { get; set; }
    public string Account { get; set; }
    public string Operation { get; set; }
    public string Coin { get; set; }
    public decimal Change { get; set; }
    public string Remark { get; set; }
}