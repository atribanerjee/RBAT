using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class NullbleChannelZons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal9",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal8",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal7",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal6",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal5",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal4",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal3",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal2",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal10",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal1",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal6",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal5",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal4",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal3",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal2",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal1",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "IdealZone",
                table: "ChannelZoneLevel",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal9",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal8",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal7",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal6",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal5",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal4",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal3",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal2",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal10",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal1",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal6",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal5",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal4",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal3",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal2",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal1",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "IdealZone",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal9",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal8",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal7",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal6",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal5",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal4",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal3",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal2",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal10",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal1",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal6",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal5",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal4",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal3",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal2",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal1",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "IdealZone",
                table: "ChannelZoneLevel",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal9",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal8",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal7",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal6",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal5",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal4",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal3",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal2",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal10",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneBelowIdeal1",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal6",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal5",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal4",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal3",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal2",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ZoneAboveIdeal1",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "IdealZone",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
