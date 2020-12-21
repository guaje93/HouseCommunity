using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class addblobfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Damages");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Damages");

            migrationBuilder.CreateTable(
                name: "BlobFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileUrl = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    DamageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlobFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlobFiles_Damages_DamageId",
                        column: x => x.DamageId,
                        principalTable: "Damages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlobFiles_DamageId",
                table: "BlobFiles",
                column: "DamageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlobFiles");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Damages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Damages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
