using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Vfurniture.Models;

namespace Vfurniture.Reponsitory
{

    public class DataContext : IdentityDbContext<AppNguoiDung>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<DanhMucs> DanhMucs { get; set; }
        public DbSet<SanPhams> SanPhams { get; set; }
        public DbSet<DatHang> DatHangs { get; set; }
        public DbSet<DatHangChiTiet> DatHangChiTiets { get; set; }

        public DbSet<DanhGia> DanhGias { get; set; }
        public DbSet<LienHe> LienHes { get; set; }
        public DbSet<VanChuyen> VanChuyens { get; set; }
        public DbSet<KhuyenMaiModel> KhuyenMaiModels { get; set; }
        //public DbSet<GioHangsModel> GioHangs { get; set; }
        public DbSet<NguoiDungModel> NguoiDungs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình các mối quan hệ hoặc các ràng buộc nếu cần.
            // Ví dụ: Nếu có mối quan hệ giữa SanPham và DanhMuc.
            modelBuilder.Entity<SanPhams>()
                .HasOne(sp => sp.DanhMuc)
                .WithMany(dm => dm.SanPhams)
                .HasForeignKey(sp => sp.MaDanhMuc);
            // Mối quan hệ giữa DatHang và DatHangChiTiet
            modelBuilder.Entity<DatHang>()
                .HasMany(dh => dh.ChiTietDonHangs)
                .WithOne(ct => ct.DatHang)
                .HasForeignKey(ct => ct.MaDatHang)
                .OnDelete(DeleteBehavior.Cascade);

          //  modelBuilder.Entity<DatHang>()
          //.HasOne(dh => dh.NguoiDung)
          //.WithMany(nd => nd.DatHangs)
          //.HasForeignKey(dh => dh.MaNguoiDung)
          //.OnDelete(DeleteBehavior.Cascade); // Xóa người dùng sẽ xóa tất cả các đơn hàng của họ


            base.OnModelCreating(modelBuilder);
        }
    }
}
