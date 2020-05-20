using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class NodeEqualDeficitsAndValuesInMillimeters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EqualDeficits",
                table: "Node",
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "ValuesInMillimeters",
                table: "Node",
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EqualDeficits",
                table: "Node");

            migrationBuilder.DropColumn(
               name: "ValuesInMillimeters",
               table: "Node");
        }
    }
}
