using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vfurniture.Migrations
{
    public partial class ThemTrangThaiDanhGia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SoSao",
                table: "DanhGias",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrangThai",
                table: "DanhGias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "DanhGias");

            migrationBuilder.AlterColumn<string>(
                name: "SoSao",
                table: "DanhGias",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
