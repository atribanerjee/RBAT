﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class NodeNewFieldInitialElevation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "InitialElevation",
                table: "Node",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialElevation",
                table: "Node");
        }
    }
}
