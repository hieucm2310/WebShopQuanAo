using SWShop.DataAccess.Repository;
using SWShop.DataAccess.Repository.IRepository;
using SWShop.Models;
using SWShop.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using SWShop.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace SWShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if (claimsIdentity.IsAuthenticated)
            {
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
                HttpContext.Session.SetInt32(SD.SessionLike,
                        _unitOfWork.Like.GetAll(u => u.ApplicationUserId == userId && u.IsLike == true).Count());
            }

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages,Sizes,Likes").OrderByDescending(u => u.Id).Take(8);
            foreach (var product in productList)
            {
                product.QuantityRemain = product.Sizes.Sum(x => x.Amount);
                var rates = _unitOfWork.Rate.GetAll(u => u.ProductId == product.Id);
                int star = 0;
                if (rates.Count() > 0)
                {
                    star = (int)Math.Ceiling(rates.Average(r => r.Star));
                    product.Star = star;
                }
                if (claimsIdentity.IsAuthenticated)
                {
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (product.Likes.Where(l => l.ApplicationUserId == userId && l.IsLike).Count() > 0)
                    {
                        product.IsLike = true;
                    }
                }

            }

            var hotTrend = _unitOfWork.OrderDetail.GetAll(includeProperties: "OrderHeader,Product")
                            .Where(w => (w.OrderHeader.PaymentStatus.Equals(SD.PaymentStatusApproved) || w.OrderHeader.PaymentStatus.Equals(SD.PaymentStatusDelayedPayment))
                                && (w.OrderHeader.OrderStatus.Equals(SD.StatusApproved) || w.OrderHeader.OrderStatus.Equals(SD.StatusShipped)))
                            .Where(w => (DateTime.Now - w.OrderHeader.OrderDate).Days <= 14)
                            .GroupBy(od => od.ProductId)
                            .Select(g => new { Product = g.FirstOrDefault()?.Product, Count = g.Sum(x => x.Count) })
                            .OrderByDescending(pc => pc.Count)
                            .Take(3).Select(x => x.Product);
            foreach (var product in hotTrend)
            {
                product.QuantityRemain = product.Sizes.Sum(x => x.Amount);
                var rates = _unitOfWork.Rate.GetAll(u => u.ProductId == product.Id);
                int star = 0;
                if (rates.Count() > 0)
                {
                    star = (int)Math.Ceiling(rates.Average(r => r.Star));
                    product.Star = star;
                }
                if (claimsIdentity.IsAuthenticated)
                {
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (product.Likes.Where(l => l.ApplicationUserId == userId && l.IsLike).Count() > 0)
                    {
                        product.IsLike = true;
                    }
                }

            }
            var bestSeller = _unitOfWork.OrderDetail.GetAll(includeProperties: "OrderHeader,Product")
                         .Where(w => (w.OrderHeader.PaymentStatus.Equals(SD.PaymentStatusApproved) || w.OrderHeader.PaymentStatus.Equals(SD.PaymentStatusDelayedPayment))
                             && (w.OrderHeader.OrderStatus.Equals(SD.StatusApproved) || w.OrderHeader.OrderStatus.Equals(SD.StatusShipped)))
                         .GroupBy(od => od.ProductId)
                         .Select(g => new { Product = g.FirstOrDefault()?.Product, Count = g.Sum(x => x.Count) })
                         .OrderByDescending(pc => pc.Count)
                         .Take(3).Select(x => x.Product);
            foreach (var product in bestSeller)
            {
                product.QuantityRemain = product.Sizes.Sum(x => x.Amount);
                var rates = _unitOfWork.Rate.GetAll(u => u.ProductId == product.Id);
                int star = 0;
                if (rates.Count() > 0)
                {
                    star = (int)Math.Ceiling(rates.Average(r => r.Star));
                    product.Star = star;
                }
                if (claimsIdentity.IsAuthenticated)
                {
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (product.Likes.Where(l => l.ApplicationUserId == userId && l.IsLike).Count() > 0)
                    {
                        product.IsLike = true;
                    }
                }

            }
            var superSale = _unitOfWork.Product.GetAll()
                         .Where(u => u.SalePrice != 0)
                         .Select(p => new
                         {
                             Product = p,
                             Difference = p.ListPrice - p.SalePrice
                         })
                         .OrderByDescending(x => x.Difference)
                         .Take(3)
                         .Select(x => x.Product)
                         .ToList();
            foreach (var product in superSale)
            {
                product.QuantityRemain = product.Sizes.Sum(x => x.Amount);
                var rates = _unitOfWork.Rate.GetAll(u => u.ProductId == product.Id);
                int star = 0;
                if (rates.Count() > 0)
                {
                    star = (int)Math.Ceiling(rates.Average(r => r.Star));
                    product.Star = star;
                }
                if (claimsIdentity.IsAuthenticated)
                {
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (product.Likes.Where(l => l.ApplicationUserId == userId && l.IsLike).Count() > 0)
                    {
                        product.IsLike = true;
                    }
                }

            }
            HomeVM homeVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                ProductList = productList,
                HotTrend = hotTrend,
                BestSeller = bestSeller,
                SuperSale = superSale
            };

            return View(homeVM);
        }
        public IActionResult Shop()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if (claimsIdentity.IsAuthenticated)
            {
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");
            return View(productList);
        }
        public IActionResult Details(int productId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var cateId = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category,ProductImages").CategoryId;
            var productList = _unitOfWork.Product.GetAll(u => u.CategoryId == cateId && u.Id != productId, includeProperties: "Category,ProductImages,Sizes,Likes").Take(4);
            var rates = _unitOfWork.Rate.GetAll(u => u.ProductId == productId, includeProperties: "ApplicationUser");
            int star = 0;
            if (rates.Count() > 0)
            {
                star = (int)Math.Ceiling(rates.Average(r => r.Star));
            }

            foreach (var product in productList)
            {
                product.QuantityRemain = product.Sizes.Sum(x => x.Amount);

                var ratesB = _unitOfWork.Rate.GetAll(u => u.ProductId == product.Id, includeProperties: "ApplicationUser");
                var starB = 0;
                if (ratesB.Count() > 0)
                {
                    starB = (int)Math.Ceiling(ratesB.Average(r => r.Star));
                    product.Star = starB;
                }
                if (claimsIdentity.IsAuthenticated)
                {
                    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    if (product.Likes.Where(l => l.ApplicationUserId == userId && l.IsLike).Count() > 0)
                    {
                        product.IsLike = true;
                    }
                }

            }
            var productDe = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category,ProductImages,Sizes");
            productDe.Star = star;
            productDe.RateCount = rates.Count();
            ShoppingCart cart = new()
            {
                Product = productDe,
                Count = 1,
                ProductId = productId,
                Products = productList,
                Rates = rates,
            };

            return View(cart);
        }

        public IActionResult GetAmount(int productId, int sizeId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var productSize = _unitOfWork.Size.Get(u => u.Id == sizeId && u.ProductId == productId);
            var cartsize = _unitOfWork.ShoppingCart.GetAll(u => u.ProductId == productId && u.SizeNo == sizeId, includeProperties: "ApplicationUser")
                            .FirstOrDefault(u => u.ApplicationUserId == userId);
            int x = 0;
            if (productSize != null)
            {
                if (cartsize != null)
                {
                    x = productSize.Amount - cartsize.Count;
                }
                else
                {
                    x = productSize.Amount;
                }
            }
            return Content("{\"x\":" + x + "}", "application/json");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(int productId, int sizeNo, int count)
        {
            if (sizeNo != 0)
            {

                var shoppingCart = new ShoppingCart
                {
                    ProductId = productId,
                    SizeNo = sizeNo,
                    Count = count
                };
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                shoppingCart.ApplicationUserId = userId;

                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
                u.ProductId == shoppingCart.ProductId && u.SizeNo == shoppingCart.SizeNo);

                if (cartFromDb != null)
                {
                    cartFromDb.Count += shoppingCart.Count;
                    _unitOfWork.ShoppingCart.Update(cartFromDb);
                    _unitOfWork.Save();
                    HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
                }
                else
                {
                    _unitOfWork.ShoppingCart.Add(shoppingCart);
                    _unitOfWork.Save();
                    HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
                }
                TempData["success"] = "Cập nhật giỏ hàng thành công";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddRating(int productId, string comment, int starNum, IFormFile? file)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var rating = new Rate
            {
                ProductId = productId,
                Comment = comment,
                Star = starNum,
                ApplicationUserId = userId
            };

            _unitOfWork.Rate.Add(rating);
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fbPath = @"images\feedbacks";
                string finalPath = Path.Combine(wwwRootPath, fbPath);

                if (!Directory.Exists(finalPath))
                {
                    Directory.CreateDirectory(finalPath);
                }

                using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                rating.ImageUrl = @"\" + fbPath + @"\" + fileName;

            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Details), new { productId = productId });
        }

        [HttpPost]
        public IActionResult DeleteRate(int? rateId)
        {
            Rate? obj = _unitOfWork.Rate.Get(c => c.Id == rateId);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Rate.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Delete rate successfully!";
            return Ok();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Like(int productId, bool isLike)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            Like like = _unitOfWork.Like.Get(u => u.ProductId == productId && u.ApplicationUserId == userId);
            if (like != null)
            {
                like.IsLike = isLike;
                _unitOfWork.Like.Update(like);
                _unitOfWork.Save();
                if (isLike)
                {
                    TempData["success"] = "Yêu thích thành công!";
                }
            }
            else
            {
                like = new Like();
                like.IsLike = isLike;
                like.ProductId = productId;
                like.ApplicationUserId = userId;
                _unitOfWork.Like.Add(like);
                _unitOfWork.Save();
                TempData["success"] = "Yêu thích thành công!";
            }
            HttpContext.Session.SetInt32(SD.SessionLike,
                        _unitOfWork.Like.GetAll(u => u.ApplicationUserId == userId && u.IsLike == true).Count());

            
            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}