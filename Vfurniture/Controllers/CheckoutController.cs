﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Vfurniture.Areas.Admin.Reponsitory;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        public CheckoutController(DataContext dataContext,IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Checkout(string tinh, string quan, string phuong, string DiaChiCuThe)
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
                    Tinh = tinh,  // Lưu tỉnh
                    Quan = quan,  // Lưu quận
                    Phuong = phuong, // Lưu phường
                    DiaChiCuThe = DiaChiCuThe // Lưu địa chỉ cụ thể
                };
                var GiaVanChuyenCookie = Request.Cookies["GiaVanChuyen"];
                decimal giaVanChuyen = 0;
                if (GiaVanChuyenCookie != null)
                {
                    var GiaVanChuyenJson = GiaVanChuyenCookie;
                    giaVanChuyen = JsonConvert.DeserializeObject<decimal>(GiaVanChuyenJson);
                    orderItem.GiaShip = giaVanChuyen;
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
            return View();
        }


        //public async Task<IActionResult> ConfirmOrder()
        //{
        //    var userEmail = User.FindFirstValue(ClaimTypes.Email);

        //    if (userEmail == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    else
        //    {
              
        //    }
        //}

    }
}
