using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class UpdateChannelNodeMapDataStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update [Node] set MapData = CONCAT('{\"mapPosition\":', MapData, '}' ) " +
                "where NodeTypeId <> 3 and MapData is not null and MapData not like ''");

            migrationBuilder.Sql("update [Channel] set MapData = CONCAT('{\"icons\":[],\"mapPosition\":', MapData, '}') " +
                "where MapData is not null and MapData not like ''");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update [Node] set MapData = SUBSTRING(MapData, 16, DATALENGTH(MapData) - 16) " +
               "where NodeTypeId <> 3 and MapData is not null and MapData not like ''");

            migrationBuilder.Sql("update [Channel] set MapData = SUBSTRING(MapData, 27, DATALENGTH(MapData) - 27) " +
                "where MapData is not null and MapData not like ''");
        }
    }
}
