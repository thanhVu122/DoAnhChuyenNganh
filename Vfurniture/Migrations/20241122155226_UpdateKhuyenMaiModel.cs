using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vfurniture.Migrations
{
    public partial class UpdateKhuyenMaiModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TenKhyenMai",
                table: "KhuyenMaiModels",
                newName: "TenKhuyenMai");

            migrationBuilder.RenameColumn(
                name: "Gia",
                table: "KhuyenMaiModels",
                newName: "PhanTramGiam");

            migrationBuilder.AlterColumn<bool>(
                name: "TrangThai",
                table: "KhuyenMaiModels",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "GiamGia",
                table: "DatHangs",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "MaKhuyenMai",
                table: "DatHangs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiamGia",
                table: "DatHangs");

            migrationBuilder.DropColumn(
                name: "MaKhuyenMai",
                table: "DatHangs");

            migrationBuilder.RenameColumn(
                name: "TenKhuyenMai",
                table: "KhuyenMaiModels",
                newName: "TenKhyenMai");

            migrationBuilder.RenameColumn(
                name: "PhanTramGiam",
                table: "KhuyenMaiModels",
                newName: "Gia");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "KhuyenMaiModels",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
