using SWShop.DataAccess.Data;
using SWShop.DataAccess.Repository.IRepository;
using SWShop.Models;
using SWShop.Models.ViewModels;
using SWShop.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Drawing.Printing;
using System.Security.Claims;

namespace SWShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var objProduct = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(objProduct);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "ProductImages");
                return View(productVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                _unitOfWork.Save();

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = productVM.Product.Id,
                        };

                        if (productVM.Product.ProductImages == null)
                        {
                            productVM.Product.ProductImages = new List<ProductImage>();
                        }
                        productVM.Product.ProductImages.Add(productImage);
                    }
                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();
                }


                TempData["success"] = "Product created/updated successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }

        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                                        Path.Combine(_webHostEnvironment.WebRootPath,
                                        imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.ProductImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();

                TempData["success"] = "Deleted successfully";
            }
            return RedirectToAction(nameof(Upsert), new {id= productId });
        }

        [HttpPost]
        public IActionResult AddSizeAmount(int productId, string sizeName, int amount)
        {
            if (!string.IsNullOrEmpty(sizeName))
            {
                var size = new Size
                {
                    ProductId = productId,
                    Name = sizeName,
                    Amount = amount
                };

                Size sizeFromDb = _unitOfWork.Size.Get(u => u.Name == sizeName && u.ProductId == productId);

                if (sizeFromDb != null)
                {
                    sizeFromDb.Amount += amount;
                    _unitOfWork.Size.Update(sizeFromDb);
                    _unitOfWork.Save();
                }
                else
                {
                    _unitOfWork.Size.Add(size);
                    _unitOfWork.Save();
                }
                TempData["success"] = "Size update successfully";

                return Ok();
            }
            else
            {
                return Ok();
            }
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            foreach (var product in objProductList)
            {
                product.QuantitySold = _unitOfWork.OrderDetail.GetAll(includeProperties: "OrderHeader")
                                .Where(od => od.ProductId == product.Id && od.OrderHeader.OrderStatus != SD.StatusPending && od.OrderHeader.PaymentStatus != SD.PaymentStatusPending)
                                .Sum(od => od.Count);

                product.QuantityRemain = _unitOfWork.Size.GetAll().Where(u=>u.ProductId == product.Id).Sum(x=>x.Amount);
                product.Sizes = null;
            }
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            //var oldImagePath = 
            //                    Path.Combine(_webHostEnvironment.WebRootPath,
            //                    productToBeDeleted.ImageUrl.TrimStart('\\'));

            //if (System.IO.File.Exists(oldImagePath))
            //{
            //    System.IO.File.Delete(oldImagePath);
            //}

            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }
                Directory.Delete(finalPath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
