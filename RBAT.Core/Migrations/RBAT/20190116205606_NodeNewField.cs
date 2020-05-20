using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class NodeNewField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "LandUseFactor",
                table: "Node",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SizeOfIrrigatedArea",
                table: "Node",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LandUseFactor",
                table: "Node");

            migrationBuilder.DropColumn(
                name: "SizeOfIrrigatedArea",
                table: "Node");
        }
    }
}
