using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vfurniture.Migrations
{
    public partial class addDiaChiDatHang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChiCuThe",
                table: "DatHangs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phuong",
                table: "DatHangs",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Quan",
                table: "DatHangs",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tinh",
                table: "DatHangs",
                type: "nvarchar(50)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaChiCuThe",
                table: "DatHangs");

            migrationBuilder.DropColumn(
                name: "Phuong",
                table: "DatHangs");

            migrationBuilder.DropColumn(
                name: "Quan",
                table: "DatHangs");

            migrationBuilder.DropColumn(
                name: "Tinh",
                table: "DatHangs");
        }
    }
}
