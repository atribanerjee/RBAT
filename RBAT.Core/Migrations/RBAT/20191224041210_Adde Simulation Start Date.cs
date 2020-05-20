using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class AddeSimulationStartDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SimulationStartDate",
                table: "NodeOptimalSolutions",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SimulationStartDate",
                table: "ChannelOptimalSolutions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SimulationStartDate",
                table: "NodeOptimalSolutions");

            migrationBuilder.DropColumn(
                name: "SimulationStartDate",
                table: "ChannelOptimalSolutions");
        }
    }
}
