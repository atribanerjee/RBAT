using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ChannelPolicyGroupChannelTypeID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChannelTypeID",
                table: "ChannelPolicyGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPolicyGroup_ChannelTypeID",
                table: "ChannelPolicyGroup",
                column: "ChannelTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelPolicyGroup_ChannelType_ChannelTypeID",
                table: "ChannelPolicyGroup",
                column: "ChannelTypeID",
                principalTable: "ChannelType",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelPolicyGroup_ChannelType_ChannelTypeID",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropIndex(
                name: "IX_ChannelPolicyGroup_ChannelTypeID",
                table: "ChannelPolicyGroup");

            migrationBuilder.DropColumn(
                name: "ChannelTypeID",
                table: "ChannelPolicyGroup");
        }
    }
}
