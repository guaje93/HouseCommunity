using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class Addchat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SenderId = table.Column<int>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    ConversationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ConversationId",
                table: "Users",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MessageId",
                table: "Users",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Conversations_ConversationId",
                table: "Users",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Messages_MessageId",
                table: "Users",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Conversations_ConversationId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Messages_MessageId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Users_ConversationId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_MessageId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "Users");
        }
    }
}
