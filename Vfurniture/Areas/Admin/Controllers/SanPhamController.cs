using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamController : Controller
    {

        private readonly DataContext _dataContext;
        public SanPhamController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            var sanPhams = await _dataContext.SanPhams
                .Include(p => p.DanhMuc)
              
                .ToListAsync();

            return View(sanPhams);
        }
        public IActionResult Create()
        {
            ViewBag.sp = new SelectList(_dataContext.SanPhams, "MaSanPham", "TenSanPham");

            return View();
        }
    }
}
