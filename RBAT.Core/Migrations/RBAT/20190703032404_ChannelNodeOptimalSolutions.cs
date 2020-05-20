using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelNodeOptimalSolutions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelOptimalSolutions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelID = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    ProjectID = table.Column<int>(nullable: false),
                    ScenarioID = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelOptimalSolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelOptimalSolutions_Channel_ChannelID",
                        column: x => x.ChannelID,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelOptimalSolutions_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelOptimalSolutions_Scenario_ScenarioID",
                        column: x => x.ScenarioID,
                        principalTable: "Scenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NodeOptimalSolutions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    NodeID = table.Column<int>(nullable: false),
                    ProjectID = table.Column<int>(nullable: false),
                    ScenarioID = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeOptimalSolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeOptimalSolutions_Node_NodeID",
                        column: x => x.NodeID,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodeOptimalSolutions_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodeOptimalSolutions_Scenario_ScenarioID",
                        column: x => x.ScenarioID,
                        principalTable: "Scenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelOptimalSolutions_ChannelID",
                table: "ChannelOptimalSolutions",
                column: "ChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelOptimalSolutions_ProjectID",
                table: "ChannelOptimalSolutions",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelOptimalSolutions_ScenarioID",
                table: "ChannelOptimalSolutions",
                column: "ScenarioID");

            migrationBuilder.CreateIndex(
                name: "IX_NodeOptimalSolutions_NodeID",
                table: "NodeOptimalSolutions",
                column: "NodeID");

            migrationBuilder.CreateIndex(
                name: "IX_NodeOptimalSolutions_ProjectID",
                table: "NodeOptimalSolutions",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_NodeOptimalSolutions_ScenarioID",
                table: "NodeOptimalSolutions",
                column: "ScenarioID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelOptimalSolutions");

            migrationBuilder.DropTable(
                name: "NodeOptimalSolutions");
        }
    }
}
