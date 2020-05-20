using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelZoneLevelRFS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelZoneLevelRecordedFlowStation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    ChannelPolicyGroupId = table.Column<long>(nullable: false),
                    ChannelId = table.Column<int>(nullable: false),
                    Zone1Id = table.Column<int>(nullable: true),
                    Zone2Id = table.Column<int>(nullable: true),
                    Zone3Id = table.Column<int>(nullable: true),
                    RecordedFlowStation1Id = table.Column<int>(nullable: true),
                    RecordedFlowStation2Id = table.Column<int>(nullable: true),
                    RecordedFlowStation3Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelZoneLevelRecordedFlowStation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelZoneLevelRecordedFlowStation_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelZoneLevelRecordedFlowStation_ChannelPolicyGroup_ChannelPolicyGroupId",
                        column: x => x.ChannelPolicyGroupId,
                        principalTable: "ChannelPolicyGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelZoneLevelRecordedFlowStation_ChannelId",
                table: "ChannelZoneLevelRecordedFlowStation",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelZoneLevelRecordedFlowStation_ChannelPolicyGroupId",
                table: "ChannelZoneLevelRecordedFlowStation",
                column: "ChannelPolicyGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelZoneLevelRecordedFlowStation");
        }
    }
}
