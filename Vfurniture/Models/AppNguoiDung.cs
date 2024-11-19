using Microsoft.AspNetCore.Identity;

namespace Vfurniture.Models
{
    public class AppNguoiDung : IdentityUser
    {
        public string RoleId { get; set; }
    }
}
