using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vfurniture.Models;

namespace Vfurniture.Controllers
{
    public class AccountController : Controller
    {

        private SignInManager<AppNguoiDung> _signInManager;

        private UserManager<AppNguoiDung> _userManager;
        public AccountController(SignInManager<AppNguoiDung> signInManager, UserManager<AppNguoiDung> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        public IActionResult Sigister()
        {
            return View();
        }
        [HttpPost]
            public async Task<IActionResult> Sigister(NguoiDungModel user)
        {
            if (ModelState.IsValid)
            {
                AppNguoiDung newUser = new AppNguoiDung
                {
                    UserName = user.TaiKhoan,
                    Email = user.Email,
                    
                };

                IdentityResult result = await _userManager.CreateAsync(newUser);
                if (result.Succeeded) {
                    TempData["success"]
                    return RedirectToAction("/Admin/Home");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

    }
}
