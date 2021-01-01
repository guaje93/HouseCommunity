using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class allowuserspossesmorethanoneflat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Flats_FlatId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FlatId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserFlats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    FlatId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFlats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFlats_Flats_FlatId",
                        column: x => x.FlatId,
                        principalTable: "Flats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFlats_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFlats_FlatId",
                table: "UserFlats",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFlats_UserId",
                table: "UserFlats",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFlats");

            migrationBuilder.AddColumn<int>(
                name: "FlatId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FlatId",
                table: "Users",
                column: "FlatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Flats_FlatId",
                table: "Users",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
