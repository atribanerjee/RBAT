using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class Node : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Node",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Node",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NodeTypeId",
                table: "Node",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "NodeType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Node_NodeTypeId",
                table: "Node",
                column: "NodeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Node_NodeType_NodeTypeId",
                table: "Node",
                column: "NodeTypeId",
                principalTable: "NodeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Node_NodeType_NodeTypeId",
                table: "Node");

            migrationBuilder.DropTable(
                name: "NodeType");

            migrationBuilder.DropIndex(
                name: "IX_Node_NodeTypeId",
                table: "Node");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Node");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Node");

            migrationBuilder.DropColumn(
                name: "NodeTypeId",
                table: "Node");
        }
    }
}
