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
using Microsoft.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SWShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var rates = _unitOfWork.Rate.GetAll(includeProperties: "ApplicationUser").OrderByDescending(u=>u.Date).Take(4);
            foreach (var rate in rates)
            {
                rate.TimeRate = FormatTimeElapsed(rate.Date);
            }
            HomeAdminVM homeAdminVM = new HomeAdminVM()
            {
                RateList = rates,
            };
            return View(homeAdminVM);
        }
        public static string FormatTimeElapsed(DateTime date)
        {
            DateTime now = DateTime.Now;
            TimeSpan timeElapsed = now - date;

            if (timeElapsed < TimeSpan.FromSeconds(60))
            {
                return "just now";
            }
            else if (timeElapsed < TimeSpan.FromMinutes(60))
            {
                int minutes = (int)timeElapsed.TotalMinutes;
                return $"{minutes} minute{(minutes != 1 ? "s" : "")} ago";
            }
            else if (timeElapsed < TimeSpan.FromDays(1))
            {
                int hours = (int)timeElapsed.TotalHours;
                return $"{hours} hour{(hours != 1 ? "s" : "")} ago";
            }
            else
            {
                int days = (int)timeElapsed.TotalDays;
                return $"{days} day{(days != 1 ? "s" : "")} ago";
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _unitOfWork.ApplicationUser.GetAll().ToList();

            foreach (var user in objUserList)
            {
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
            }

            return Json(new { data = objUserList });
        }

        [HttpPost]
        public IActionResult LockUnLock([FromBody] string id)
        {
            var objFromDb = _unitOfWork.ApplicationUser.Get(u=>u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _unitOfWork.ApplicationUser.Update(objFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Operation Successful" });
        }
        #endregion
    }
}
