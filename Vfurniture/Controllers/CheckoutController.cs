using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        public CheckoutController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Checkout()
        {
            var userEmail =
                User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var orderCode = Guid.NewGuid().ToString();
                var orderItem = new DatHang();
                orderItem.MaDatHang = orderCode;
                orderItem.TenNguoiDat = userEmail;
                orderItem.TrangThai = 1;
                orderItem.NgayTao = DateTime.Now;
                _dataContext.Add(orderItem);
                _dataContext.SaveChanges();
                List<GioHangsModel> gioHangsItem = HttpContext.Session.GetJson<List<GioHangsModel>>("GioHang") ?? new List<GioHangsModel>();
                foreach (var item in gioHangsItem)
                {
                    var ChiTietDatHang = new DatHangChiTiet();
                    ChiTietDatHang.TenNguoiDat = userEmail;
                    ChiTietDatHang.MaDatHang = orderCode;
                    ChiTietDatHang.MaSanPham = item.SanphamId;
                    ChiTietDatHang.Gia = item.Gia;
                    ChiTietDatHang.SoLuong = item.SoLuong;
                    _dataContext.Add(ChiTietDatHang);
                    _dataContext.SaveChanges();

                }
                HttpContext.Session.Remove("GioHang");
                TempData["success"] = "Đơn hàng đã được tạo.Chờ xử lý";
                return RedirectToAction("Index","GioHang");
            }
            return View();
        }
    }
}
