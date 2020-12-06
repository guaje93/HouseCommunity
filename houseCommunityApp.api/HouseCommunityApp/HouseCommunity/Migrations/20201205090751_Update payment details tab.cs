using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class Updatepaymentdetailstab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Period",
                table: "PaymentDetail");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "PaymentDetail",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "PaymentDetail");

            migrationBuilder.AddColumn<string>(
                name: "Period",
                table: "PaymentDetail",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
