using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class AddedUserNamecolumntoBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TimeWaterUse",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TimeStorageCapacity",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TimeRecordedFlow",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TimeNaturalFlow",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TimeHistoricLevel",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TimeClimateData",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "RecordedFlowStation",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ProjectNode",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Project",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Node",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "NetEvaporation",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ClimateStation",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ChannelTravelTime",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ChannelRecordedFlowStation",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ChannelOutflowCapacity",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Channel",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TimeWaterUse");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TimeStorageCapacity");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TimeRecordedFlow");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TimeNaturalFlow");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TimeHistoricLevel");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TimeClimateData");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "RecordedFlowStation");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ProjectNode");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Node");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "NetEvaporation");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ClimateStation");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ChannelTravelTime");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ChannelRecordedFlowStation");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ChannelOutflowCapacity");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Channel");
        }
    }
}
