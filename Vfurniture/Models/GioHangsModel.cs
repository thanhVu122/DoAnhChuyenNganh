namespace Vfurniture.Models
{
    public class GioHangsModel
    {
        public string SanphamName { get; set; }
        public long SanphamId { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }
        public decimal GiaKhuyenMai { get; set; }
        public string HinhAnh { get; set; }
        public string KichThuoc { get; set; }
        public decimal TongTien => SoLuong * GiaKhuyenMai;
        public GioHangsModel() { }
        public GioHangsModel(SanPhams sanPhams)
        {
            SanphamId = sanPhams.MaSanPham;
            SanphamName = sanPhams.TenSanPham;
            Gia = sanPhams.Gia;
            GiaKhuyenMai = sanPhams.GiaKhuyenMai;
            SoLuong = 1;
            HinhAnh = sanPhams.HinhAnh;
            KichThuoc = sanPhams.KichThuoc;
        }
    }
}
