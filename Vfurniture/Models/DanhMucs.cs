using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vfurniture.Models
{
    public class DanhMucs
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [StringLength(30)]
        [DisplayName("Mã danh mục")]
        public string? MaDanhMuc { get; set; } // Mã danh mục (khóa chính)

        [Required]
        [StringLength(100)]
        [DisplayName("Tên danh mục")]
        public string TenDanhMuc { get; set; } // Tên danh mục

        [Required]
        [DisplayName("Trạng thái")]
        public bool TrangThai { get; set; } // Trạng thái của danh mục (true: hiển thị, false: ẩn)

        // Quan hệ 1-n với sản phẩm
        public ICollection<SanPhams> SanPhams { get; set; } // Danh sách sản phẩm thuộc danh mục

    }
}
