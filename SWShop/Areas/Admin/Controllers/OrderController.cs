using SWShop.DataAccess.Repository.IRepository;
using SWShop.Models;
using SWShop.Models.ViewModels;
using SWShop.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace SWShop.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVnPayService _vnPayService;
        private readonly IGHNService _gHNService;
        private readonly HttpClient _httpClient;

        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork, IVnPayService vnPayService, IGHNService gHNService, IHttpClientFactory httpClientFactory)
        {
            _unitOfWork = unitOfWork;
            _vnPayService = vnPayService;
            _gHNService = gHNService;
            _httpClient = httpClientFactory.CreateClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int orderId)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View(OrderVM);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.Address = OrderVM.OrderHeader.Address;
            orderHeaderFromDb.Ward = OrderVM.OrderHeader.Ward;
            orderHeaderFromDb.District = OrderVM.OrderHeader.District;
            orderHeaderFromDb.Province = OrderVM.OrderHeader.Province;
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Order Details Updated Successfully";

            return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            OrderInformationModel orderModel = new OrderInformationModel();
            orderModel.ToAddress = OrderVM.OrderHeader.Address;
            orderModel.ToPhone = OrderVM.OrderHeader.PhoneNumber;
            orderModel.ToWard = OrderVM.OrderHeader.Ward;
            orderModel.ToDistrict = OrderVM.OrderHeader.District;
            orderModel.ToProvince = OrderVM.OrderHeader.Province;
            orderModel.OrderCode = OrderVM.OrderHeader.TrackingNumber;
            orderModel.CODAmount = (int)OrderVM.OrderHeader.OrderTotal;
            orderModel.PaymentType = OrderVM.OrderHeader.PaymentStatus == SD.PaymentStatusApproved ? 1 : 2;
            orderModel.ToName = OrderVM.OrderHeader.Name;

            OrderResponseModel createOrderGHN = _gHNService.OrderExecute(orderModel).GetAwaiter().GetResult();

            if (createOrderGHN.Code == "200")
            {
                var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
                orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
                orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
                orderHeader.OrderStatus = SD.StatusShipped;
                orderHeader.ShippingDate = DateTime.ParseExact(createOrderGHN.EDTime, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                _unitOfWork.OrderHeader.Update(orderHeader);
                _unitOfWork.Save();
                TempData["Success"] = "Tạo đơn ship thành công.";

            }
            else
            {
                TempData["Error"] = createOrderGHN.Message;
            }
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });

        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public async Task<IActionResult> CancelOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);

            if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                PaymentInformationModel paymentInformation = new PaymentInformationModel();
                paymentInformation.Id = orderHeader.Id;
                paymentInformation.Amount = orderHeader.OrderTotal;
                paymentInformation.Name = orderHeader.Name;
                paymentInformation.OrderId = orderHeader.PaymentIntentId;

                var uri = _vnPayService.CreateRefundUrl(paymentInformation, HttpContext);
                var response = await _httpClient.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    // Handle successful response
                    return Ok();
                }
                else
                {
                    // Handle unsuccessful response
                    return StatusCode((int)response.StatusCode);
                }
                //Redirect(url);
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled);
            }
            _unitOfWork.Save();
            TempData["Success"] = "Order Cancelled Successfully";
            //return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
            return Ok();
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> objOrderHeaders;
            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objOrderHeaders = _unitOfWork.OrderHeader
                    .GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser");
            }

            switch (status)
            {
                case "pending":
                    objOrderHeaders = objOrderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = objOrderHeaders });
        }
        #endregion
    }
}
