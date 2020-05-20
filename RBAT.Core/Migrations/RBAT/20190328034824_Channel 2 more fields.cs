using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class Channel2morefields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UpstreamChannelWithControlStructureID",
                table: "Channel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpstreamNodeWithControlStructureID",
                table: "Channel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel",
                column: "UpstreamChannelWithControlStructureID");

            migrationBuilder.CreateIndex(
                name: "IX_Channel_UpstreamNodeWithControlStructureID",
                table: "Channel",
                column: "UpstreamNodeWithControlStructureID");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel",
                column: "UpstreamChannelWithControlStructureID",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_UpstreamNodeWithControlStructureID",
                table: "Channel",
                column: "UpstreamNodeWithControlStructureID",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel");

            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_UpstreamNodeWithControlStructureID",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_UpstreamNodeWithControlStructureID",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "UpstreamChannelWithControlStructureID",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "UpstreamNodeWithControlStructureID",
                table: "Channel");
        }
    }
}
