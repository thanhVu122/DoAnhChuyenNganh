using Microsoft.AspNetCore.Mvc;
using Vfurniture.Models;
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

        public async Task<IActionResult>  Deltail(int id)
        {
            if (id == null) { 
            return RedirectToAction("Index");
            }
            var sanPhamtuDanhMuc = _dataContext.SanPhams.Where(s=>s.MaSanPham==id).FirstOrDefault();

            return View(sanPhamtuDanhMuc);
        }
    }
}
