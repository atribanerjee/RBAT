using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class TimeHistoricLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeHistoricLevel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    NodeID = table.Column<int>(nullable: false),
                    TimeComponentType = table.Column<int>(nullable: false),
                    TimeComponentValue = table.Column<int>(nullable: false),
                    Elevation = table.Column<double>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeHistoricLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeHistoricLevel_Node_NodeID",
                        column: x => x.NodeID,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeHistoricLevel_NodeID",
                table: "TimeHistoricLevel",
                column: "NodeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeHistoricLevel");
        }
    }
}
