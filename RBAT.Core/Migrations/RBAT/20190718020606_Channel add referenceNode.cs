using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChanneladdreferenceNode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReferenceNodeID",
                table: "Channel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channel_ReferenceNodeID",
                table: "Channel",
                column: "ReferenceNodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_ReferenceNodeID",
                table: "Channel",
                column: "ReferenceNodeID",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_ReferenceNodeID",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_ReferenceNodeID",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "ReferenceNodeID",
                table: "Channel");
        }
    }
}
