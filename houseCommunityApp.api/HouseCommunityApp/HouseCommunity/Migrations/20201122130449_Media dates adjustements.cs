using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class Mediadatesadjustements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "MediaHistory");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "MediaHistory");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndPeriodDate",
                table: "MediaHistory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartPeriodDate",
                table: "MediaHistory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndPeriodDate",
                table: "MediaHistory");

            migrationBuilder.DropColumn(
                name: "StartPeriodDate",
                table: "MediaHistory");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "MediaHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Period",
                table: "MediaHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
