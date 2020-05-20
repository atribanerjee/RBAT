using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class StartingReservoirLevelInitialElevationisoftypedouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "InitialElevation",
                table: "StartingReservoirLevel",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "InitialElevation",
                table: "StartingReservoirLevel",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
