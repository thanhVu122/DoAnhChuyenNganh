using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Vfurniture.Areas.Admin.Reponsitory;
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
        private readonly IEmailSender _emailSender;

        private UserManager<AppNguoiDung> _userManager;
        public AccountController(SignInManager<AppNguoiDung> signInManager, UserManager<AppNguoiDung> userManager,
            DataContext dataContext, IEmailSender emailSender)

        {
            _emailSender = emailSender;
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
            var user = await _dataContext.Users.FirstOrDefaultAsync(o => o.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatInfo(AppNguoiDung userModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound();
                }
                user.NgaySinh = userModel.NgaySinh;
                user.PhoneNumber = userModel.PhoneNumber;
                user.Email = userModel.Email;
                user.TenNguoiDung = userModel.TenNguoiDung;

                // Lưu thay đổi vào cơ sở dữ liệu
                _dataContext.Users.Update(user);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật thành công";
                return RedirectToAction("UpdatInfo");
            }

            return RedirectToAction("UpdatInfo");

        }
        public async Task<IActionResult> ForgetPass()
        {
            return View();
        }
        public async Task<IActionResult> SendEmailForgetPass(AppNguoiDung nguoiDung)
        {
            var checkMail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == nguoiDung.Email);
            if (checkMail == null)
            {
                TempData["error"] = "Email không tồn tại";
                return RedirectToAction("ForgetPass");
            }
            else
            {
                string token = Guid.NewGuid().ToString();
                checkMail.token = token;
                _dataContext.Update(checkMail);
                await _dataContext.SaveChangesAsync();
                var receiver = checkMail.Email;
                var subject = "Thay đổi mật khẩu của khách hàng" + checkMail.Email;
                var message = "Nhấn vào đây để thay đổi mật khẩu: " +
                  $"<a href='{Request.Scheme}://{Request.Host}/Account/NewPass?email={checkMail.Email}&token={token}'>Đổi mật khẩu</a>";

                await _emailSender.SendEmailAsync(receiver, subject, message);

            }
            TempData["success"] = "Đã gửi đường link bạn hãy kiểm tra email của mình";
            return RedirectToAction("ForgetPass");
        }

        public async Task<IActionResult> NewPass(AppNguoiDung user, string token)
        {
            var checkUser = await _userManager.Users
        .Where(u => u.Email == user.Email && u.token == token)
        .FirstOrDefaultAsync();
            if (checkUser != null)
            {
                ViewBag.Email = checkUser.Email;
                ViewBag.Token = token;
            }
            else
            {
                TempData["error"] = "Email không hợp lệ";
                return RedirectToAction("ForgetPass");
            }
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> UpdateNewPass(AppNguoiDung user, string token)
        {
            var checkUser = await _userManager.Users
                             .Where(u => u.Email == user.Email)
                             .Where(u => u.token == token)
                             .FirstOrDefaultAsync();
            if (checkUser != null)
            {
                string newToken = Guid.NewGuid().ToString();

                var passwordHasher = new PasswordHasher<AppNguoiDung>();
                var passwordHash = passwordHasher.HashPassword(checkUser, user.PasswordHash);
                checkUser.PasswordHash = passwordHash;
                checkUser.token = newToken;

                await _userManager.UpdateAsync(checkUser);
                TempData["success"] = "Cập nhật thành công";
                return RedirectToAction("Login");

            }
            else
            {
                TempData["error"] = "Cập nhật thất bại";
                return RedirectToAction("ForgetPass");
            }
            return View();
        }
    }
}
