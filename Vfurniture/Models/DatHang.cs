using System.ComponentModel.DataAnnotations;

namespace Vfurniture.Models
{
    public class DatHang
    {
        [Key]
        public string MaDatHang { get; set; }
        public DateTime NgayTao { get; set; }   
        public int TrangThai {  get; set; }
        public string TenNguoiDat {  get; set; }
        public decimal GiaShip { get; set; }
        public string MaKhuyenMai { get; set; } // Lưu mã khuyến mãi
        public decimal GiamGia { get; set; } // Số tiền giảm giá

        public ICollection<DatHangChiTiet> ChiTietDonHangs { get; set; }
    }
}
