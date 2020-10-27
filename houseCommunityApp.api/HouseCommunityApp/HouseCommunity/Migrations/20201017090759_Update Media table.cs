using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class UpdateMediatable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_Users_UserId",
                table: "Media");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Media",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "ColdWaterState",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "HotWaterState",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "MasurmentDate",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Media");

            migrationBuilder.RenameTable(
                name: "Media",
                newName: "MediaHistory");

            migrationBuilder.RenameIndex(
                name: "IX_Media_UserId",
                table: "MediaHistory",
                newName: "IX_MediaHistory_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptanceDate",
                table: "MediaHistory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "MediaHistory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "MediaHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "MediaHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MediaType",
                table: "MediaHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserDescription",
                table: "MediaHistory",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaHistory",
                table: "MediaHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaHistory_Users_UserId",
                table: "MediaHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaHistory_Users_UserId",
                table: "MediaHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaHistory",
                table: "MediaHistory");

            migrationBuilder.DropColumn(
                name: "AcceptanceDate",
                table: "MediaHistory");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "MediaHistory");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "MediaHistory");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "MediaHistory");

            migrationBuilder.DropColumn(
                name: "MediaType",
                table: "MediaHistory");

            migrationBuilder.DropColumn(
                name: "UserDescription",
                table: "MediaHistory");

            migrationBuilder.RenameTable(
                name: "MediaHistory",
                newName: "Media");

            migrationBuilder.RenameIndex(
                name: "IX_MediaHistory_UserId",
                table: "Media",
                newName: "IX_Media_UserId");

            migrationBuilder.AddColumn<double>(
                name: "ColdWaterState",
                table: "Media",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HotWaterState",
                table: "Media",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "MasurmentDate",
                table: "Media",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Media",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Media",
                table: "Media",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_Users_UserId",
                table: "Media",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
