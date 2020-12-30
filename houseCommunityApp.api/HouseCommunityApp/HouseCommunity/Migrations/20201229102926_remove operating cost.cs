using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class removeoperatingcost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperatingCostDescription",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "OperatingCostValue",
                table: "PaymentDetail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OperatingCostDescription",
                table: "PaymentDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OperatingCostValue",
                table: "PaymentDetail",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
