using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/KhuyenMai")]
    [Authorize(Roles = "admin")]

    public class KhuyenMaiController : Controller
    {
        private readonly DataContext _dataContext;

        public KhuyenMaiController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var DanhSachKhuyenMai = await _dataContext.KhuyenMaiModels.ToListAsync();
            ViewBag.DSKM = DanhSachKhuyenMai;
            return View();
        }
        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhuyenMaiModel khuyenMaiModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Add(khuyenMaiModel);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Thêm khuyến mãi thành công.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Ghi log nếu cần
                    TempData["error"] = "Có lỗi xảy ra khi thêm khuyến mãi.";
                    ModelState.AddModelError("", ex.Message);
                }
         
            }
            return View(khuyenMaiModel);
        }
        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            // Tìm khuyến mãi dựa trên ID
            var khuyenMaiModel = await _dataContext.KhuyenMaiModels.FindAsync(id);

            if (khuyenMaiModel == null)
            {
                TempData["error"] = "Khuyến mãi không tồn tại.";
                return RedirectToAction("Index");
            }

            return View(khuyenMaiModel);
        }

        [HttpPost]
        [Route("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhuyenMaiModel khuyenMaiModel)
        {
            if (id != khuyenMaiModel.Id)
            {
                TempData["error"] = "ID không khớp.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Update(khuyenMaiModel);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Cập nhật khuyến mãi thành công.";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Ghi log nếu cần
                    TempData["error"] = "Có lỗi xảy ra khi cập nhật. Vui lòng thử lại.";
                    ModelState.AddModelError("", ex.Message);
                }
            }

            // Trả về view với model nếu có lỗi
            return View(khuyenMaiModel);
        }

    }
}
