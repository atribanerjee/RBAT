using Microsoft.EntityFrameworkCore.Migrations;

namespace RBAT.Core.Migrations.RBAT
{
    public partial class ScenarioAddNumberOfLookheadTimeSteps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfLookaheadTimeSteps",
                table: "Scenario",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfLookaheadTimeSteps",
                table: "Scenario");
        }
    }
}
