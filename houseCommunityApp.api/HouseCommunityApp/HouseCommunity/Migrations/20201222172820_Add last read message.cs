using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class Addlastreadmessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastMessageReadId",
                table: "UserConversations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserConversations_LastMessageReadId",
                table: "UserConversations",
                column: "LastMessageReadId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_Messages_LastMessageReadId",
                table: "UserConversations",
                column: "LastMessageReadId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_Messages_LastMessageReadId",
                table: "UserConversations");

            migrationBuilder.DropIndex(
                name: "IX_UserConversations_LastMessageReadId",
                table: "UserConversations");

            migrationBuilder.DropColumn(
                name: "LastMessageReadId",
                table: "UserConversations");
        }
    }
}
