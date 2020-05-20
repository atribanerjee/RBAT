using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class OptimalSolutioAddScenarioName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IdealNetEvaporation",
                table: "NodeOptimalSolutionsData");

            migrationBuilder.DropColumn(
                name: "OptimalNetEvaporation",
                table: "NodeOptimalSolutionsData");

            migrationBuilder.DropColumn(
                name: "ScenarioID",
                table: "NodeOptimalSolutions");

            migrationBuilder.DropColumn(
                name: "ScenarioID",
                table: "ChannelOptimalSolutions");

            migrationBuilder.AddColumn<string>(
                name: "ScenarioName",
                table: "NodeOptimalSolutions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScenarioName",
                table: "ChannelOptimalSolutions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScenarioName",
                table: "NodeOptimalSolutions");

            migrationBuilder.DropColumn(
                name: "ScenarioName",
                table: "ChannelOptimalSolutions");

            migrationBuilder.AddColumn<double>(
                name: "IdealNetEvaporation",
                table: "NodeOptimalSolutionsData",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OptimalNetEvaporation",
                table: "NodeOptimalSolutionsData",
                nullable: true);

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NodeOptimalSolutions_Scenario_ScenarioID",
                table: "NodeOptimalSolutions",
                column: "ScenarioID",
                principalTable: "Scenario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
