namespace SWShop.Models;

public class OrderInformationModel
{
    public string ToName { get; set; }
    public string ToPhone { get; set; }
    public string ToAddress { get; set; }
    public string ToWard { get; set; }
    public string ToDistrict { get; set; }
    public string ToProvince { get; set; }
    public int PaymentType { get; set; }
    public int CODAmount { get; set; }
    public string OrderCode { get; set; }
}