using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DanhMucs danhMuc)
        {
            if (ModelState.IsValid)
            {
                // Check if a category with the same MaDanhMuc already exists
                var existingDanhMuc = _dataContext.DanhMucs.FirstOrDefault(d => d.MaDanhMuc == danhMuc.MaDanhMuc);

                if (existingDanhMuc != null)
                {
                    // If a category with the same MaDanhMuc exists, add an error to the model state
                    TempData["error"] = "Danh mục với mã này đã tồn tại.";
                    return RedirectToAction("Create");  // Return the view with the error message
                }

                // If no existing category, add the new one
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
                var existingNameDanhMuc = await _dataContext.DanhMucs
                    .FirstOrDefaultAsync(d => d.TenDanhMuc == danhMuc.TenDanhMuc && d.MaDanhMuc != danhMuc.MaDanhMuc);

                if (existingNameDanhMuc != null)
                {
                    TempData["error"] = "Cập nhật danh mục thất bại. Vì tên danh mục đã có.";
                    return RedirectToAction("Edit", new { id = danhMuc.MaDanhMuc });  // Ensure the ID is passed along
                }

                try
                {
                    _dataContext.Update(danhMuc);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Cập nhật danh mục thành công";
                    return RedirectToAction("Index");
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
                // Check if there are any products associated with this category
                var productsInCategory = await _dataContext.SanPhams
                                                          .Where(p => p.MaDanhMuc == danhMuc.MaDanhMuc)
                                                          .AnyAsync();

                if (productsInCategory)
                {
                    // If there are products in this category, show an error message and prevent deletion
                    TempData["error"] = "Không thể xóa danh mục này vì nó có sản phẩm.";
                    return RedirectToAction("Index"); // Redirect to the list page
                }

                // If no products are found, proceed with deletion
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
