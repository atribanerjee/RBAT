using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class AddZoneWeightsOffe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnualLicensedVolume",
                table: "Channel");

            migrationBuilder.AddColumn<double>(
                name: "ZoneWeightsOffset",
                table: "NodePolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneWeightsOffset",
                table: "ChannelPolicyGroup",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZoneWeightsOffset",
                table: "NodePolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneWeightsOffset",
                table: "ChannelPolicyGroup");

            migrationBuilder.AddColumn<double>(
                name: "AnualLicensedVolume",
                table: "Channel",
                nullable: true);
        }
    }
}
