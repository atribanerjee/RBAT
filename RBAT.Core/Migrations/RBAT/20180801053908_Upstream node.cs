using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class Upstreamnode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpstreamNodeID",
                table: "Channel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Channel_UpstreamNodeID",
                table: "Channel",
                column: "UpstreamNodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_UpstreamNodeID",
                table: "Channel",
                column: "UpstreamNodeID",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_UpstreamNodeID",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_UpstreamNodeID",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "UpstreamNodeID",
                table: "Channel");
        }
    }
}
