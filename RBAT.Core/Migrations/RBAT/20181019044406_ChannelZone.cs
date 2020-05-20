using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scenario",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    ProjectID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scenario_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "ChannelZoneLevel",
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
                    table.PrimaryKey("PK_ChannelZoneLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelZoneLevel_ChannelZone_ChannelZoneID",
                        column: x => x.ChannelZoneID,
                        principalTable: "ChannelZone",
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
                name: "IX_ChannelZoneLevel_ChannelZoneID",
                table: "ChannelZoneLevel",
                column: "ChannelZoneID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelZoneWeight_ChannelZoneID",
                table: "ChannelZoneWeight",
                column: "ChannelZoneID");

            migrationBuilder.CreateIndex(
                name: "IX_Scenario_ProjectID",
                table: "Scenario",
                column: "ProjectID");          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelZoneLevel");

            migrationBuilder.DropTable(
                name: "ChannelZoneWeight");

            migrationBuilder.DropTable(
                name: "ChannelZone");

            migrationBuilder.DropTable(
                name: "Scenario");
        }
    }
}
