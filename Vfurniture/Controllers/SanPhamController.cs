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
            var SanPhamtuMaSp = _dataContext.SanPhams
      
                .Where(s => s.MaSanPham == id)
                .FirstOrDefault();
            // Lấy danh sách đánh giá liên quan đến sản phẩm
            var danhGiaList = await _dataContext.DanhGias
                .Where(dg => dg.MaSanPham == id)
                .ToListAsync();
            // Gán danh sách đánh giá vào thuộc tính của sản phẩm
            SanPhamtuMaSp.DanhGiaList = danhGiaList;
            //Các sản phẩm liên qua 
            var SanPhamLienQuan = await _dataContext.SanPhams.Where(p => p.MaDanhMuc == SanPhamtuMaSp.MaDanhMuc && p.MaSanPham != SanPhamtuMaSp.MaSanPham)
                .Take(3)
                .ToListAsync();

            ViewBag.SPLQ = SanPhamLienQuan;
            return View(SanPhamtuMaSp);
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
