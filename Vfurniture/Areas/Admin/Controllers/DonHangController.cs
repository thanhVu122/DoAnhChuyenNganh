using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vfurniture.Reponsitory;

namespace Vfurniture.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class DonHangController : Controller
    {
        private readonly DataContext _dataContext;

        public DonHangController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            var datHangs = await _dataContext.DatHangs
                .OrderByDescending(p => p.MaDatHang)
                .ToListAsync();

            return View(datHangs);
        }
        public async Task<IActionResult> XemChiTiet(string MaDonHang)
        {
            if (string.IsNullOrEmpty(MaDonHang))
            {
                return NotFound("Mã đơn hàng không được để trống.");
            }

            // Lấy thông tin chi tiết đơn hàng
            var chiTietDonHang = await _dataContext.DatHangChiTiets
                .Include(od => od.SanPhams)
                .Where(od => od.MaDatHang == MaDonHang)
                .ToListAsync();
            var GiaShipping = _dataContext.DatHangs.Where(od => od.MaDatHang == MaDonHang).First();
            ViewBag.DanhSachDatHangs = GiaShipping.GiaShip;
            // Nếu không có chi tiết đơn hàng nào
            if (chiTietDonHang == null || !chiTietDonHang.Any())
            {
                return NotFound($"Không tìm thấy chi tiết đơn hàng với mã: {MaDonHang}");
            }

            return View(chiTietDonHang); // Trả về danh sách chi tiết đơn hàng
        }
        [HttpPost]
        [Route("CapNhatDonHang")]
        public async Task<IActionResult> CapNhatDonHang(string orderCode, int status)
        {

            var order = await _dataContext.DatHangs.FirstOrDefaultAsync(o => o.MaDatHang == orderCode);
            if (order == null)
            {
                return NotFound();
            }
            order.TrangThai = status;
            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Đơn hàng đã được cập nhật" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã có lỗi");
            }
        }



    }
}
