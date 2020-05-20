using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class Channelchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_DownstreamNodeId",
                table: "Channel");

            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_UpstreamNodeId",
                table: "Channel");

            migrationBuilder.RenameColumn(
                name: "UpstreamNodeId",
                table: "Channel",
                newName: "UpstreamNodeID");

            migrationBuilder.RenameColumn(
                name: "DownstreamNodeId",
                table: "Channel",
                newName: "DownstreamNodeID");

            migrationBuilder.RenameIndex(
                name: "IX_Channel_UpstreamNodeId",
                table: "Channel",
                newName: "IX_Channel_UpstreamNodeID");

            migrationBuilder.RenameIndex(
                name: "IX_Channel_DownstreamNodeId",
                table: "Channel",
                newName: "IX_Channel_DownstreamNodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_DownstreamNodeID",
                table: "Channel",
                column: "DownstreamNodeID",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_UpstreamNodeID",
                table: "Channel",
                column: "UpstreamNodeID",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_DownstreamNodeID",
                table: "Channel");

            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_UpstreamNodeID",
                table: "Channel");

            migrationBuilder.RenameColumn(
                name: "UpstreamNodeID",
                table: "Channel",
                newName: "UpstreamNodeId");

            migrationBuilder.RenameColumn(
                name: "DownstreamNodeID",
                table: "Channel",
                newName: "DownstreamNodeId");

            migrationBuilder.RenameIndex(
                name: "IX_Channel_UpstreamNodeID",
                table: "Channel",
                newName: "IX_Channel_UpstreamNodeId");

            migrationBuilder.RenameIndex(
                name: "IX_Channel_DownstreamNodeID",
                table: "Channel",
                newName: "IX_Channel_DownstreamNodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_DownstreamNodeId",
                table: "Channel",
                column: "DownstreamNodeId",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_UpstreamNodeId",
                table: "Channel",
                column: "UpstreamNodeId",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
