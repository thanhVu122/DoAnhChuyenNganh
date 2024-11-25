using Microsoft.AspNetCore.Mvc;
using Vfurniture.Models;
using Vfurniture.Reponsitory;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Vfurniture.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
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
            var GiaVanChuyenCookie = Request.Cookies["GiaVanChuyen"];
            List<GioHangsModel> gioHangsItem = HttpContext.Session.GetJson<List<GioHangsModel>>("GioHang") ?? new List<GioHangsModel>();

            decimal giaVanChuyen = 0;
            if (GiaVanChuyenCookie != null)
            {
                var GiaVanChuyenJson = GiaVanChuyenCookie;
                giaVanChuyen = JsonConvert.DeserializeObject<decimal>(GiaVanChuyenJson);
            }

            GioHangsViewModel cartVM = new()
            {
                GiaVanChuyen = giaVanChuyen,
                GioHangs = gioHangsItem,
                GioHangsTotal = gioHangsItem.Sum(g => (g.GiaKhuyenMai) * g.SoLuong)
            };
            return View(cartVM);
        }

        public async Task<IActionResult> Them(long Id)
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
        public async Task<IActionResult> congSanpham(long Id)
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
        public async Task<IActionResult> truSanpham(long Id)
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

        public async Task<IActionResult> remove(long Id)
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


        [HttpPost]
        [Route("GioHang/GetShipping")]
        public async Task<IActionResult> GetShipping(VanChuyen vanChuyen, string quan, string tinh, string phuong)
        {
            var existingGia = await _dataContext.VanChuyens
                .FirstOrDefaultAsync(x => x.TinhThanhPho == tinh && x.QuanHuyen == quan && x.PhuongXa == phuong);
            decimal GiaVanChuyen = 0;
            if (existingGia != null)
            {
                GiaVanChuyen = existingGia.PhiVanChuyen;
            }
            else
            {
                // nếu không có trong các phường huyện tỉnh thì cho giá bằng 0
                GiaVanChuyen = 0;
            }
            var GiaVanChuyenJson = JsonConvert.SerializeObject(GiaVanChuyen);
            try
            {
                var cookieOption = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(10),
                    Secure = true
                };
                Response.Cookies.Append("GiaVanChuyen", GiaVanChuyenJson, cookieOption);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");


            }
            return Json(new { GiaVanChuyen });
        }
        [HttpGet]
        [Route("GioHang/DeleteVanChuyen")]
        public IActionResult DeleteVanChuyen()
        {
            Response.Cookies.Delete("GiaVanChuyen");
            return RedirectToAction("Index", "GioHang");
        }

        private GioHangsViewModel GetGioHangsViewModel()
        {
            // Lấy giỏ hàng từ session (dưới dạng JSON)
            List<GioHangsModel> gioHangsItem = HttpContext.Session.GetJson<List<GioHangsModel>>("GioHang") ?? new List<GioHangsModel>();

            decimal giaVanChuyen = 0;
            var GiaVanChuyenCookie = Request.Cookies["GiaVanChuyen"];
            if (GiaVanChuyenCookie != null)
            {
                giaVanChuyen = JsonConvert.DeserializeObject<decimal>(GiaVanChuyenCookie);
            }

            return new GioHangsViewModel
            {
                GiaVanChuyen = giaVanChuyen,
                GioHangs = gioHangsItem,
                GioHangsTotal = gioHangsItem.Sum(g => g.GiaKhuyenMai * g.SoLuong)
            };
        }


    //    [HttpPost]
    //    public IActionResult ApplyCoupon(string couponCode)
    //    {
    //        var viewModel = GetGioHangsViewModel();  // Lấy thông tin giỏ hàng từ phương thức JSON

    //        var khuyenMai = _dataContext.KhuyenMaiModels
    //            .FirstOrDefault(km => km.TenKhuyenMai == "cho");

    //        if (khuyenMai != null && khuyenMai.TrangThai && khuyenMai.NgayBatdau <= DateTime.Now && khuyenMai.NgayKetThuc >= DateTime.Now)
    //        {
    //            viewModel.MaKhuyenMai = couponCode;
    //            viewModel.TienGiamGia = (viewModel.GioHangsTotal * khuyenMai.PhanTramGiam) / 100;
    //            TempData["CouponMessage"] = "Mã khuyến mãi đã được áp dụng.";
    //        }
    //        else
    //        {
    //            TempData["CouponMessage"] = "Mã khuyến mãi không hợp lệ hoặc đã hết hạn.";
    //        }

    //        // Lưu giỏ hàng đã cập nhật vào session dưới dạng JSON
    //        HttpContext.Session.SetJson("GioHang", viewModel.GioHangs);

    //        return RedirectToAction("Index");
    //    }

    }
}
