using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class CustomTieStepsetsenariIdasforeignkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CustomTimeStep_ScenarioId",
                table: "CustomTimeStep",
                column: "ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTimeStep_Scenario_ScenarioId",
                table: "CustomTimeStep",
                column: "ScenarioId",
                principalTable: "Scenario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AlterColumn<long>(
               name: "ScenarioId",
               table: "CustomTimeStep",
               nullable: false,
               oldClrType: typeof(long),
               oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomTimeStep_Scenario_ScenarioId",
                table: "CustomTimeStep");

            migrationBuilder.DropIndex(
                name: "IX_CustomTimeStep_ScenarioId",
                table: "CustomTimeStep");

            migrationBuilder.AlterColumn<long>(
                name: "ScenarioId",
                table: "CustomTimeStep",
                nullable: true,
                oldClrType: typeof(long));
        }
    }
}
