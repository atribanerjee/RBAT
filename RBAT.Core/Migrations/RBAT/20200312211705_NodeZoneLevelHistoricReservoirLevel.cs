using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class NodeZoneLevelHistoricReservoirLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NodeZoneLevelHistoricReservoirLevel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NodePolicyGroupId = table.Column<long>(nullable: false),
                    NodeId = table.Column<int>(nullable: false),
                    UseHistoricReservoirLevels = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeZoneLevelHistoricReservoirLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeZoneLevelHistoricReservoirLevel_Node_NodeId",
                        column: x => x.NodeId,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NodeZoneLevelHistoricReservoirLevel_NodePolicyGroup_NodePolicyGroupId",
                        column: x => x.NodePolicyGroupId,
                        principalTable: "NodePolicyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NodeZoneLevelHistoricReservoirLevel_NodeId",
                table: "NodeZoneLevelHistoricReservoirLevel",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_NodeZoneLevelHistoricReservoirLevel_NodePolicyGroupId",
                table: "NodeZoneLevelHistoricReservoirLevel",
                column: "NodePolicyGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NodeZoneLevelHistoricReservoirLevel");
        }
    }
}
