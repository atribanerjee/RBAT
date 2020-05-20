using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class Channelrevised : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_UpstreamNodeID",
                table: "Channel");

            migrationBuilder.RenameColumn(
                name: "UpstreamNodeID",
                table: "Channel",
                newName: "UpstreamNodeId");

            migrationBuilder.RenameIndex(
                name: "IX_Channel_UpstreamNodeID",
                table: "Channel",
                newName: "IX_Channel_UpstreamNodeId");

            migrationBuilder.AlterColumn<int>(
                name: "UpstreamNodeId",
                table: "Channel",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "RoutingCoefficientN",
                table: "Channel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "RoutingCoefficientA",
                table: "Channel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "PercentReturnFlow",
                table: "Channel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfRoutingPhases",
                table: "Channel",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_UpstreamNodeId",
                table: "Channel",
                column: "UpstreamNodeId",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Node_UpstreamNodeId",
                table: "Channel");

            migrationBuilder.RenameColumn(
                name: "UpstreamNodeId",
                table: "Channel",
                newName: "UpstreamNodeID");

            migrationBuilder.RenameIndex(
                name: "IX_Channel_UpstreamNodeId",
                table: "Channel",
                newName: "IX_Channel_UpstreamNodeID");

            migrationBuilder.AlterColumn<int>(
                name: "UpstreamNodeID",
                table: "Channel",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RoutingCoefficientN",
                table: "Channel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RoutingCoefficientA",
                table: "Channel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PercentReturnFlow",
                table: "Channel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfRoutingPhases",
                table: "Channel",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Node_UpstreamNodeID",
                table: "Channel",
                column: "UpstreamNodeID",
                principalTable: "Node",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
