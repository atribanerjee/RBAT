using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ClimateStationgeo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Altitude",
                table: "ClimateStation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "ClimateStation",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "ClimateStation",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altitude",
                table: "ClimateStation");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "ClimateStation");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "ClimateStation");
        }
    }
}
