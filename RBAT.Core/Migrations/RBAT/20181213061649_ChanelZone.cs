using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChanelZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelPolicyGroup_Scenario_ScenarioID",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelZoneLevel_Scenario_ScenarioID",
                table: "ChannelZoneLevel");

            migrationBuilder.DropColumn(
                name: "NumberOfZonesAboveIdealLevel",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "NumberOfZonesBelowIdealLevel",
                table: "ChannelPolicyGroup");

            migrationBuilder.RenameColumn(
                name: "ScenarioID",
                table: "ChannelZoneLevel",
                newName: "ChannelZoneID");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelZoneLevel_ScenarioID",
                table: "ChannelZoneLevel",
                newName: "IX_ChannelZoneLevel_ChannelZoneID");

            migrationBuilder.RenameColumn(
                name: "ScenarioID",
                table: "ChannelPolicyGroup",
                newName: "ScenarioId");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelPolicyGroup_ScenarioID",
                table: "ChannelPolicyGroup",
                newName: "IX_ChannelPolicyGroup_ScenarioId");

            migrationBuilder.AlterColumn<long>(
                name: "ScenarioId",
                table: "ChannelPolicyGroup",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "ChannelZoneID",
                table: "ChannelPolicyGroup",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ChannelZone",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelZoneId = table.Column<long>(nullable: true),
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
                        name: "FK_ChannelZone_ChannelZone_ChannelZoneId",
                        column: x => x.ChannelZoneId,
                        principalTable: "ChannelZone",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChannelZone_Scenario_ScenarioID",
                        column: x => x.ScenarioID,
                        principalTable: "Scenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPolicyGroup_ChannelZoneID",
                table: "ChannelPolicyGroup",
                column: "ChannelZoneID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelZone_ChannelZoneId",
                table: "ChannelZone",
                column: "ChannelZoneId");

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
                name: "FK_ChannelPolicyGroup_Scenario_ScenarioId",
                table: "ChannelPolicyGroup",
                column: "ScenarioId",
                principalTable: "Scenario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelZoneLevel_ChannelZone_ChannelZoneID",
                table: "ChannelZoneLevel",
                column: "ChannelZoneID",
                principalTable: "ChannelZone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelPolicyGroup_ChannelZone_ChannelZoneID",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelPolicyGroup_Scenario_ScenarioId",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelZoneLevel_ChannelZone_ChannelZoneID",
                table: "ChannelZoneLevel");

            migrationBuilder.DropTable(
                name: "ChannelZone");

            migrationBuilder.DropIndex(
                name: "IX_ChannelPolicyGroup_ChannelZoneID",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ChannelZoneID",
                table: "ChannelPolicyGroup");

            migrationBuilder.RenameColumn(
                name: "ChannelZoneID",
                table: "ChannelZoneLevel",
                newName: "ScenarioID");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelZoneLevel_ChannelZoneID",
                table: "ChannelZoneLevel",
                newName: "IX_ChannelZoneLevel_ScenarioID");

            migrationBuilder.RenameColumn(
                name: "ScenarioId",
                table: "ChannelPolicyGroup",
                newName: "ScenarioID");

            migrationBuilder.RenameIndex(
                name: "IX_ChannelPolicyGroup_ScenarioId",
                table: "ChannelPolicyGroup",
                newName: "IX_ChannelPolicyGroup_ScenarioID");

            migrationBuilder.AlterColumn<long>(
                name: "ScenarioID",
                table: "ChannelPolicyGroup",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

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
                name: "FK_ChannelZoneLevel_Scenario_ScenarioID",
                table: "ChannelZoneLevel",
                column: "ScenarioID",
                principalTable: "Scenario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
