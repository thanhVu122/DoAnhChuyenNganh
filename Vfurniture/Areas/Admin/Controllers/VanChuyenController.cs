using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/VanChuyen")]
    [Authorize(Roles = "admin")]
    public class VanChuyenController : Controller
    {
        private readonly DataContext _dataContext;
        public VanChuyenController(DataContext dataContext)
        {
            _dataContext = dataContext;

        }
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var DanhSachVanChuyen = await _dataContext.VanChuyens.ToListAsync();
            ViewBag.DSVC = DanhSachVanChuyen;
            return View();
        }
        [HttpPost]
        [Route("StoreShipping")]

        public async Task<IActionResult> StoreShipping(VanChuyen shippingModel, string phuong, string quan, string tinh, decimal price)
        {
            // Gán các giá trị từ tham số vào model
            shippingModel.TinhThanhPho = tinh;
            shippingModel.QuanHuyen = quan;
            shippingModel.PhuongXa = phuong;
            shippingModel.PhiVanChuyen = price;

            try
            {
                // Kiểm tra xem dữ liệu đã tồn tại hay chưa
                var existingShipping = await _dataContext.VanChuyens
                    .AnyAsync(x => x.TinhThanhPho == tinh && x.PhuongXa == phuong && x.QuanHuyen == quan);


                if (existingShipping)
                {
                    return Ok(new { duplicate = true, message = "Dữ liệu trùng lặp." });
                }

                // Thêm dữ liệu mới vào cơ sở dữ liệu
                _dataContext.VanChuyens.Add(shippingModel);
                await _dataContext.SaveChangesAsync();

                return Ok(new { success = true, message = "Thêm shipping thành công." });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi xảy ra ngoại lệ
                return StatusCode(500, new { error = true, message = "Đã xảy ra lỗi khi thêm shipping.", detail = ex.Message });
            }
        }
        public async Task<IActionResult> Delete(int Id)
        {
            VanChuyen vanChuyen = await _dataContext.VanChuyens.FindAsync(Id);
            _dataContext.VanChuyens.Remove(vanChuyen);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Đã xóa thành công";
            return RedirectToAction("Index");
        }

    }
}
