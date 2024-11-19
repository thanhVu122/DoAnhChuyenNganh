using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Vfurniture.Models;
using Vfurniture.Models.ViewModels;

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
        public IActionResult Login(string returnURL)
        {
            return View(new LoginViewModel { ReturnURL = returnURL });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager
                    .PasswordSignInAsync(loginViewModel.TaiKhoan, loginViewModel.MatKhau, false, false);
                if (result.Succeeded)
                {
                    return Redirect(loginViewModel.ReturnURL ?? "/");
                }
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
            }
            return View(loginViewModel);
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
                    PhoneNumber=user.SoDienThoai,
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, user.MatKhau);
                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(newUser, "User");
                    TempData["success"] = "Tạo tài khoản thành công";
                    return Redirect("/account/Login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
        public async Task<IActionResult> Logout(string returnURL = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnURL);

        }

    }
}
