using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class CustomTimeStepaddscenarioID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomTimeStep_Project_ProjectId",
                table: "CustomTimeStep");

            migrationBuilder.DropIndex(
                name: "IX_CustomTimeStep_ProjectId",
                table: "CustomTimeStep");

            migrationBuilder.AlterColumn<long>(
                name: "ProjectId",
                table: "CustomTimeStep",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<long>(
                name: "ScenarioId",
                table: "CustomTimeStep",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScenarioId",
                table: "CustomTimeStep");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "CustomTimeStep",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.CreateIndex(
                name: "IX_CustomTimeStep_ProjectId",
                table: "CustomTimeStep",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomTimeStep_Project_ProjectId",
                table: "CustomTimeStep",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
