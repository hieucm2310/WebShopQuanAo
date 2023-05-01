using SWShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SWShop.Ultility
{
    public class GHNService : IGHNService
    {
        public async Task<OrderResponseModel> OrderExecute(OrderInformationModel model)
        {
            var url = "https://dev-online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/create";
            var accessToken = "ac622677-de94-11ed-ab31-3eeb4194879e";
            var orderResponse = new OrderResponseModel();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("token", accessToken);

            var body = new
            {
                payment_type_id = model.PaymentType,
                note = "GHN",
                required_note = "CHOXEMHANGKHONGTHU",
                to_name = model.ToName,
                to_phone = model.ToPhone,
                to_address = model.ToAddress,
                to_ward_name = model.ToWard,
                to_district_name = model.ToDistrict,
                to_province_name = model.ToProvince,
                cod_amount = model.CODAmount,
                client_order_code = model.OrderCode,
                content = "Content",
                weight = 200,
                length = 1,
                width = 19,
                height = 10,
                insurance_value = 0,
                service_id = 53350,
                service_type_id = 5
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                orderResponse.Code = responseData.code;
                orderResponse.Message = responseData.message;
                orderResponse.EDTime = responseData.data.expected_delivery_time;
                orderResponse.OrderCode = responseData.data.order_code;
            }
            else
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
                if (responseData.ContainsKey("message"))
                {
                    orderResponse.Message = responseData["message"].ToString();
                }
                else
                {
                    orderResponse.Message = $"Error creating order: {response.Content.ReadAsStringAsync().Result}";
                }
            }

            return orderResponse;
        }

    }
}
