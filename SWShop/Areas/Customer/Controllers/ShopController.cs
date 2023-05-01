using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SWShop.DataAccess.Data;
using SWShop.DataAccess.Repository;
using SWShop.DataAccess.Repository.IRepository;
using SWShop.Models;
using SWShop.Models.ViewModels;
using SWShop.Ultility;
using System.Security.Claims;

namespace SWShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(int? category, string searchTerm = null, int pageNumber = 1, int pageSize = 9,  decimal? minPrice = null, decimal? maxPrice = null, string? size = null)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if (claimsIdentity.IsAuthenticated)
            {
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            var products = await _unitOfWork.Product.GetProductsAsync(searchTerm, pageNumber, pageSize, category, minPrice, maxPrice, size, includeProperties: "ProductImages,Likes,Sizes");
            foreach (var product in products)
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
            var totalCount = await _unitOfWork.Product.GetProductsCountAsync(searchTerm, pageNumber, pageSize, category, minPrice, maxPrice, size);

            ViewData["SearchTerm"] = searchTerm;
            ViewData["PageNumber"] = pageNumber;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalCount"] = totalCount;
            ViewData["Category"] = category;
            ViewData["Size"] = size;
            ViewData["MinPrice"] = minPrice;
            ViewData["MaxPrice"] = maxPrice;

            // Calculate pagination values
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var hasPreviousPage = pageNumber > 1;
            var hasNextPage = pageNumber < totalPages;

            // Set pagination values in ViewBag
            ViewBag.TotalPages = totalPages;
            ViewBag.HasPreviousPage = hasPreviousPage;
            ViewBag.HasNextPage = hasNextPage;
            ViewBag.PreviousPageNumber = hasPreviousPage ? pageNumber - 1 : 1;
            ViewBag.NextPageNumber = hasNextPage ? pageNumber + 1 : totalPages;

            HomeVM homeVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                SizeList = _unitOfWork.Size.GetAll().Select(s => s.Name).Distinct().ToList(),
                MinPrice = (int)_unitOfWork.Product.GetAll().Min(s => s.Price),
                MaxPrice = (int)_unitOfWork.Product.GetAll().Max(s => s.Price),
                ProductList = products
            };

            return View(homeVM);
        }

        public IActionResult Likes()
        {
            IEnumerable<Product> productList = new List<Product>();
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            if (claimsIdentity.IsAuthenticated)
            {
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
                HttpContext.Session.SetInt32(SD.SessionLike,
                        _unitOfWork.Like.GetAll(u => u.ApplicationUserId == userId && u.IsLike == true).Count());

                productList = _unitOfWork.Like.GetAll(includeProperties: "Product,Product.Sizes,Product.ProductImages").Where(u => u.ApplicationUserId == userId && u.IsLike == true)
                                                    .Select(l => l.Product)
                                                    .ToList();

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
                        userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                        if (product.Likes.Where(l => l.ApplicationUserId == userId && l.IsLike).Count() > 0)
                        {
                            product.IsLike = true;
                        }
                    }

                }
            }

            return View(productList);
        }
    }
}
