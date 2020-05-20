using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class Channeldownstreamnode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DownstreamNodeId",
                table: "Channel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channel_DownstreamNodeId",
                table: "Channel",
                column: "DownstreamNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_DownstreamNodeId",
                table: "Channel",
                column: "DownstreamNodeId",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_DownstreamNodeId",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_DownstreamNodeId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "DownstreamNodeId",
                table: "Channel");
        }
    }
}
