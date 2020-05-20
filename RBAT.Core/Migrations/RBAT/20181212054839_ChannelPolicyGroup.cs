using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelPolicyGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelZoneLevel_ChannelZone_ChannelZoneID",
                table: "ChannelZoneLevel");

            migrationBuilder.DropTable(
                name: "ChannelZoneWeight");

            migrationBuilder.DropTable(
                name: "ChannelZone");

            migrationBuilder.RenameColumn(
                name: "ChannelZoneID",
                table: "ChannelZoneLevel",
                newName: "ScenarioID");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelZoneLevel_ChannelZoneID",
                table: "ChannelZoneLevel",
                newName: "IX_ChannelZoneLevel_ScenarioID");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Scenario",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Scenario",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChannelID",
                table: "ChannelZoneLevel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ChannelZoneLevel",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChannelPolicyGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    IdealZone = table.Column<double>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    NumberOfZonesAboveIdealLevel = table.Column<int>(nullable: false),
                    NumberOfZonesBelowIdealLevel = table.Column<int>(nullable: false),
                    ScenarioID = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    ZoneAboveIdeal1 = table.Column<double>(nullable: false),
                    ZoneAboveIdeal2 = table.Column<double>(nullable: false),
                    ZoneAboveIdeal3 = table.Column<double>(nullable: false),
                    ZoneAboveIdeal4 = table.Column<double>(nullable: false),
                    ZoneAboveIdeal5 = table.Column<double>(nullable: false),
                    ZoneAboveIdeal6 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal1 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal10 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal2 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal3 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal4 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal5 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal6 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal7 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal8 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal9 = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelPolicyGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelPolicyGroup_Scenario_ScenarioID",
                        column: x => x.ScenarioID,
                        principalTable: "Scenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChannelPolicyGroupChannel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelID = table.Column<int>(nullable: false),
                    ChannelPolicyGroupID = table.Column<long>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelPolicyGroupChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelPolicyGroupChannel_Channel_ChannelID",
                        column: x => x.ChannelID,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelPolicyGroupChannel_ChannelPolicyGroup_ChannelPolicyGroupID",
                        column: x => x.ChannelPolicyGroupID,
                        principalTable: "ChannelPolicyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelZoneLevel_ChannelID",
                table: "ChannelZoneLevel",
                column: "ChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPolicyGroup_ScenarioID",
                table: "ChannelPolicyGroup",
                column: "ScenarioID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPolicyGroupChannel_ChannelID",
                table: "ChannelPolicyGroupChannel",
                column: "ChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPolicyGroupChannel_ChannelPolicyGroupID",
                table: "ChannelPolicyGroupChannel",
                column: "ChannelPolicyGroupID");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelZoneLevel_Channel_ChannelID",
                table: "ChannelZoneLevel",
                column: "ChannelID",
                principalTable: "Channel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelZoneLevel_Scenario_ScenarioID",
                table: "ChannelZoneLevel",
                column: "ScenarioID",
                principalTable: "Scenario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelZoneLevel_Channel_ChannelID",
                table: "ChannelZoneLevel");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelZoneLevel_Scenario_ScenarioID",
                table: "ChannelZoneLevel");

            migrationBuilder.DropTable(
                name: "ChannelPolicyGroupChannel");

            migrationBuilder.DropTable(
                name: "ChannelPolicyGroup");

            migrationBuilder.DropIndex(
                name: "IX_ChannelZoneLevel_ChannelID",
                table: "ChannelZoneLevel");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Scenario");

            migrationBuilder.DropColumn(
                name: "ChannelID",
                table: "ChannelZoneLevel");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ChannelZoneLevel");

            migrationBuilder.RenameColumn(
                name: "ScenarioID",
                table: "ChannelZoneLevel",
                newName: "ChannelZoneID");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelZoneLevel_ScenarioID",
                table: "ChannelZoneLevel",
                newName: "IX_ChannelZoneLevel_ChannelZoneID");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Scenario",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "ChannelZone",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelID = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    NumberOfZonesAboveIdealLevel = table.Column<int>(nullable: false),
                    NumberOfZonesBelowIdealLevel = table.Column<int>(nullable: false),
                    ScenarioID = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelZone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelZone_Channel_ChannelID",
                        column: x => x.ChannelID,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelZone_Scenario_ScenarioID",
                        column: x => x.ScenarioID,
                        principalTable: "Scenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChannelZoneWeight",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelZoneID = table.Column<long>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    IdealZone = table.Column<double>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    TimeComponentType = table.Column<int>(nullable: false),
                    TimeComponentValue = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    ZoneAboveIdeal1 = table.Column<double>(nullable: false),
                    ZoneAboveIdeal2 = table.Column<double>(nullable: false),
                    ZoneAboveIdeal3 = table.Column<double>(nullable: false),
                    ZoneAboveIdeal4 = table.Column<double>(nullable: false),
                    ZoneAboveIdeal5 = table.Column<double>(nullable: false),
                    ZoneAboveIdeal6 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal1 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal10 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal2 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal3 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal4 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal5 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal6 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal7 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal8 = table.Column<double>(nullable: false),
                    ZoneBelowIdeal9 = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelZoneWeight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelZoneWeight_ChannelZone_ChannelZoneID",
                        column: x => x.ChannelZoneID,
                        principalTable: "ChannelZone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelZone_ChannelID",
                table: "ChannelZone",
                column: "ChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelZone_ScenarioID",
                table: "ChannelZone",
                column: "ScenarioID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelZoneWeight_ChannelZoneID",
                table: "ChannelZoneWeight",
                column: "ChannelZoneID");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelZoneLevel_ChannelZone_ChannelZoneID",
                table: "ChannelZoneLevel",
                column: "ChannelZoneID",
                principalTable: "ChannelZone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
