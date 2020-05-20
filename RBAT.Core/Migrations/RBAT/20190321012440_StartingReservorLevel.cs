using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class StartingReservorLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StartingReservoirLevel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    InitialElevation = table.Column<decimal>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    NodeId = table.Column<int>(nullable: false),
                    ScenarioId = table.Column<long>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartingReservoirLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StartingReservoirLevel_Node_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StartingReservoirLevel_Scenario_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StartingReservoirLevel_NodeId",
                table: "StartingReservoirLevel",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_StartingReservoirLevel_ScenarioId",
                table: "StartingReservoirLevel",
                column: "ScenarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StartingReservoirLevel");
        }
    }
}
