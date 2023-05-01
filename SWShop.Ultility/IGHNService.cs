using SWShop.Models;
using Microsoft.AspNetCore.Http;

namespace SWShop.Ultility;
public interface IGHNService
{
    Task<OrderResponseModel> OrderExecute(OrderInformationModel model);
}