using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}
