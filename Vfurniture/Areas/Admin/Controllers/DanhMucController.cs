using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize (Roles ="admin")]

    public class DanhMucController : Controller
    {
        private readonly DataContext _dataContext;

        public DanhMucController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // Danh sách danh mục
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.DanhMucs.ToListAsync());
        }

        // Tạo danh mục mới (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Tạo danh mục mới (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMucs danhMuc)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Add(danhMuc);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm danh mục thành công";
                return RedirectToAction("Index");
            }
            return View(danhMuc);
        }

        // Chỉnh sửa danh mục (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _dataContext.DanhMucs.FindAsync(id);
            if (danhMuc == null)
            {
                return NotFound();
            }
            return View(danhMuc);
        }

        // Chỉnh sửa danh mục (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DanhMucs danhMuc)
        {
            if (id != danhMuc.MaDanhMuc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Update(danhMuc);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Cập nhật danh mục thành công";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanhMucExists(danhMuc.MaDanhMuc))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(danhMuc);
        }

        // Xóa danh mục (GET)
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var danhMuc = await _dataContext.DanhMucs
                .FirstOrDefaultAsync(m => m.MaDanhMuc == id);
            if (danhMuc == null)
            {
                return NotFound();
            }

            return View(danhMuc);
        }

        // Xóa danh mục (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var danhMuc = await _dataContext.DanhMucs.FindAsync(id);
            if (danhMuc != null)
            {
                _dataContext.DanhMucs.Remove(danhMuc);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Xóa danh mục thành công";
            }
            return RedirectToAction("Index");
        }

        private bool DanhMucExists(string id)
        {
            return _dataContext.DanhMucs.Any(e => e.MaDanhMuc == id);
        }
    }
}
