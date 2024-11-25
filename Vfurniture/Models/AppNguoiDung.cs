using Microsoft.AspNetCore.Identity;

namespace Vfurniture.Models
{
    public class AppNguoiDung : IdentityUser
    {
        public string RoleId { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string TenNguoiDung { get; set; } // Tên người dùng
    }
}
