using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Vfurniture.Models.ViewModels
{
    public class LoginViewModel
    {
        public int MaNguoiDung { get; set; } // Mã người dùng, khóa chính

        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        [DisplayName("Tài khoản")]
        public string TaiKhoan { get; set; } // Tài khoản người dùng
        [DataType(DataType.Password), Required(ErrorMessage = "Mật khẩu phải từ 6 đến 32 ký tự.")]
        [DisplayName("Mật khẩu")]
        public string MatKhau { get; set; } // Mật khẩu người dùng
        public string ReturnURL { get; set; }
    }
}
