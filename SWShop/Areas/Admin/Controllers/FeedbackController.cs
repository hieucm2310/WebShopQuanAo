using SWShop.DataAccess.Data;
using SWShop.DataAccess.Repository.IRepository;
using SWShop.Models;
using SWShop.Ultility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SWShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using SWShop.DataAccess.Repository;

namespace SWShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class FeedbackController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public FeedbackController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Rate> objRateList = _unitOfWork.Rate.GetAll(includeProperties: "Product,ApplicationUser").ToList();

            return Json(new { data = objRateList });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Rate.Get(u=>u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Lỗi" });
            }
            else
            {
                _unitOfWork.Rate.Remove(objFromDb);
                _unitOfWork.Save();
            }

            return Json(new { success = true, message = "Xóa thành công" });
        }
        #endregion
    }
}
