using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class OptimalSolutionAddScenario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ScenarioID",
                table: "NodeOptimalSolutions",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ScenarioID",
                table: "ChannelOptimalSolutions",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_NodeOptimalSolutions_ScenarioID",
                table: "NodeOptimalSolutions",
                column: "ScenarioID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelOptimalSolutions_ScenarioID",
                table: "ChannelOptimalSolutions",
                column: "ScenarioID");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelOptimalSolutions_Scenario_ScenarioID",
                table: "ChannelOptimalSolutions",
                column: "ScenarioID",
                principalTable: "Scenario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NodeOptimalSolutions_Scenario_ScenarioID",
                table: "NodeOptimalSolutions",
                column: "ScenarioID",
                principalTable: "Scenario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelOptimalSolutions_Scenario_ScenarioID",
                table: "ChannelOptimalSolutions");

            migrationBuilder.DropForeignKey(
                name: "FK_NodeOptimalSolutions_Scenario_ScenarioID",
                table: "NodeOptimalSolutions");

            migrationBuilder.DropIndex(
                name: "IX_NodeOptimalSolutions_ScenarioID",
                table: "NodeOptimalSolutions");

            migrationBuilder.DropIndex(
                name: "IX_ChannelOptimalSolutions_ScenarioID",
                table: "ChannelOptimalSolutions");

            migrationBuilder.DropColumn(
                name: "ScenarioID",
                table: "NodeOptimalSolutions");

            migrationBuilder.DropColumn(
                name: "ScenarioID",
                table: "ChannelOptimalSolutions");
        }
    }
}
