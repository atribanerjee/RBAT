using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class NodeZoneLevelWeights : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NodePolicyGroup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    NodeTypeID = table.Column<int>(nullable: false),
                    NumberOfZonesAboveIdealLevel = table.Column<int>(nullable: false),
                    NumberOfZonesBelowIdealLevel = table.Column<int>(nullable: false),
                    ScenarioID = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodePolicyGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodePolicyGroup_NodeType_NodeTypeID",
                        column: x => x.NodeTypeID,
                        principalTable: "NodeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodePolicyGroup_Scenario_ScenarioID",
                        column: x => x.ScenarioID,
                        principalTable: "Scenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NodePolicyGroupNode",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    NodeID = table.Column<int>(nullable: false),
                    NodePolicyGroupID = table.Column<long>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodePolicyGroupNode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodePolicyGroupNode_Node_NodeID",
                        column: x => x.NodeID,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodePolicyGroupNode_NodePolicyGroup_NodePolicyGroupID",
                        column: x => x.NodePolicyGroupID,
                        principalTable: "NodePolicyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "NodeZoneLevel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    NodeID = table.Column<int>(nullable: false),
                    NodePolicyGroupID = table.Column<long>(nullable: false),
                    TimeComponentType = table.Column<int>(nullable: false),
                    TimeComponentValue = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    Year = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_NodeZoneLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeZoneLevel_Node_NodeID",
                        column: x => x.NodeID,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodeZoneLevel_NodePolicyGroup_NodePolicyGroupID",
                        column: x => x.NodePolicyGroupID,
                        principalTable: "NodePolicyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "NodeZoneWeight",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    NodePolicyGroupID = table.Column<long>(nullable: false),
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
                    table.PrimaryKey("PK_NodeZoneWeight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeZoneWeight_NodePolicyGroup_NodePolicyGroupID",
                        column: x => x.NodePolicyGroupID,
                        principalTable: "NodePolicyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NodePolicyGroup_NodeTypeID",
                table: "NodePolicyGroup",
                column: "NodeTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_NodePolicyGroup_ScenarioID",
                table: "NodePolicyGroup",
                column: "ScenarioID");

            migrationBuilder.CreateIndex(
                name: "IX_NodePolicyGroupNode_NodeID",
                table: "NodePolicyGroupNode",
                column: "NodeID");

            migrationBuilder.CreateIndex(
                name: "IX_NodePolicyGroupNode_NodePolicyGroupID",
                table: "NodePolicyGroupNode",
                column: "NodePolicyGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_NodeZoneLevel_NodeID",
                table: "NodeZoneLevel",
                column: "NodeID");

            migrationBuilder.CreateIndex(
                name: "IX_NodeZoneLevel_NodePolicyGroupID",
                table: "NodeZoneLevel",
                column: "NodePolicyGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_NodeZoneWeight_NodePolicyGroupID",
                table: "NodeZoneWeight",
                column: "NodePolicyGroupID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NodePolicyGroupNode");

            migrationBuilder.DropTable(
                name: "NodeZoneLevel");

            migrationBuilder.DropTable(
                name: "NodeZoneWeight");

            migrationBuilder.DropTable(
                name: "NodePolicyGroup");
        }
    }
}
