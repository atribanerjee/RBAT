using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChanneladdHydroPowerPlantfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel");

            migrationBuilder.AddColumn<double>(
                name: "ConstantHeadWaterLevel",
                table: "Channel",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ConstantTailWaterLevel",
                table: "Channel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DownstreamChannelTailWaterElevationID",
                table: "Channel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DownstreamReservoirTailWaterElevationID",
                table: "Channel",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OverallHydroPowerPlantEfficiency",
                table: "Channel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpstreamChannelHeadWaterElevationID",
                table: "Channel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpstreamReservoirHeadWaterElevationID",
                table: "Channel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channel_DownstreamReservoirTailWaterElevationID",
                table: "Channel",
                column: "DownstreamReservoirTailWaterElevationID");

            migrationBuilder.CreateIndex(
                name: "IX_Channel_UpstreamReservoirHeadWaterElevationID",
                table: "Channel",
                column: "UpstreamReservoirHeadWaterElevationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_DownstreamReservoirTailWaterElevationID",
                table: "Channel",
                column: "DownstreamReservoirTailWaterElevationID",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_UpstreamReservoirHeadWaterElevationID",
                table: "Channel",
                column: "UpstreamReservoirHeadWaterElevationID",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_DownstreamReservoirTailWaterElevationID",
                table: "Channel");

            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_UpstreamReservoirHeadWaterElevationID",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_DownstreamReservoirTailWaterElevationID",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_UpstreamReservoirHeadWaterElevationID",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "ConstantHeadWaterLevel",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "ConstantTailWaterLevel",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "DownstreamChannelTailWaterElevationID",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "DownstreamReservoirTailWaterElevationID",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "OverallHydroPowerPlantEfficiency",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "UpstreamChannelHeadWaterElevationID",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "UpstreamReservoirHeadWaterElevationID",
                table: "Channel");

            migrationBuilder.CreateIndex(
                name: "IX_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel",
                column: "UpstreamChannelWithControlStructureID");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Channel_UpstreamChannelWithControlStructureID",
                table: "Channel",
                column: "UpstreamChannelWithControlStructureID",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
