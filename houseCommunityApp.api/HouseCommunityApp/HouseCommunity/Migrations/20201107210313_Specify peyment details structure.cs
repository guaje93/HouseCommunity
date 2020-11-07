﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class Specifypeymentdetailsstructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "PaymentDetail");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PaymentDetail",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Period",
                table: "PaymentDetail",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PaymentDetail");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "PaymentDetail");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "PaymentDetail",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
