using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")] 
    public class DanhGiaController : Controller
    {

        private readonly DataContext _dataContext;

        public DanhGiaController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            var Show = _dataContext.DanhGias.Include(d => d.SanPham).ToList();
            return View(Show );
        }
        [HttpPost]
        public IActionResult UpdateTrangThai(int id, int trangThai)
        {
            var danhGia = _dataContext.DanhGias.FirstOrDefault(d => d.Id == id);
            if (danhGia != null)
            {
                danhGia.TrangThai = trangThai;
                _dataContext.SaveChanges();

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}
