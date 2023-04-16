using Demo.Models;
using Microsoft.AspNetCore.Http;

namespace Demo.Ultility;
public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    string CreateRefundUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}