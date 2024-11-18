using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vfurniture.Migrations
{
    public partial class TaoDonHang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DatHangChiTiets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNguoiDat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaDatHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaSanPham = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatHangChiTiets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDatHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    TenNguoiDat = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatHangs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatHangChiTiets");

            migrationBuilder.DropTable(
                name: "DatHangs");
        }
    }
}
