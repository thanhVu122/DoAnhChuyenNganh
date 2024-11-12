using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Vfurniture.Models
{
    public class SanPhams
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Mã sản phẩm")]
        public int MaSanPham { get; set; } // Mã sản phẩm, khóa chính

        [Required]
        [StringLength(300)]
        [DisplayName("Tên sản phẩm")]
        public string TenSanPham { get; set; } // Tên sản phẩm

        [ForeignKey("DanhMuc")]
        public string MaDanhMuc { get; set; } // Mã danh mục, khóa ngoại
        [DisplayName("Giá")]
        public int Gia { get; set; } // Giá từng sản phẩm
        [DisplayName("Phần trăm giảm giá")]
        public int? Discount { get; set; } // Phần trăm giảm giá, cho phép null

        [StringLength(200)]
        [DisplayName("Kích thước")]
        public string? KichThuoc { get; set; } // Kích thước từng sản phẩm
        [DisplayName("Mô tả kích thước")]
        public string? MoTaKichThuoc { get; set; } // Phần mô tả kích thước của sản phẩm
        [DisplayName("Mô tả chất liệu")]
        public string? MoTaChatLieu { get; set; } // Phần mô tả chất liệu của sản phẩm

        [Column(TypeName = "varchar(MAX)")]
        [DisplayName("Hình ảnh")]
        public string HinhAnh { get; set; } // Hình ảnh chính của sản phẩm

        [Column(TypeName = "varchar(MAX)")]
        [DisplayName("Danh sách hình ảnh")]
        public string? DanhSachHinhAnh { get; set; } // Danh sách hình ảnh chi tiết của sản phẩm
        [DisplayName("Mô tả")]
        public string? Mota { get; set; } // Các mô tả chi tiết của sản phẩm
        [DisplayName("Ngày thêm")]
        public DateTime NgayTao { get; set; } // Ngày thêm sản phẩm
        [DisplayName("Ngày cập nhật")]
        public DateTime NgayCapNhat { get; set; } // Ngày cập nhật sản phẩm
        [DisplayName("Trạng thái")]
        public bool TrangThai { get; set; } // Trạng thái của danh mục (true: hiển thị, false: ẩn)
        // Điều hướng tới DanhMuc
        public virtual DanhMucs DanhMuc { get; set; }
        [NotMapped]
        [FileExtensions]
        public IFormFile imagesLoad { get; set; }
    }
}