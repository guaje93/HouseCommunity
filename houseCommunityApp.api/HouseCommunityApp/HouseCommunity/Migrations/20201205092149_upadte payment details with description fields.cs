using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class upadtepaymentdetailswithdescriptionfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "PaymentDetail");

            migrationBuilder.AddColumn<string>(
                name: "AdministrationDescription",
                table: "PaymentDetail",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AdministrationValue",
                table: "PaymentDetail",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ColdWaterDescription",
                table: "PaymentDetail",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ColdWaterValue",
                table: "PaymentDetail",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "GarbageDescription",
                table: "PaymentDetail",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "GarbageValue",
                table: "PaymentDetail",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "HeatingDescription",
                table: "PaymentDetail",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeatingRefundDescription",
                table: "PaymentDetail",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HeatingRefundValue",
                table: "PaymentDetail",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HeatingValue",
                table: "PaymentDetail",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "HotWaterDescription",
                table: "PaymentDetail",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HotWaterValue",
                table: "PaymentDetail",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "OperatingCostDescription",
                table: "PaymentDetail",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OperatingCostValue",
                table: "PaymentDetail",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "WaterRefundDescription",
                table: "PaymentDetail",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "WaterRefundValue",
                table: "PaymentDetail",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdministrationDescription",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "AdministrationValue",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "ColdWaterDescription",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "ColdWaterValue",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "GarbageDescription",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "GarbageValue",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "HeatingDescription",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "HeatingRefundDescription",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "HeatingRefundValue",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "HeatingValue",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "HotWaterDescription",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "HotWaterValue",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "OperatingCostDescription",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "OperatingCostValue",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "WaterRefundDescription",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "WaterRefundValue",
                table: "PaymentDetail");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PaymentDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PaymentDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "PaymentDetail",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
