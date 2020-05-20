using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelZoneWeight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdealZone",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneAboveIdeal1",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneAboveIdeal2",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneAboveIdeal3",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneAboveIdeal4",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneAboveIdeal5",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneAboveIdeal6",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneBelowIdeal1",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneBelowIdeal10",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneBelowIdeal2",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneBelowIdeal3",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneBelowIdeal4",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneBelowIdeal5",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneBelowIdeal6",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneBelowIdeal7",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneBelowIdeal8",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ZoneBelowIdeal9",
                table: "ChannelPolicyGroup");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChannelPolicyGroup",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ChannelZoneWeight",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelPolicyGroupID = table.Column<long>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    IdealZone = table.Column<double>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    ZoneAboveIdeal1 = table.Column<double>(nullable: true),
                    ZoneAboveIdeal2 = table.Column<double>(nullable: true),
                    ZoneAboveIdeal3 = table.Column<double>(nullable: true),
                    ZoneAboveIdeal4 = table.Column<double>(nullable: true),
                    ZoneAboveIdeal5 = table.Column<double>(nullable: true),
                    ZoneAboveIdeal6 = table.Column<double>(nullable: true),
                    ZoneBelowIdeal1 = table.Column<double>(nullable: true),
                    ZoneBelowIdeal10 = table.Column<double>(nullable: true),
                    ZoneBelowIdeal2 = table.Column<double>(nullable: true),
                    ZoneBelowIdeal3 = table.Column<double>(nullable: true),
                    ZoneBelowIdeal4 = table.Column<double>(nullable: true),
                    ZoneBelowIdeal5 = table.Column<double>(nullable: true),
                    ZoneBelowIdeal6 = table.Column<double>(nullable: true),
                    ZoneBelowIdeal7 = table.Column<double>(nullable: true),
                    ZoneBelowIdeal8 = table.Column<double>(nullable: true),
                    ZoneBelowIdeal9 = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelZoneWeight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelZoneWeight_ChannelPolicyGroup_ChannelPolicyGroupID",
                        column: x => x.ChannelPolicyGroupID,
                        principalTable: "ChannelPolicyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelZoneWeight_ChannelPolicyGroupID",
                table: "ChannelZoneWeight",
                column: "ChannelPolicyGroupID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelZoneWeight");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChannelPolicyGroup",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AddColumn<double>(
                name: "IdealZone",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneAboveIdeal1",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneAboveIdeal2",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneAboveIdeal3",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneAboveIdeal4",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneAboveIdeal5",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneAboveIdeal6",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneBelowIdeal1",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneBelowIdeal10",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneBelowIdeal2",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneBelowIdeal3",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneBelowIdeal4",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneBelowIdeal5",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneBelowIdeal6",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneBelowIdeal7",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneBelowIdeal8",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ZoneBelowIdeal9",
                table: "ChannelPolicyGroup",
                nullable: true);
        }
    }
}
