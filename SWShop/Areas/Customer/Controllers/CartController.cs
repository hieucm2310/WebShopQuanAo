﻿using Azure;
using SWShop.DataAccess.Repository.IRepository;
using SWShop.Models;
using SWShop.Models.ViewModels;
using SWShop.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SWShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVnPayService _vnPayService;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IVnPayService vnPayService)
        {
            _unitOfWork = unitOfWork;
            _vnPayService = vnPayService;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };

            IEnumerable<ProductImage> productImages = _unitOfWork.ProductImage.GetAll();

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Product.ProductImages = productImages.Where(u => u.ProductId == cart.Product.Id).ToList();
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }

            return View(ShoppingCartVM);
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            //ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            //ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }

            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }

            if (ShoppingCartVM.OrderHeader.PaymentMethod.Equals(SD.VNPay))
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            if (ShoppingCartVM.OrderHeader.PaymentMethod.Equals(SD.VNPay))
            {
                PaymentInformationModel paymentInformation = new PaymentInformationModel();
                paymentInformation.Id = ShoppingCartVM.OrderHeader.Id;
                paymentInformation.Amount = ShoppingCartVM.OrderHeader.OrderTotal;
                paymentInformation.Name = ShoppingCartVM.OrderHeader.Name;
                var orderId = DateTime.Now.Ticks.ToString();
                paymentInformation.OrderId = orderId;


                var url = _vnPayService.CreatePaymentUrl(paymentInformation, HttpContext);
                _unitOfWork.OrderHeader.UpdateVNPayID(ShoppingCartVM.OrderHeader.Id, orderId, "");
                _unitOfWork.Save();
                return Redirect(url);
            }
            else
            {
                TempData["orderId"] = ShoppingCartVM.OrderHeader.Id;
                return RedirectToAction(nameof(OrderConfirmation));
            }


            //Response.Headers.Add("Location", url);
            //return new StatusCodeResult(303);

            //return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        }
        public IActionResult OrderConfirmation()
        {
            if (Request.Query.Count == 12)
            {
                var response = _vnPayService.PaymentExecute(Request.Query);

                if (response.VnPayResponseCode.Equals("00"))
                {
                    string orderDes = response.OrderDescription;
                    int id = Convert.ToInt16(orderDes.Split('*')[1]);
                    OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
                    if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
                    {
                        var paymentStatus = true;
                        if (paymentStatus)
                        {
                            _unitOfWork.OrderHeader.UpdateVNPayID(id, response.OrderId, response.TransactionId);
                            _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                        }
                    }

                    List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
                        .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
                    foreach (var item in shoppingCarts)
                    {
                        int count = item.Count;
                        int productId = item.ProductId;
                        int sizeId = item.SizeNo;
                        var productSize = _unitOfWork.Size
                        .Get(u => u.Id == sizeId && u.ProductId == productId);
                        productSize.Amount -= count;
                        _unitOfWork.Size.Update(productSize);
                    }
                    _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
                    _unitOfWork.Save();
                    HttpContext.Session.Clear();
                    ViewBag.OrderConfirmStatus = true;
                    ViewBag.OrderConfirmDetail = orderDes;
                }
                else
                {
                    ViewBag.OrderConfirmStatus = false;
                }
            }
            else
            {
                if (TempData["orderId"] != null)
                {
                    int id = (int)TempData["orderId"];
                    OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
                    if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
                    {
                        var paymentStatus = true;
                        if (paymentStatus)
                        {
                            _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusDelayedPayment);
                        }
                    }

                    List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
                        .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
                    foreach (var item in shoppingCarts)
                    {
                        int count = item.Count;
                        int productId = item.ProductId;
                        int sizeId = item.SizeNo;
                        var productSize = _unitOfWork.Size
                        .Get(u => u.Id == sizeId && u.ProductId == productId);
                        productSize.Amount -= count;
                        _unitOfWork.Size.Update(productSize);
                    }
                    _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
                    _unitOfWork.Save();
                    HttpContext.Session.Clear();
                    ViewBag.OrderConfirmStatus = true;
                    ViewBag.OrderConfirmDetail = "Đặt hàng thành công";
                }

            }


            return View();
        }
        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
                HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            HttpContext.Session.SetInt32(SD.SessionCart,
                    _unitOfWork.ShoppingCart
                    .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        private decimal GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Product.SalePrice == 0)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                return shoppingCart.Product.SalePrice;
            }
        }
    }
}
