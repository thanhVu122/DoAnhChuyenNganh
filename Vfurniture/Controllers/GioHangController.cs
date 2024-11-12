using Microsoft.AspNetCore.Mvc;
using Vfurniture.Models;
using Vfurniture.Reponsitory;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Vfurniture.Models.ViewModels;
namespace Vfurniture.Controllers
{
    public class GioHangController : Controller
    {
        private readonly DataContext _dataContext;

        public GioHangController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            List<GioHangsModel> gioHangsItem = HttpContext.Session.GetJson<List<GioHangsModel>>("GioHang") ?? new List<GioHangsModel>();
            GioHangsViewModel cartVM = new()
            {
                GioHangs = gioHangsItem,
                GioHangsTotal = gioHangsItem.Sum(g => g.Gia * g.SoLuong)
            };
            return View(cartVM);
        }

        public async Task<IActionResult> Them(int Id)
        {
            SanPhams sanPhams = await _dataContext.SanPhams.FindAsync(Id);

            List<GioHangsModel> gioHangs = HttpContext.Session.GetJson<List<GioHangsModel>>("GioHang") ?? new List<GioHangsModel>();

            GioHangsModel gioHangsModel = gioHangs.Where(g => g.SanphamId == Id).FirstOrDefault();
            if (gioHangsModel == null)
            {
                gioHangs.Add(new GioHangsModel(sanPhams));

            }
            else
            {
                gioHangsModel.SoLuong += 1;
            }
            HttpContext.Session.SetJson("GioHang", gioHangs);
            TempData["success"] = "Thêm vào giỏ hàng thành công";
            return Redirect(Request.Headers["Referer"].ToString());

        }
        public async Task<IActionResult> congSanpham(int Id)
        {
            List<GioHangsModel> gioHangs = HttpContext.Session.GetJson<List<GioHangsModel>>("GioHang");
            GioHangsModel gioHangsModel = gioHangs.Where(g => g.SanphamId == Id).FirstOrDefault();
            if (gioHangsModel.SoLuong >= 1)
            {
                ++gioHangsModel.SoLuong;
            }
            else
                gioHangs.RemoveAll(p => p.SanphamId == Id);
            if (gioHangs.Count == 0)
            {
                HttpContext.Session.Remove("GioHang");

            }
            else 
            HttpContext.Session.SetJson("GioHang", gioHangs);
            
            return RedirectToAction("Index");   
        }
        public async Task<IActionResult> truSanpham(int Id)
        {
            List<GioHangsModel> gioHangs = HttpContext.Session.GetJson<List<GioHangsModel>>("GioHang");
            GioHangsModel gioHangsModel = gioHangs.Where(g => g.SanphamId == Id).FirstOrDefault();
            if (gioHangsModel.SoLuong > 1)
            {
                --gioHangsModel.SoLuong;
            }
            else
                gioHangs.RemoveAll(p => p.SanphamId == Id);
            if (gioHangs.Count == 0)
            {
                HttpContext.Session.Remove("GioHang");

            }
            else
                HttpContext.Session.SetJson("GioHang", gioHangs);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> remove(int Id)
        {
            List<GioHangsModel> gioHangs = HttpContext.Session.GetJson<List<GioHangsModel>>("GioHang");
            gioHangs.RemoveAll(g => g.SanphamId == Id);
            if (gioHangs.Count == 0)
            {
                HttpContext.Session.Remove("GioHang");

            }
            else
                HttpContext.Session.SetJson("GioHang", gioHangs);
            return RedirectToAction("Index");
            
        }
    }
}