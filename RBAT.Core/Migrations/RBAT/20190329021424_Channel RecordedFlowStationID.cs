using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelRecordedFlowStationID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecordedFlowStationID",
                table: "Channel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channel_RecordedFlowStationID",
                table: "Channel",
                column: "RecordedFlowStationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_RecordedFlowStation_RecordedFlowStationID",
                table: "Channel",
                column: "RecordedFlowStationID",
                principalTable: "RecordedFlowStation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_RecordedFlowStation_RecordedFlowStationID",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_RecordedFlowStationID",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "RecordedFlowStationID",
                table: "Channel");
        }
    }
}
