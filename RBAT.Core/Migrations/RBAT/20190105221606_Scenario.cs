using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class Scenario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Scenario",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Scenario",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CalculationBegins",
                table: "Scenario",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CalculationEnds",
                table: "Scenario",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DataTimeStepTypeID",
                table: "Scenario",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScenarioTypeID",
                table: "Scenario",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeStepTypeID",
                table: "Scenario",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScenarioType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scenario_DataTimeStepTypeID",
                table: "Scenario",
                column: "DataTimeStepTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Scenario_ScenarioTypeID",
                table: "Scenario",
                column: "ScenarioTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Scenario_TimeStepTypeID",
                table: "Scenario",
                column: "TimeStepTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Scenario_TimeStepType_DataTimeStepTypeID",
                table: "Scenario",
                column: "DataTimeStepTypeID",
                principalTable: "TimeStepType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scenario_ScenarioType_ScenarioTypeID",
                table: "Scenario",
                column: "ScenarioTypeID",
                principalTable: "ScenarioType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scenario_TimeStepType_TimeStepTypeID",
                table: "Scenario",
                column: "TimeStepTypeID",
                principalTable: "TimeStepType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scenario_TimeStepType_DataTimeStepTypeID",
                table: "Scenario");

            migrationBuilder.DropForeignKey(
                name: "FK_Scenario_ScenarioType_ScenarioTypeID",
                table: "Scenario");

            migrationBuilder.DropForeignKey(
                name: "FK_Scenario_TimeStepType_TimeStepTypeID",
                table: "Scenario");

            migrationBuilder.DropTable(
                name: "ScenarioType");

            migrationBuilder.DropIndex(
                name: "IX_Scenario_DataTimeStepTypeID",
                table: "Scenario");

            migrationBuilder.DropIndex(
                name: "IX_Scenario_ScenarioTypeID",
                table: "Scenario");

            migrationBuilder.DropIndex(
                name: "IX_Scenario_TimeStepTypeID",
                table: "Scenario");

            migrationBuilder.DropColumn(
                name: "CalculationBegins",
                table: "Scenario");

            migrationBuilder.DropColumn(
                name: "CalculationEnds",
                table: "Scenario");

            migrationBuilder.DropColumn(
                name: "DataTimeStepTypeID",
                table: "Scenario");

            migrationBuilder.DropColumn(
                name: "ScenarioTypeID",
                table: "Scenario");

            migrationBuilder.DropColumn(
                name: "TimeStepTypeID",
                table: "Scenario");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Scenario",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Scenario",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
