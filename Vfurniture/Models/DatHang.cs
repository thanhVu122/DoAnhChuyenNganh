using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Tinh { get; set; }  // Tỉnh/Thành phố
        public string Quan { get; set; }  // Quận/Huyện
        public string Phuong { get; set; } // Phường/Xã
        public string DiaChiCuThe { get; set; } // Địa chỉ cụ

        public string? PhuongThucThanhToan {  get; set; }    
        //[ForeignKey("NguoiDung")]
        //public int MaNguoiDung { get; set; } // Khóa ngoại liên kết với NguoiDungModel
        //public virtual NguoiDungModel NguoiDung { get; set; } // Navigation property
        public ICollection<DatHangChiTiet> ChiTietDonHangs { get; set; }
    }
}
