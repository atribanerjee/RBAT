using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class RoutingOptionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_TimeStepType_DataStepTypeID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_TimeStepType_TimeStepTypeID",
                table: "Project");

            migrationBuilder.AlterColumn<int>(
                name: "TimeStepTypeID",
                table: "Project",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "RoutingOptionTypeID",
                table: "Project",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "DataStepTypeID",
                table: "Project",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "RoutingOptionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutingOptionType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Project_RoutingOptionTypeID",
                table: "Project",
                column: "RoutingOptionTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_TimeStepType_DataStepTypeID",
                table: "Project",
                column: "DataStepTypeID",
                principalTable: "TimeStepType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_RoutingOptionType_RoutingOptionTypeID",
                table: "Project",
                column: "RoutingOptionTypeID",
                principalTable: "RoutingOptionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_TimeStepType_TimeStepTypeID",
                table: "Project",
                column: "TimeStepTypeID",
                principalTable: "TimeStepType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_TimeStepType_DataStepTypeID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_RoutingOptionType_RoutingOptionTypeID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_TimeStepType_TimeStepTypeID",
                table: "Project");

            migrationBuilder.DropTable(
                name: "RoutingOptionType");

            migrationBuilder.DropIndex(
                name: "IX_Project_RoutingOptionTypeID",
                table: "Project");

            migrationBuilder.AlterColumn<int>(
                name: "TimeStepTypeID",
                table: "Project",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RoutingOptionTypeID",
                table: "Project",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DataStepTypeID",
                table: "Project",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_TimeStepType_DataStepTypeID",
                table: "Project",
                column: "DataStepTypeID",
                principalTable: "TimeStepType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
