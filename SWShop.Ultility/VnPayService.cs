using SWShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace SWShop.Ultility
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"*{model.Id}*Khach hang {model.Name} thanh toan cho don hang Id: {model.Id}. So tien {model.Amount}**");
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", model.OrderId);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public string CreateRefundUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

            var pay = new VnPayLibrary();

            pay.AddRequestData("vnp_RequestId", Guid.NewGuid().ToString().Substring(0,10));
            pay.AddRequestData("vnp_Version", _configuration["Vnrefund:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnrefund:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnrefund:TmnCode"]);
            pay.AddRequestData("vnp_TransactionType", _configuration["Vnrefund:TransactionType"]);
            pay.AddRequestData("vnp_TransactionDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CreateBy", model.Name);
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_OrderInfo", $"*{model.Id}*Khach hang {model.Name} hoan tien cho don hang Id: {model.Id}. So tien {model.Amount}**");
            pay.AddRequestData("vnp_TxnRef", model.OrderId);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnrefund:BaseUrl"], _configuration["Vnrefund:HashSecret"]);

            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            return response;
        }
    }
}
