using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vfurniture.Models;
using Vfurniture.Models.ViewModels;
using Vfurniture.Reponsitory;
using static NuGet.Packaging.PackagingConstants;

namespace Vfurniture.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _dataContext;
        private SignInManager<AppNguoiDung> _signInManager;

        private UserManager<AppNguoiDung> _userManager;
        public AccountController(SignInManager<AppNguoiDung> signInManager, UserManager<AppNguoiDung> userManager, DataContext dataContext)

        {
            _dataContext = dataContext;
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
                    TenNguoiDung = user.TenNguoiDung,
                    UserName = user.TaiKhoan,
                    Email = user.Email,
                    PhoneNumber = user.SoDienThoai,
                    NgaySinh = user.NgaySinh
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

        public async Task<IActionResult> HistoryOrder()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            // Lấy danh sách đơn hàng theo userId
            var orders = await _dataContext.DatHangs
                                .Include(o => o.ChiTietDonHangs) // Nếu cần thông tin chi tiết đơn hàng
                                .Where(o => o.TenNguoiDat == userEmail)
                                .OrderByDescending(o => o.NgayTao)
                                .ToListAsync();
            ViewBag.UserEmail = userEmail;
            return View(orders);
        }
        public async Task<IActionResult> OrderDetail(string id)
        {
            var order = await _dataContext.DatHangs
                            .Include(o => o.ChiTietDonHangs)
                            .ThenInclude(c => c.SanPhams)
                            .FirstOrDefaultAsync(o => o.MaDatHang == id);
            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng!";
                return RedirectToAction("HistoryOrder");
            }
            return View(order);
        }

        public async Task<IActionResult> Cancel(string orderCode)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var order = await _dataContext.DatHangs
                    .Where(o => o.MaDatHang == orderCode)
                    .FirstAsync();
                order.TrangThai = 2;
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("HistoryOrder");
        }


        [HttpGet]
        public async Task<IActionResult> UpdatInfo()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var user = await _userManager.Users.FirstOrDefaultAsync(o => o.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdattoInfo()
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound();
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                _dataContext.Users.Update(user);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật thành công";
                return RedirectToAction("UpdatInfo");
            }

            return RedirectToAction("UpdattoInfo");

        }
    }
}
