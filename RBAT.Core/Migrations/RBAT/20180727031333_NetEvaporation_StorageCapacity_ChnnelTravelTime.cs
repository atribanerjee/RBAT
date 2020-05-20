using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class NetEvaporation_StorageCapacity_ChnnelTravelTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Modified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NetEvaporation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdjustmentFactor = table.Column<double>(nullable: false),
                    ClimateStationID = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    NodeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetEvaporation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetEvaporation_ClimateStation_ClimateStationID",
                        column: x => x.ClimateStationID,
                        principalTable: "ClimateStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NetEvaporation_Node_NodeID",
                        column: x => x.NodeID,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeStorageCapacity",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Area = table.Column<double>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Elevation = table.Column<double>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    NodeID = table.Column<int>(nullable: false),
                    SurveyDate = table.Column<DateTime>(nullable: false),
                    Volume = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeStorageCapacity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeStorageCapacity_Node_NodeID",
                        column: x => x.NodeID,
                        principalTable: "Node",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChannelTravelTime",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelID = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Flow = table.Column<double>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    TravelTime = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelTravelTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelTravelTime_Channel_ChannelID",
                        column: x => x.ChannelID,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelTravelTime_ChannelID",
                table: "ChannelTravelTime",
                column: "ChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_NetEvaporation_ClimateStationID",
                table: "NetEvaporation",
                column: "ClimateStationID");

            migrationBuilder.CreateIndex(
                name: "IX_NetEvaporation_NodeID",
                table: "NetEvaporation",
                column: "NodeID");

            migrationBuilder.CreateIndex(
                name: "IX_TimeStorageCapacity_NodeID",
                table: "TimeStorageCapacity",
                column: "NodeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelTravelTime");

            migrationBuilder.DropTable(
                name: "NetEvaporation");

            migrationBuilder.DropTable(
                name: "TimeStorageCapacity");

            migrationBuilder.DropTable(
                name: "Channel");
        }
    }
}
