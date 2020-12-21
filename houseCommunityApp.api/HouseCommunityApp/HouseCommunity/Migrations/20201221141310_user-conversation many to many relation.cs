using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class userconversationmanytomanyrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Conversations_ConversationId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Messages_MessageId",
                table: "Users");

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

            migrationBuilder.CreateTable(
                name: "UserConversation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    ConversationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserConversation_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserConversation_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserConversation_ConversationId",
                table: "UserConversation",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConversation_UserId",
                table: "UserConversation",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserConversation");

            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ConversationId",
                table: "Users",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MessageId",
                table: "Users",
                column: "MessageId");

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
    }
}
