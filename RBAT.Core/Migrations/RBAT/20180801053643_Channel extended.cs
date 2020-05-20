using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class Channelextended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChannelTypeId",
                table: "Channel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRoutingPhases",
                table: "Channel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PercentReturnFlow",
                table: "Channel",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RoutingCoefficientA",
                table: "Channel",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RoutingCoefficientN",
                table: "Channel",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "RoutingOptionUse",
                table: "Channel",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ChannelType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Channel_ChannelTypeId",
                table: "Channel",
                column: "ChannelTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_ChannelType_ChannelTypeId",
                table: "Channel",
                column: "ChannelTypeId",
                principalTable: "ChannelType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_ChannelType_ChannelTypeId",
                table: "Channel");

            migrationBuilder.DropTable(
                name: "ChannelType");

            migrationBuilder.DropIndex(
                name: "IX_Channel_ChannelTypeId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "ChannelTypeId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "NumberOfRoutingPhases",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "PercentReturnFlow",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "RoutingCoefficientA",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "RoutingCoefficientN",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "RoutingOptionUse",
                table: "Channel");
        }
    }
}
