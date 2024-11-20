using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Vfurniture.Reponsitory.Validation;

namespace Vfurniture.Models
{
    public class SanPhams
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Mã sản phẩm")]
        public long MaSanPham { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(300, ErrorMessage = "Tên sản phẩm không được vượt quá 300 ký tự")]
        [DisplayName("Tên sản phẩm")]
        public string TenSanPham { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [ForeignKey("DanhMuc")]
        public string MaDanhMuc { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(1000, int.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 1000")]
        [DisplayName("Giá")]
  
        public decimal Gia { get; set; }

        [Range(0, 100, ErrorMessage = "Phần trăm giảm giá phải trong khoảng 0 đến 100")]
        [DisplayName("Phần trăm giảm giá")]
        public int? Discount { get; set; }

        [StringLength(200, ErrorMessage = "Kích thước không được vượt quá 200 ký tự")]
        [DisplayName("Kích thước")]
        public string? KichThuoc { get; set; }

        [DisplayName("Mô tả kích thước")]
        public string? MoTaKichThuoc { get; set; }

        [DisplayName("Mô tả chất liệu")]
        public string? MoTaChatLieu { get; set; }

        
        [Column(TypeName = "varchar(MAX)")]
        [DisplayName("Hình ảnh")]
        public string HinhAnh { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [DisplayName("Danh sách hình ảnh")]
        public string? DanhSachHinhAnh { get; set; }

        [DisplayName("Mô tả")]
        public string? Mota { get; set; }

        [DisplayName("Ngày thêm")]
        [DataType(DataType.DateTime)]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [DisplayName("Ngày cập nhật")]
        [DataType(DataType.DateTime)]
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        [DisplayName("Trạng thái")]
        public bool TrangThai { get; set; }

        public virtual DanhMucs DanhMuc { get; set; }

        // Not mapped property for file upload
        [NotMapped]
        [FileExtension]
        public IFormFile imagesLoad { get; set; }
        [NotMapped]
        public List<IFormFile> imagesLoadList { get; set; }
        public decimal GiaKhuyenMai
        {
            get
            {
                return Gia - (Gia * (Discount ?? 0) / 100); // Nếu Discount là null, mặc định 0%
            }
        }
        public List<DanhGia> DanhGiaList { get; set; }
    }
}
