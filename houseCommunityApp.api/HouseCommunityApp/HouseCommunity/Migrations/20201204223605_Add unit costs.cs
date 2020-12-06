using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class Addunitcosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExUnitCost",
                table: "Cost");

            migrationBuilder.AddColumn<double>(
                name: "ColdWaterEstimatedUsageForOneHuman",
                table: "Flats",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HeatingEstimatedUsageForOneHuman",
                table: "Flats",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HotWaterEstimatedUsageForOneHuman",
                table: "Flats",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "OperatingUnitCost",
                table: "Cost",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "HotWaterUnitCost",
                table: "Cost",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "HeatingUnitCost",
                table: "Cost",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "GarbageUnitCost",
                table: "Cost",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "ColdWaterUnitCost",
                table: "Cost",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "AdministrationUnitCost",
                table: "Cost",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<double>(
                name: "ColdWaterEstimatedUsageForOneHuman",
                table: "Buildings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HeatingEstimatedUsageForOneHuman",
                table: "Buildings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HotWaterEstimatedUsageForOneHuman",
                table: "Buildings",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ColdWaterEstimatedUsageForOneHuman",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "HeatingEstimatedUsageForOneHuman",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "HotWaterEstimatedUsageForOneHuman",
                table: "Buildings");

            migrationBuilder.AlterColumn<decimal>(
                name: "OperatingUnitCost",
                table: "Cost",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "HotWaterUnitCost",
                table: "Cost",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "HeatingUnitCost",
                table: "Cost",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "GarbageUnitCost",
                table: "Cost",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "ColdWaterUnitCost",
                table: "Cost",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "AdministrationUnitCost",
                table: "Cost",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<decimal>(
                name: "ExUnitCost",
                table: "Cost",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
