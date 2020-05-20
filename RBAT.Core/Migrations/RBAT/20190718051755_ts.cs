using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Channel_DownstreamChannelTailWaterElevationID",
                table: "Channel",
                column: "DownstreamChannelTailWaterElevationID");

            migrationBuilder.CreateIndex(
                name: "IX_Channel_UpstreamChannelHeadWaterElevationID",
                table: "Channel",
                column: "UpstreamChannelHeadWaterElevationID");

            migrationBuilder.CreateIndex(
                name: "IX_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel",
                column: "UpstreamChannelWithControlStructureID");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Channel_DownstreamChannelTailWaterElevationID",
                table: "Channel",
                column: "DownstreamChannelTailWaterElevationID",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Channel_UpstreamChannelHeadWaterElevationID",
                table: "Channel",
                column: "UpstreamChannelHeadWaterElevationID",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel",
                column: "UpstreamChannelWithControlStructureID",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Channel_DownstreamChannelTailWaterElevationID",
                table: "Channel");

            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Channel_UpstreamChannelHeadWaterElevationID",
                table: "Channel");

            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_DownstreamChannelTailWaterElevationID",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_UpstreamChannelHeadWaterElevationID",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel");
        }
    }
}
