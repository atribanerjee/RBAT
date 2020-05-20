using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelZone2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelPolicyGroup_Scenario_ScenarioId",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropIndex(
                name: "IX_ChannelPolicyGroup_ScenarioId",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ScenarioId",
                table: "ChannelPolicyGroup");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ScenarioId",
                table: "ChannelPolicyGroup",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPolicyGroup_ScenarioId",
                table: "ChannelPolicyGroup",
                column: "ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelPolicyGroup_Scenario_ScenarioId",
                table: "ChannelPolicyGroup",
                column: "ScenarioId",
                principalTable: "Scenario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
