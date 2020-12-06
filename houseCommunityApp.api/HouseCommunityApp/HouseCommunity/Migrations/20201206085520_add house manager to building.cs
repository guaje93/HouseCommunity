using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class addhousemanagertobuilding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HouseManagerId",
                table: "Buildings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_HouseManagerId",
                table: "Buildings",
                column: "HouseManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Users_HouseManagerId",
                table: "Buildings",
                column: "HouseManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Users_HouseManagerId",
                table: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_HouseManagerId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "HouseManagerId",
                table: "Buildings");
        }
    }
}
