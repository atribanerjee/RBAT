using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class AddSimulationEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SimulationEndDate",
                table: "NodeOptimalSolutions",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SimulationEndDate",
                table: "ChannelOptimalSolutions",
                nullable: true);

            migrationBuilder.Sql("UPDATE NodeOptimalSolutions " +
                "SET SimulationEndDate = CalculationEnds " +
                "FROM NodeOptimalSolutions " +
                "INNER JOIN Scenario ON NodeOptimalSolutions.ScenarioID = Scenario.Id");

            migrationBuilder.Sql("UPDATE ChannelOptimalSolutions " +
                "SET SimulationEndDate = CalculationEnds " +
                "FROM ChannelOptimalSolutions " +
                "INNER JOIN Scenario ON ChannelOptimalSolutions.ScenarioID = Scenario.Id");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SimulationEndDate",
                table: "NodeOptimalSolutions");

            migrationBuilder.DropColumn(
                name: "SimulationEndDate",
                table: "ChannelOptimalSolutions");
        }
    }
}
