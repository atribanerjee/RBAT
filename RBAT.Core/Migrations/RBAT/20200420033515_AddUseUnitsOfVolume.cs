using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class AddUseUnitsOfVolume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseUnitsOfVolume",
                table: "NodeOptimalSolutions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseUnitsOfVolume",
                table: "ChannelOptimalSolutions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseUnitsOfVolume",
                table: "NodeOptimalSolutions");

            migrationBuilder.DropColumn(
                name: "UseUnitsOfVolume",
                table: "ChannelOptimalSolutions");
        }
    }
}
