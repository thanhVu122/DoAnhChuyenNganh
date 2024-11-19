namespace Vfurniture.Models
{
	public class GioHangsModel
	{

		public string SanphamName { get; set; }
		public long SanphamId { get; set; }
		public int SoLuong { get; set; }
		public decimal Gia { get; set; }
		//public int? Discount { get; set; } // Phần trăm giảm giá, cho phép null
		public decimal tongTien
		{
			get { return SoLuong * GiaKhuyenMai; }
		}
        public decimal GiaKhuyenMai { get; set; }
        public string HinhAnh { get; set; }
		public string? KichThuoc { get; set; } // Kích thước từng sản phẩm

		public GioHangsModel()
		{

		}
		public GioHangsModel(SanPhams sanPhams)
		{
			SanphamId = sanPhams.MaSanPham;
			SanphamName = sanPhams.TenSanPham;
			Gia = sanPhams.Gia;
			SoLuong = 1;
			HinhAnh = sanPhams.HinhAnh;
			KichThuoc=sanPhams.KichThuoc;
			//Discount = sanPhams.Discount;
			GiaKhuyenMai = sanPhams.GiaKhuyenMai;
		}
	}
}
