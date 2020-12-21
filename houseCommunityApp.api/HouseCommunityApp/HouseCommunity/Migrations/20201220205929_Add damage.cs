using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class Adddamage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Damages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    RequestCreatorId = table.Column<int>(nullable: true),
                    BuildingId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Damages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Damages_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Damages_Users_RequestCreatorId",
                        column: x => x.RequestCreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Damages_BuildingId",
                table: "Damages",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Damages_RequestCreatorId",
                table: "Damages",
                column: "RequestCreatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Damages");
        }
    }
}
