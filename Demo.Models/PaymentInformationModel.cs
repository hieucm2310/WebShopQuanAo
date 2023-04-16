namespace Demo.Models;

public class PaymentInformationModel
{
    public int Id { get; set; }
    public double Amount { get; set; }
    public string Name { get; set; }
    public string OrderId { get; set; }
}