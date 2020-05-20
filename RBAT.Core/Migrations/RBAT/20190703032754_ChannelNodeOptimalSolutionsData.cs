using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelNodeOptimalSolutionsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelOptimalSolutionsData",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelOptimalSolutionsID = table.Column<long>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    IdealValue = table.Column<double>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false),
                    OptimalValue = table.Column<double>(nullable: true),
                    SurveyDate = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelOptimalSolutionsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelOptimalSolutionsData_ChannelOptimalSolutions_ChannelOptimalSolutionsID",
                        column: x => x.ChannelOptimalSolutionsID,
                        principalTable: "ChannelOptimalSolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NodeOptimalSolutionsData",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    IdealNetEvaporation = table.Column<double>(nullable: true),
                    IdealValue = table.Column<double>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false),
                    NodeOptimalSolutionsID = table.Column<long>(nullable: false),
                    OptimalNetEvaporation = table.Column<double>(nullable: true),
                    OptimalValue = table.Column<double>(nullable: true),
                    SurveyDate = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NodeOptimalSolutionsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NodeOptimalSolutionsData_NodeOptimalSolutions_NodeOptimalSolutionsID",
                        column: x => x.NodeOptimalSolutionsID,
                        principalTable: "NodeOptimalSolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelOptimalSolutionsData_ChannelOptimalSolutionsID",
                table: "ChannelOptimalSolutionsData",
                column: "ChannelOptimalSolutionsID");

            migrationBuilder.CreateIndex(
                name: "IX_NodeOptimalSolutionsData_NodeOptimalSolutionsID",
                table: "NodeOptimalSolutionsData",
                column: "NodeOptimalSolutionsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelOptimalSolutionsData");

            migrationBuilder.DropTable(
                name: "NodeOptimalSolutionsData");
        }
    }
}
