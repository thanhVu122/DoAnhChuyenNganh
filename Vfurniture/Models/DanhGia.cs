using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vfurniture.Models
{
    public class DanhGia
    {
        [Key]
        public int Id { get; set; }

        public long MaSanPham { get; set; }
        [DisplayName("Tên")]
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string TenNguoiDanhGia { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        public string MoTa { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        public string Email { get; set; }
        public int SoSao { get; set; }
        [ForeignKey("MaSanPham")]
        public SanPhams SanPham { get; set; }
    }
}
