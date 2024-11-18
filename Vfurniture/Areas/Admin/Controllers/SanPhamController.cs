using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
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
            ViewBag.dm = new SelectList(_dataContext.DanhMucs, "MaDanhMuc", "TenDanhMuc",sanPhams.MaDanhMuc);

            if (ModelState.IsValid)
            {

                if (sanPhams.imagesLoad != null && sanPhams.imagesLoad.Length > 0)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sanpham");

                    string imegaName = Guid.NewGuid().ToString() + "_" + sanPhams.imagesLoad.FileName;
                    string filePath = Path.Combine(uploadsDir, imegaName);

                    // Save the file
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await sanPhams.imagesLoad.CopyToAsync(fs);
                    fs.Close();
                    sanPhams.HinhAnh = imegaName; // Save the image name in the database
                }

                _dataContext.Add(sanPhams);
                await _dataContext.SaveChangesAsync();
                TempData["Thanhcong"] = "Thêm sản phẩm thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ThatBai"] = "Không hợp lệ";
                List<string> erorrList = new List<string>();

                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                erorrList.Add(error.ErrorMessage);        
                    }
                }

            }
            return View(sanPhams);
        }

        [HttpGet]
        public async Task<IActionResult>Delete (int Id)
        {
            var move=await _dataContext.SanPhams.FirstOrDefaultAsync(x=>x.MaSanPham==Id);
            return View(move);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCf(int Id)
        {
            var move = await _dataContext.SanPhams.FindAsync(Id);
            if (move!= null)
            {
                _dataContext.Remove(move);
            }
            await _dataContext.SaveChangesAsync();
            TempData["Success"] = "Sản phẩm đã được xóa thành công.";
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
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

            // Update fields with new values
            sanPhamDb.TenSanPham = sanPham.TenSanPham;
            sanPhamDb.MaDanhMuc = sanPham.MaDanhMuc;
            sanPhamDb.Gia = sanPham.Gia;
            sanPhamDb.Discount = sanPham.Discount;
            sanPhamDb.KichThuoc = sanPham.KichThuoc;
            sanPhamDb.Mota = sanPham.Mota;
            sanPhamDb.TrangThai = sanPham.TrangThai;

            // Set NgayCapNhat to the current date and time
            sanPhamDb.NgayCapNhat = DateTime.Now;

            // Update image if a new one is uploaded
            if (sanPham.imagesLoad != null && sanPham.imagesLoad.Length > 0)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/sanpham");
                string imageName = Guid.NewGuid().ToString() + "_" + sanPham.imagesLoad.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await sanPham.imagesLoad.CopyToAsync(fs);
                }

                sanPhamDb.HinhAnh = imageName; // Update with the new image name
            }

            _dataContext.Update(sanPhamDb);
            await _dataContext.SaveChangesAsync();

            TempData["ThanhCong"] = "Cập nhật sản phẩm thành công";
            return RedirectToAction("Index");
        }

    }

}
