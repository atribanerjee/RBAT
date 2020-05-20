using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class RecordFlowStaiongeo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Altitude",
                table: "RecordedFlowStation",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "RecordedFlowStation",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "RecordedFlowStation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altitude",
                table: "RecordedFlowStation");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "RecordedFlowStation");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "RecordedFlowStation");
        }
    }
}
