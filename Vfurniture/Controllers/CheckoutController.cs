using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Vfurniture.Areas.Admin.Reponsitory;
using Vfurniture.Models;
using Vfurniture.Reponsitory;
using Vfurniture.Service.Momo;

namespace Vfurniture.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        private readonly IMomoService _momoService;
        public CheckoutController(DataContext dataContext, IEmailSender emailSender ,IMomoService momoService)
        {
            _emailSender = emailSender;
            _momoService = momoService;
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string PaymentMethod , string DiaChiCuThe, string quan, string tinh, string phuong)
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
                var orderItem = new DatHang
                {

                    MaDatHang = orderCode,
                    TenNguoiDat = userEmail,
                    TrangThai = 1,
                    NgayTao = DateTime.Now,
                    Tinh = tinh,
                    Quan = quan,
                    Phuong = phuong,
                    DiaChiCuThe = DiaChiCuThe

                };
                var GiaVanChuyenCookie = Request.Cookies["GiaVanChuyen"];
                decimal giaVanChuyen = 0;
                if (GiaVanChuyenCookie != null)
                {
                    var GiaVanChuyenJson = GiaVanChuyenCookie;
                    giaVanChuyen = JsonConvert.DeserializeObject<decimal>(GiaVanChuyenJson);
                    orderItem.GiaShip = giaVanChuyen;
                }
                if (PaymentMethod != null)
                {
                    orderItem.PhuongThucThanhToan = PaymentMethod;
                }
                else
                {
                    orderItem.PhuongThucThanhToan = "COD";
                }
                _dataContext.Add(orderItem);
                await _dataContext.SaveChangesAsync();  // Đảm bảo gọi SaveChangesAsync()
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
                //Send mail 
                var receiver = userEmail;
                var subject = "Đặt hàng thành công";
                var message = "Chúng tôi cảm ơn bạn đã mua sắm trên VFurniture. Đơn hàng của bạn đang được xử lý.";
                await _emailSender.SendEmailAsync(receiver, subject, message);
                TempData["success"] = "Đơn hàng đã được tạo.Chờ xử lý";
                return RedirectToAction("HistoryOrder", "Acccount");
            }
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallBack(MomoInfoModel model)
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
            // Lấy các tham số từ query string
            var requestQuery = HttpContext.Request.Query;

            // Xử lý việc trả về của Momo khi có callback
       
            if (requestQuery["resultCode"] != 0) //giao dicjh kh thanh cong
            {
                var newMomoInsert = new MomoInfoModel
                {
                    OrderId = requestQuery["orderId"],
                    FullName = User.FindFirstValue(ClaimTypes.Email),
                    Amount = decimal.Parse(requestQuery["amount"]),
                    OrderInfo = requestQuery["orderInfo"], // Sửa lại key nếu cần thiết
                    DatePaid = DateTime.Now,
                };
                var diaChiCuThe = "Địa chỉ từ callback";
                var tinh = "Tỉnh từ callback";
                var quan = "Quận từ callback";
                var phuong = "Phường từ callback";
                _dataContext.Add(newMomoInsert);
                await _dataContext.SaveChangesAsync();
                // tien hanh dat don hang khi thanh toan thanh con
                 await Checkout(requestQuery["orderId"], diaChiCuThe,quan,tinh,phuong);
            }
            else
            {
                TempData["success"] = "Giao dich khong thanh cong";
                return RedirectToAction("Index","GioHang");
            }
            return View(response);
        }
    }
}
