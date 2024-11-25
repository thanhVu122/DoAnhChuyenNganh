namespace Vfurniture.Models.ViewModels
{
    public class GioHangsViewModel
    {
        public List<GioHangsModel> GioHangs { get; set; } = new();
        public decimal GioHangsTotal { get; set; }
        public decimal GiaVanChuyen { get; set; }
        public decimal TongThanhToan => GioHangsTotal + GiaVanChuyen - TienGiamGia;
        public string? MaKhuyenMai { get; set; }
        public decimal TienGiamGia { get; set; }
    }
}
