using SWShop.Models;
using Microsoft.AspNetCore.Http;

namespace SWShop.Ultility;
public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    string CreateRefundUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}