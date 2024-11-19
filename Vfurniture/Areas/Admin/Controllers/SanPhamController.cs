using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class SanPhamController : Controller
    {

        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SanPhamController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var sanPhams = await _dataContext.SanPhams
                .Include(p => p.DanhMuc)
                .ToListAsync();

            return View(sanPhams);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //ViewBag.sp = new SelectList(_dataContext.SanPhams, "MaSanPham", "TenSanPham");
            ViewBag.dm = new SelectList(_dataContext.DanhMucs, "MaDanhMuc", "TenDanhMuc");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanPhams sanPhams)
        {
            ViewBag.dm = new SelectList(_dataContext.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPhams.MaDanhMuc);

            if (ModelState.IsValid)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sanpham");

                // Xử lý hình ảnh chính
                if (sanPhams.imagesLoad != null && sanPhams.imagesLoad.Length > 0)
                {
                    string imageName = Guid.NewGuid().ToString() + "_" + sanPhams.imagesLoad.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await sanPhams.imagesLoad.CopyToAsync(fs);
                    }

                    sanPhams.HinhAnh = imageName; // Lưu tên ảnh chính vào database
                }

                // Xử lý danh sách hình ảnh
                if (sanPhams.imagesLoadList != null && sanPhams.imagesLoadList.Count > 0)
                {
                    List<string> danhSachHinhAnh = new List<string>();

                    foreach (var file in sanPhams.imagesLoadList)
                    {
                        string imageName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        string filePath = Path.Combine(uploadsDir, imageName);

                        using (var fs = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                        }

                        danhSachHinhAnh.Add(imageName); // Lưu tên các hình ảnh vào danh sách
                    }

                    sanPhams.DanhSachHinhAnh = string.Join(",", danhSachHinhAnh); // Gộp tên hình ảnh thành chuỗi
                }

                // Lưu sản phẩm vào database
                _dataContext.Add(sanPhams);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }

            // Xử lý lỗi nếu dữ liệu không hợp lệ
            TempData["ThatBai"] = "Không hợp lệ";
            foreach (var modelState in ModelState)
            {
                foreach (var error in modelState.Value.Errors)
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
            }

            return View(sanPhams);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(long Id)
        {
            var move = await _dataContext.SanPhams.FirstOrDefaultAsync(x => x.MaSanPham == Id);
            return View(move);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCf(long Id)
        {
            var move = await _dataContext.SanPhams.FindAsync(Id);
            if (move != null)
            {
                _dataContext.Remove(move);
            }
            await _dataContext.SaveChangesAsync();
            TempData["Success"] = "Sản phẩm đã được xóa thành công.";
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var sanPham = await _dataContext.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }

            ViewBag.dm = new SelectList(_dataContext.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
            return View(sanPham);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SanPhams sanPham)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.dm = new SelectList(_dataContext.DanhMucs, "MaDanhMuc", "TenDanhMuc", sanPham.MaDanhMuc);
                return View(sanPham);
            }

            var sanPhamDb = await _dataContext.SanPhams.FindAsync(sanPham.MaSanPham);
            if (sanPhamDb == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin sản phẩm
            sanPhamDb.TenSanPham = sanPham.TenSanPham;
            sanPhamDb.MaDanhMuc = sanPham.MaDanhMuc;
            sanPhamDb.Gia = sanPham.Gia;
            sanPhamDb.Discount = sanPham.Discount;
            sanPhamDb.KichThuoc = sanPham.KichThuoc;
            sanPhamDb.Mota = sanPham.Mota;
            sanPhamDb.TrangThai = sanPham.TrangThai;
            sanPhamDb.NgayCapNhat = DateTime.Now;

            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sanpham");

            // Xử lý hình ảnh chính
            if (sanPham.imagesLoad != null && sanPham.imagesLoad.Length > 0)
            {
                string imageName = Guid.NewGuid().ToString() + "_" + sanPham.imagesLoad.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await sanPham.imagesLoad.CopyToAsync(fs);
                }

                sanPhamDb.HinhAnh = imageName; // Cập nhật hình ảnh chính
            }

            // Xử lý danh sách hình ảnh
            if (sanPham.imagesLoadList != null && sanPham.imagesLoadList.Count > 0)
            {
                List<string> danhSachHinhAnh = new List<string>();

                foreach (var file in sanPham.imagesLoadList)
                {
                    string imageName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fs);
                    }

                    danhSachHinhAnh.Add(imageName);
                }

                sanPhamDb.DanhSachHinhAnh = string.Join(",", danhSachHinhAnh); // Cập nhật danh sách hình ảnh
            }

            _dataContext.Update(sanPhamDb);
            await _dataContext.SaveChangesAsync();

            TempData["success"] = "Cập nhật sản phẩm thành công";
            return RedirectToAction("Index");
        }
    }
}
