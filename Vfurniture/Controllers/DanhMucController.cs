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

        public async Task<IActionResult> Index(string Sort_by, string maDM = "")
        {
            ViewBag.tendanhmuc=_dataContext.DanhGias.ToList();

            DanhMucs danhMucs = _dataContext.DanhMucs.Where(c => c.MaDanhMuc == maDM).FirstOrDefault();

            if (danhMucs == null)
            {
                return RedirectToAction("Index");
            }
            IQueryable<SanPhams> sanPhamtuDanhMuc = _dataContext.SanPhams.Where(s => s.MaDanhMuc == danhMucs.MaDanhMuc);
            var conut = await sanPhamtuDanhMuc.CountAsync();
            if (conut > 0)
            {
                if (Sort_by == "price_asc")
                {
                    sanPhamtuDanhMuc = sanPhamtuDanhMuc.OrderBy(o => o.Gia);
                }
                else if (Sort_by == "price_desc")
                {
                    sanPhamtuDanhMuc = sanPhamtuDanhMuc.OrderByDescending(o => o.Gia);
                }
                else if (Sort_by == "price_newest")
                {
                    sanPhamtuDanhMuc = sanPhamtuDanhMuc.OrderByDescending(o => o.MaSanPham);
                }
                else if (Sort_by == "price_oldest")
                {
                    sanPhamtuDanhMuc = sanPhamtuDanhMuc.OrderBy(o => o.MaSanPham);
                }
                else
                {
                    sanPhamtuDanhMuc = sanPhamtuDanhMuc.OrderByDescending(o => o.MaSanPham);
                }
            }

            return View(await sanPhamtuDanhMuc.ToListAsync());
        }


    }
}

