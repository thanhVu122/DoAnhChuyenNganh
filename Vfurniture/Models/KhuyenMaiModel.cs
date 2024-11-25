using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vfurniture.Models
{
    public class KhuyenMaiModel
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Tên khuyến mãi")]
        [StringLength(300, ErrorMessage = "Tên khuyến mãi không được vượt quá 300 ký tự")]
        [Required(ErrorMessage = "Tên khuyến mãi không được để trống")]
        public string TenKhuyenMai { get; set; }

        [DisplayName("Mô tả khuyến mãi")]
        [StringLength(300, ErrorMessage = "Mô tả khuyến mãi không được vượt quá 300 ký tự")]
        [Required(ErrorMessage = "Mô tả khuyến mãi không được để trống")]
        public string Mota { get; set; }

        [DisplayName("Số lượng khuyến mãi")]
        [Required (ErrorMessage ="Vui long nhap so luong")]
        public int SoLuong { get; set; }

        [DisplayName("Phần trăm giảm giá (%)")]
        [Required(ErrorMessage = "Vui lòng nhập phần trăm giảm giá")]
        [Range(0, 100, ErrorMessage = "Phần trăm giảm giá phải nằm trong khoảng từ 0 đến 100.")]
        public decimal PhanTramGiam { get; set; } // Bắt buộc, dùng cho giảm giá %
        [DisplayName("Ngày bắt đầu")]
        public DateTime NgayBatdau { get; set; }

        [DisplayName("Ngày kết thúc")]
        public DateTime NgayKetThuc { get; set; }

        [DisplayName("Trạng thái")]
        public bool TrangThai { get; set; }
    }
}
