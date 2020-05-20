using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelRecordedFlowStation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelRecordedFlowStation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelID = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    RecordedFlowStationID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelRecordedFlowStation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelRecordedFlowStation_Channel_ChannelID",
                        column: x => x.ChannelID,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelRecordedFlowStation_RecordedFlowStation_RecordedFlowStationID",
                        column: x => x.RecordedFlowStationID,
                        principalTable: "RecordedFlowStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRecordedFlowStation_ChannelID",
                table: "ChannelRecordedFlowStation",
                column: "ChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRecordedFlowStation_RecordedFlowStationID",
                table: "ChannelRecordedFlowStation",
                column: "RecordedFlowStationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelRecordedFlowStation");
        }
    }
}
