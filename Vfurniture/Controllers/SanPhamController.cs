using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;
using Vfurniture.Models.ViewModels;
using Vfurniture.Reponsitory;

namespace Vfurniture.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly DataContext _dataContext;
        public SanPhamController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Deltail(int id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            //Lay cac san pham
            var sanPham = await _dataContext.SanPhams
        .Include(sp => sp.DanhGiaList) // Load danh sách đánh giá
        .FirstOrDefaultAsync(sp => sp.MaSanPham == id);

            // Lấy danh sách đánh giá liên quan đến sản phẩm
            sanPham.DanhGiaList = sanPham.DanhGiaList
         .Where(dg => dg.TrangThai == 1)
         .ToList();

            //Các sản phẩm liên qua 
            var sanPhamLienQuan = await _dataContext.SanPhams
        .Where(p => p.MaDanhMuc == sanPham.MaDanhMuc && p.MaSanPham != sanPham.MaSanPham)
        .Take(3)
        .ToListAsync();

            ViewBag.SPLQ = sanPhamLienQuan;
            return View(sanPham);
        }
        public IActionResult TimKiem(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return View("DanhSach", _dataContext.SanPhams.ToList());
            }

            var sanPhams = _dataContext.SanPhams
                .Where(sp => sp.TenSanPham.Contains(keyword))
                .ToList();

            ViewData["Keyword"] = keyword;
            return View("DanhSach", sanPhams);
        }
        [HttpPost]
        public IActionResult ThemDanhGia(DanhGia danhGia)
        {
            if (ModelState.IsValid)
            {
                _dataContext.DanhGias.Add(danhGia);
                _dataContext.SaveChanges();
                TempData["Success"] = "Đánh giá của bạn đã được ghi nhận!";
            }
            else
            {
                TempData["Error"] = "Vui lòng kiểm tra thông tin và thử lại.";
            }

            return RedirectToAction("Deltail", new { id = danhGia.MaSanPham });
        }

    }
}
