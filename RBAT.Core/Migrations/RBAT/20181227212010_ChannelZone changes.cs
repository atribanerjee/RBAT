using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelZonechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelZone_ChannelZone_ChannelZoneId",
                table: "ChannelZone");

            migrationBuilder.DropIndex(
                name: "IX_ChannelZone_ChannelZoneId",
                table: "ChannelZone");

            migrationBuilder.DropColumn(
                name: "ChannelZoneId",
                table: "ChannelZone");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ChannelZoneId",
                table: "ChannelZone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelZone_ChannelZoneId",
                table: "ChannelZone",
                column: "ChannelZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelZone_ChannelZone_ChannelZoneId",
                table: "ChannelZone",
                column: "ChannelZoneId",
                principalTable: "ChannelZone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
