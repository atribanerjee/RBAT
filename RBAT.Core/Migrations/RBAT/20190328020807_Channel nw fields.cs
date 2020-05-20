using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class Channelnwfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AnualLicensedVolume",
                table: "Channel",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalLicensedVolume",
                table: "Channel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnualLicensedVolume",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "TotalLicensedVolume",
                table: "Channel");
        }
    }
}
