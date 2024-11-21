﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<DatHang> DatHangs{ get; set; }
        public DbSet<DatHangChiTiet> DatHangChiTiets { get; set; }

        public DbSet<DanhGia> DanhGias { get; set; }
        public DbSet<LienHe> LienHes {  get; set; }
      public DbSet<VanChuyen> VanChuyens { get; set; }      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình các mối quan hệ hoặc các ràng buộc nếu cần.
            // Ví dụ: Nếu có mối quan hệ giữa SanPham và DanhMuc.
            modelBuilder.Entity<SanPhams>()
                .HasOne(sp => sp.DanhMuc)
                .WithMany(dm => dm.SanPhams)
                .HasForeignKey(sp => sp.MaDanhMuc);

            base.OnModelCreating(modelBuilder);
        }
    }
}
