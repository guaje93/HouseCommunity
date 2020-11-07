using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class Updatepaymentdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetail_Payments_PaymentId",
                table: "PaymentDetail");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDetail_PaymentId",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "PaymentDetail");

            migrationBuilder.AddColumn<int>(
                name: "DetailsId",
                table: "Payments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DetailsId",
                table: "Payments",
                column: "DetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentDetail_DetailsId",
                table: "Payments",
                column: "DetailsId",
                principalTable: "PaymentDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentDetail_DetailsId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_DetailsId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DetailsId",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "PaymentDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetail_PaymentId",
                table: "PaymentDetail",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetail_Payments_PaymentId",
                table: "PaymentDetail",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
