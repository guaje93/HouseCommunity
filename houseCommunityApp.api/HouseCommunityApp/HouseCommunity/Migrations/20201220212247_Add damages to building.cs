using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class Adddamagestobuilding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Damages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Damages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Damages");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Damages");
        }
    }
}
