using Microsoft.EntityFrameworkCore;
using Vfurniture.Models;

namespace Vfurniture.Reponsitory
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();

            // Danh sách các danh mục cần thêm
            var danhMucList = new List<DanhMucs>
    {
        new DanhMucs { MaDanhMuc = "TQA", TenDanhMuc = "Tủ quần áo", TrangThai = true },
        new DanhMucs { MaDanhMuc = "G", TenDanhMuc = "Giường", TrangThai = true },
        new DanhMucs { MaDanhMuc = "KTV", TenDanhMuc = "Kệ TV", TrangThai = true },
        new DanhMucs { MaDanhMuc = "TDG", TenDanhMuc = "Tủ đầu giường", TrangThai = true },
        new DanhMucs { MaDanhMuc = "BTD", TenDanhMuc = "Bàn trang điểm", TrangThai = true },
        new DanhMucs { MaDanhMuc = "BG", TenDanhMuc = "Bàn ghế", TrangThai = true }
    };

            // Kiểm tra và thêm các danh mục nếu chưa có
            foreach (var danhMuc in danhMucList)
            {
                if (!_context.DanhMucs.Any(d => d.MaDanhMuc == danhMuc.MaDanhMuc))
                {
                    _context.DanhMucs.Add(danhMuc);
                }
            }

            // Thêm sản phẩm mẫu nếu chưa có sản phẩm nào
            var sanPhamList = new List<SanPhams>
    {
        new SanPhams
        {
            TenSanPham = "Tủ quần áo VIENNA Ver 1 H50",
            Gia = 3990000,
            Discount = 50,
            KichThuoc = "1m2",
            HinhAnh = "tu1.jpg",
            MoTaChatLieu = "Gỗ Phối Trắng",
            MoTaKichThuoc = "1m2x50cm",
            DanhMuc = _context.DanhMucs.First(d => d.MaDanhMuc == "TQA"),
            DanhSachHinhAnh = "",
            Mota = "TỦ QUẦN ÁO GỖ 1 CÁNH MOHO VIENNA 201 ( Màu tự nhiên )",
            MaDanhMuc = "TQA",
            NgayCapNhat = DateTime.Now,
            NgayTao = DateTime.Now,
            TrangThai = true
        },
        new SanPhams
        {
            TenSanPham = "Ghế Sofa MOHO HALDEN 801",
            Gia = 9990000,
            Discount = 5,
            KichThuoc = "Dài 180cm x Rộng 85cm x Cao 82cm",
            HinhAnh = "ghe1.jpg",
            MoTaChatLieu = "- Gỗ cao su tự nhiên\r\n\r\n- Vải sợi tổng hợp có khả năng chống thấm nước và dầu",
            MoTaKichThuoc = "Dài 180cm x Rộng 85cm x Cao 82cm",
            DanhMuc = _context.DanhMucs.First(d => d.MaDanhMuc == "BG"),
            DanhSachHinhAnh = "",
            Mota = "2. CHI TIẾT VẬT LIỆU\r\nVải bọc sợi tổng hợp - Soil release",
            MaDanhMuc = "BG",
            NgayCapNhat = DateTime.Now,
            NgayTao = DateTime.Now,
            TrangThai = true
        }
    };

            // Kiểm tra và thêm từng sản phẩm nếu chưa tồn tại
            foreach (var sanPham in sanPhamList)
            {
                if (!_context.SanPhams.Any(sp => sp.TenSanPham == sanPham.TenSanPham))
                {
                    _context.SanPhams.Add(sanPham);
                }
            }

            _context.SaveChanges();

        }
    }
}
