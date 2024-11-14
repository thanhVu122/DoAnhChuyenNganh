namespace Vfurniture.Models
{
	public class GioHangsModel
	{

		public string SanphamName { get; set; }
		public long SanphamId { get; set; }
		public int SoLuong { get; set; }
		public int? Gia { get; set; }
		public int? Discount { get; set; } // Phần trăm giảm giá, cho phép null
		public decimal? tongTien
		{
			get { return SoLuong * Gia; }
		}
		public decimal? GiaMoi
		{
			get
			{
				if (Discount.HasValue && Discount.Value > 0)
				{
					return Gia * (1 - Discount.Value / 100m);
					
				}
				return Gia;
			}
		}
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
			Discount = sanPhams.Discount;
		}
	}
}
