using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelZoneremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelPolicyGroup_ChannelZone_ChannelZoneID",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelZoneLevel_ChannelZone_ChannelZoneID",
                table: "ChannelZoneLevel");

            migrationBuilder.DropTable(
                name: "ChannelZone");

            migrationBuilder.RenameColumn(
                name: "ChannelZoneID",
                table: "ChannelZoneLevel",
                newName: "ChannelPolicyGroupID");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelZoneLevel_ChannelZoneID",
                table: "ChannelZoneLevel",
                newName: "IX_ChannelZoneLevel_ChannelPolicyGroupID");

            migrationBuilder.RenameColumn(
                name: "ChannelZoneID",
                table: "ChannelPolicyGroup",
                newName: "ScenarioID");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelPolicyGroup_ChannelZoneID",
                table: "ChannelPolicyGroup",
                newName: "IX_ChannelPolicyGroup_ScenarioID");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfZonesAboveIdealLevel",
                table: "ChannelPolicyGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfZonesBelowIdealLevel",
                table: "ChannelPolicyGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelPolicyGroup_Scenario_ScenarioID",
                table: "ChannelPolicyGroup",
                column: "ScenarioID",
                principalTable: "Scenario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelZoneLevel_ChannelPolicyGroup_ChannelPolicyGroupID",
                table: "ChannelZoneLevel",
                column: "ChannelPolicyGroupID",
                principalTable: "ChannelPolicyGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelPolicyGroup_Scenario_ScenarioID",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelZoneLevel_ChannelPolicyGroup_ChannelPolicyGroupID",
                table: "ChannelZoneLevel");

            migrationBuilder.DropColumn(
                name: "NumberOfZonesAboveIdealLevel",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "NumberOfZonesBelowIdealLevel",
                table: "ChannelPolicyGroup");

            migrationBuilder.RenameColumn(
                name: "ChannelPolicyGroupID",
                table: "ChannelZoneLevel",
                newName: "ChannelZoneID");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelZoneLevel_ChannelPolicyGroupID",
                table: "ChannelZoneLevel",
                newName: "IX_ChannelZoneLevel_ChannelZoneID");

            migrationBuilder.RenameColumn(
                name: "ScenarioID",
                table: "ChannelPolicyGroup",
                newName: "ChannelZoneID");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelPolicyGroup_ScenarioID",
                table: "ChannelPolicyGroup",
                newName: "IX_ChannelPolicyGroup_ChannelZoneID");

            migrationBuilder.CreateTable(
                name: "ChannelZone",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    NumberOfZonesAboveIdealLevel = table.Column<int>(nullable: false),
                    NumberOfZonesBelowIdealLevel = table.Column<int>(nullable: false),
                    ScenarioID = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelZone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelZone_Scenario_ScenarioID",
                        column: x => x.ScenarioID,
                        principalTable: "Scenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelZone_ScenarioID",
                table: "ChannelZone",
                column: "ScenarioID");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelPolicyGroup_ChannelZone_ChannelZoneID",
                table: "ChannelPolicyGroup",
                column: "ChannelZoneID",
                principalTable: "ChannelZone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
