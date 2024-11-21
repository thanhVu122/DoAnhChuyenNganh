using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vfurniture.Migrations
{
    public partial class AddVanChuyen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VanChuyens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhiVanChuyen = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhuongXa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuanHuyen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhThanhPho = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VanChuyens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VanChuyens");
        }
    }
}
