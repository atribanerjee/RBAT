using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class AddMeasuringUnittotheNode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValuesInMillimeters",
                table: "Node");

            migrationBuilder.AlterColumn<bool>(
                name: "EqualDeficits",
                table: "Node",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<int>(
                name: "MeasuringUnitId",
                table: "Node",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MeasuringUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasuringUnit", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Node_MeasuringUnitId",
                table: "Node",
                column: "MeasuringUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Node_MeasuringUnit_MeasuringUnitId",
                table: "Node",
                column: "MeasuringUnitId",
                principalTable: "MeasuringUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Node_MeasuringUnit_MeasuringUnitId",
                table: "Node");

            migrationBuilder.DropTable(
                name: "MeasuringUnit");

            migrationBuilder.DropIndex(
                name: "IX_Node_MeasuringUnitId",
                table: "Node");

            migrationBuilder.DropColumn(
                name: "MeasuringUnitId",
                table: "Node");

            migrationBuilder.AlterColumn<bool>(
                name: "EqualDeficits",
                table: "Node",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ValuesInMillimeters",
                table: "Node",
                nullable: false,
                defaultValue: false);
        }
    }
}
