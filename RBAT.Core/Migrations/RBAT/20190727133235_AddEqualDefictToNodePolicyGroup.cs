using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class AddEqualDefictToNodePolicyGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EqualDeficits",
                table: "Node");

            migrationBuilder.AddColumn<int>(
                name: "EndTimeStep",
                table: "NodePolicyGroup",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EqualDeficits",
                table: "NodePolicyGroup",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StartTimeStep",
                table: "NodePolicyGroup",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTimeStep",
                table: "NodePolicyGroup");

            migrationBuilder.DropColumn(
                name: "EqualDeficits",
                table: "NodePolicyGroup");

            migrationBuilder.DropColumn(
                name: "StartTimeStep",
                table: "NodePolicyGroup");

            migrationBuilder.AddColumn<bool>(
                name: "EqualDeficits",
                table: "Node",
                nullable: true);
        }
    }
}
