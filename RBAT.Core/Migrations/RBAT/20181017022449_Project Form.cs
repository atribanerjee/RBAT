using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ProjectForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CalculationBegins",
                table: "Project",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CalculationEnds",
                table: "Project",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "DataStepTypeID",
                table: "Project",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Project",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoutingOptionTypeID",
                table: "Project",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeStepTypeID",
                table: "Project",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TimeStepType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    CalculationFlag = table.Column<bool>(nullable: false),
                    DataFlag = table.Column<bool>(nullable: false)
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeStepType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_DataStepTypeID",
                table: "Project",
                column: "DataStepTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_TimeStepTypeID",
                table: "Project",
                column: "TimeStepTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_TimeStepType_DataStepTypeID",
                table: "Project",
                column: "DataStepTypeID",
                principalTable: "TimeStepType",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_TimeStepType_TimeStepTypeID",
                table: "Project",
                column: "TimeStepTypeID",
                principalTable: "TimeStepType",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_TimeStepType_DataStepTypeID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_TimeStepType_TimeStepTypeID",
                table: "Project");

            migrationBuilder.DropTable(
                name: "TimeStepType");

            migrationBuilder.DropIndex(
                name: "IX_Project_DataStepTypeID",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_TimeStepTypeID",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CalculationBegins",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CalculationEnds",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "DataStepTypeID",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "RoutingOptionTypeID",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "TimeStepTypeID",
                table: "Project");
        }
    }
}
