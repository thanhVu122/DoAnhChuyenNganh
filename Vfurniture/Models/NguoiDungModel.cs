using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Vfurniture.Models
{
    public class NguoiDungModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaNguoiDung { get; set; } // Mã người dùng, khóa chính

        [DisplayName("Họ và tên")]
        public string TenNguoiDung { get; set; } // Tên người dùng

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DisplayName("Email")]
        public string Email { get; set; } // Email người dùng (override từ IdentityUser)


        [DisplayName("Ngày sinh")]
        public DateTime? NgaySinh { get; set; } // Ngày sinh người dùng

        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        [DisplayName("Tài khoản")]
        public string TaiKhoan { get; set; } // Tài khoản người dùng

        [DataType(DataType.Password), Required(ErrorMessage = "Mật khẩu phải từ 6 đến 32 ký tự.")]
        [DisplayName("Mật khẩu")]
        public string MatKhau { get; set; } // Mật khẩu người dùng

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [DisplayName("Số điện thoại")]
        public string SoDienThoai { get; set; } // Số điện thoại
        public string Quyen { get; set; } // Quyền của người dùng
    }
}
