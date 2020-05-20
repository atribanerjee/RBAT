using Microsoft.EntityFrameworkCore.Migrations;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class AddedOptimalIdealPowerValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "IdealPowerValue",
                table: "ChannelOptimalSolutionsData",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OptimalPowerValue",
                table: "ChannelOptimalSolutionsData",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdealPowerValue",
                table: "ChannelOptimalSolutionsData");

            migrationBuilder.DropColumn(
                name: "OptimalPowerValue",
                table: "ChannelOptimalSolutionsData");
        }
    }
}
