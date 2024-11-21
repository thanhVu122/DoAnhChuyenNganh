using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Vfurniture.Models
{
    public class LienHe
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Họ và tên")]
        [Required(ErrorMessage = ("Vui lòng nhập tên"))]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [DisplayName("Email")]
        public string Email { get; set; } // Email người dùng (override từ IdentityUser)

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [DisplayName("Số điện thoại")]
        public string SoDienThoai { get; set; } // Số điện thoại
        [DisplayName("Nội dung")]
        [Required(ErrorMessage =("Vui lòng nhập nội dung"))]
        public string NoiDung { get; set; }
        [Display(Name = "Ngày gửi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
