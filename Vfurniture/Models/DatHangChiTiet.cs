using System.ComponentModel.DataAnnotations.Schema;

namespace Vfurniture.Models
{
    public class DatHangChiTiet
    {
        public int Id { get; set; }
        public string TenNguoiDat { get; set; }
        public string MaDatHang { get; set; }
        public long MaSanPham { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        [ForeignKey("MaSanPham")]
        public SanPhams SanPhams { get; set; }

    }
}
