using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class Projectremovecalculationtimestep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_TimeStepType_TimeStepTypeID",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_TimeStepTypeID",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "TimeStepTypeID",
                table: "Project");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeStepTypeID",
                table: "Project",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_TimeStepTypeID",
                table: "Project",
                column: "TimeStepTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_TimeStepType_TimeStepTypeID",
                table: "Project",
                column: "TimeStepTypeID",
                principalTable: "TimeStepType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
