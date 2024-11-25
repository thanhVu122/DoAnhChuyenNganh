using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vfurniture.Models
{
    public class DatHangChiTiet
    {
        [Key]
        public int Id { get; set; }
        public string MaDatHang { get; set; }
        public string TenNguoiDat { get; set; }
        public long MaSanPham { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        [ForeignKey("MaSanPham")]
        public SanPhams SanPhams { get; set; }
        [ForeignKey("MaDatHang")]
        public DatHang DatHang { get; set; } // Tham chiếu đến đơn hàng

    }
}
