using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class TimeClimateData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClimateStation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClimateStation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeClimateData",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClimateStationID = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    TimeComponentType = table.Column<int>(nullable: false),
                    TimeComponentValue = table.Column<int>(nullable: false),
                    ClimateData = table.Column<double>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeClimateData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeClimateData_ClimateStation_ClimateStationID",
                        column: x => x.ClimateStationID,
                        principalTable: "ClimateStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeClimateData_ClimateStationID",
                table: "TimeClimateData",
                column: "ClimateStationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeClimateData");

            migrationBuilder.DropTable(
                name: "ClimateStation");
        }
    }
}
