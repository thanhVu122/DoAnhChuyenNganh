using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Controllers
{
    public class DanhMucController : Controller
    {
        private readonly DataContext _dataContext;
        public DanhMucController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IActionResult> Index(string maDM = "")
        {
            DanhMucs danhMucs = _dataContext.DanhMucs.Where(c => c.MaDanhMuc == maDM).FirstOrDefault();
            if (danhMucs == null)
            {
                return RedirectToAction("Index");
            }
            var sanPhamtuDanhMuc = _dataContext.SanPhams.Where(s => s.MaDanhMuc == danhMucs.MaDanhMuc);
            return View(await sanPhamtuDanhMuc.OrderByDescending(o => o.MaSanPham).ToListAsync());

        }
    }
}
