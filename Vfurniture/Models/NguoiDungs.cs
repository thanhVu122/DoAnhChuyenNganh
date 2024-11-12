using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Vfurniture.Models
{
    public class NguoiDungs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaNguoiDung { get; set; } // Mã người dùng, khóa chính

        [Required]
        [StringLength(50)]
        public string TenNguoiDung { get; set; } // Tên người dùng

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } // Email người dùng

        [Required]
        public DateTime NgaySinh { get; set; } // Ngày sinh người dùng

        [Required]
        [StringLength(50)]
        public string TaiKhoan { get; set; } // Tài khoản người dùng

        [Required]
        [StringLength(32)]
        public string MatKhau { get; set; } // Mật khẩu người dùng

        [Required]
        [StringLength(20)]
        public string Quyen { get; set; } // Quyền của người dùng
    }
}
