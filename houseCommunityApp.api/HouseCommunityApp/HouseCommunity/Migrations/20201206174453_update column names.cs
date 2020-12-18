using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class updatecolumnnames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColdWaterEstimatedUsageForOneHuman",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "HeatingEstimatedUsageForOneHuman",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "HotWaterEstimatedUsageForOneHuman",
                table: "Flats");

            migrationBuilder.AddColumn<double>(
                name: "ColdWaterEstimatedUsage",
                table: "Flats",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HeatingEstimatedUsage",
                table: "Flats",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HotWaterEstimatedUsage",
                table: "Flats",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColdWaterEstimatedUsage",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "HeatingEstimatedUsage",
                table: "Flats");

            migrationBuilder.DropColumn(
                name: "HotWaterEstimatedUsage",
                table: "Flats");

            migrationBuilder.AddColumn<double>(
                name: "ColdWaterEstimatedUsageForOneHuman",
                table: "Flats",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HeatingEstimatedUsageForOneHuman",
                table: "Flats",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HotWaterEstimatedUsageForOneHuman",
                table: "Flats",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
