using Microsoft.EntityFrameworkCore.Migrations;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class AddedRatedPower : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RatedPower",
                table: "Channel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatedPower",
                table: "Channel");
        }
    }
}
