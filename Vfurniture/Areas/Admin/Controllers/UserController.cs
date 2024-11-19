using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {

        private readonly UserManager<AppNguoiDung> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;
        public UserController(UserManager<AppNguoiDung> userManager, RoleManager<IdentityRole> roleManager, DataContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dataContext = context;
        }
        [HttpGet]

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách người dùng cùng với quyền
            var userWithRoles = await (from u in _dataContext.Users
                                       join ur in _dataContext.UserRoles on u.Id equals ur.UserId
                                       join r in _dataContext.Roles on ur.RoleId equals r.Id
                                       select new
                                       {
                                           User=u, RoleName=r.Name
                                       }).ToListAsync();

            // Truyền dữ liệu sang View
            return View(userWithRoles);
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(new AppNguoiDung());

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(AppNguoiDung user)
        {
            // Kiểm tra ModelState
            if (ModelState.IsValid)
            {
                var createUserResult = await _userManager.CreateAsync(user, user.PasswordHash);
                if (createUserResult.Succeeded)
                {
                    var createUser = await _userManager.FindByEmailAsync(user.Email);
                    var userID = createUser.Id;
                    var userRole = _roleManager.FindByIdAsync(user.RoleId);
                    var addToRoleResutl = await _userManager.AddToRoleAsync(createUser, userRole.Result.Name);
                    if (!addToRoleResutl.Succeeded)
                    {
                        foreach (var errors in createUserResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, errors.Description);
                        }
                    }
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Nếu có lỗi, load lại danh sách Roles vào ViewBag
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            return View(user); // Trả lại model để giữ giá trị đã nhập
        }
        [HttpGet]
        [Route("Delete/{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            // Tìm người dùng theo ID
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy người dùng.";
                return RedirectToAction("Index");
            }

            // Truyền dữ liệu người dùng đến View
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("DeleteConfirmed/{Id}")]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            // Tìm người dùng theo ID
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy người dùng.";
                return RedirectToAction("Index");
            }

            // Xóa người dùng
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Xóa người dùng thành công.";
            }
            else
            {
                TempData["Error"] = "Có lỗi khi xóa người dùng.";
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("Edit/{Id}")]
        public async Task<IActionResult> Edit(string Id)
        {
            // Tìm người dùng theo ID
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy người dùng.";
                return RedirectToAction("Index");
            }

            // Lấy danh sách quyền
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");

            // Truyền thông tin người dùng tới View
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit/{Id}")]
        public async Task<IActionResult> Edit(string Id, AppNguoiDung user, string RoleId)
        {
            // Tìm người dùng theo ID
            var existingUser = await _userManager.FindByIdAsync(Id);
            if (existingUser == null)
            {
                TempData["Error"] = "Không tìm thấy người dùng.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                // Cập nhật thông tin người dùng
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.RoleId = user.RoleId;
                // Lưu thay đổi
                var result = await _userManager.UpdateAsync(existingUser);

                if (result.Succeeded)
                {
                    // Cập nhật quyền nếu có
                    if (!string.IsNullOrEmpty(RoleId))
                    {
                        var roles = await _userManager.GetRolesAsync(existingUser);
                        await _userManager.RemoveFromRolesAsync(existingUser, roles);
                        var role = await _roleManager.FindByIdAsync(RoleId);
                        if (role != null)
                        {
                            await _userManager.AddToRoleAsync(existingUser, role.Name);
                        }
                    }

                    TempData["Success"] = "Cập nhật thông tin người dùng thành công.";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Nếu lỗi, hiển thị lại thông tin
            var rolesList = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(rolesList, "Id", "Name");

            return View(user);
        }

    }
}
