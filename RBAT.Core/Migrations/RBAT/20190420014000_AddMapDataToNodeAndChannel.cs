using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class AddMapDataToNodeAndChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "MapData",
               table: "Node",
               maxLength: 2000,
               nullable: true);

            migrationBuilder.AddColumn<string>(
               name: "MapData",
               table: "Channel",
               maxLength: 2000,
               nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "MapData",
               table: "Node");

            migrationBuilder.DropColumn(
               name: "MapData",
               table: "Channel");
        }
    }
}
