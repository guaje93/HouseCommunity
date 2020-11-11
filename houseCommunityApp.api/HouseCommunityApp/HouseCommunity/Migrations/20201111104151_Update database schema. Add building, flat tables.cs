using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class UpdatedatabaseschemaAddbuildingflattables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HousingDevelopments_Address_AddressId",
                table: "HousingDevelopments");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaHistory_Users_UserId",
                table: "MediaHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_HousingDevelopments_HousingDevelopmentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_HousingDevelopmentId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_MediaHistory_UserId",
                table: "MediaHistory");

            migrationBuilder.DropIndex(
                name: "IX_HousingDevelopments_AddressId",
                table: "HousingDevelopments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "HousingDevelopmentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MediaHistory");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "HousingDevelopments");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "FlatId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlatId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlatId",
                table: "MediaHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Addresses",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HousingDevelopmentId = table.Column<int>(nullable: false),
                    AddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Buildings_HousingDevelopments_HousingDevelopmentId",
                        column: x => x.HousingDevelopmentId,
                        principalTable: "HousingDevelopments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Flats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlatNumber = table.Column<string>(nullable: true),
                    BuildingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flats_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_FlatId",
                table: "Users",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FlatId",
                table: "Payments",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaHistory_FlatId",
                table: "MediaHistory",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_AddressId",
                table: "Buildings",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_HousingDevelopmentId",
                table: "Buildings",
                column: "HousingDevelopmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_BuildingId",
                table: "Flats",
                column: "BuildingId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaHistory_Flats_FlatId",
                table: "MediaHistory",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Flats_FlatId",
                table: "Payments",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Flats_FlatId",
                table: "Users",
                column: "FlatId",
                principalTable: "Flats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaHistory_Flats_FlatId",
                table: "MediaHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Flats_FlatId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Flats_FlatId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Flats");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropIndex(
                name: "IX_Users_FlatId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Payments_FlatId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_MediaHistory_FlatId",
                table: "MediaHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "FlatId",
                table: "MediaHistory");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Address");

            migrationBuilder.AddColumn<int>(
                name: "HousingDevelopmentId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MediaHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "HousingDevelopments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_HousingDevelopmentId",
                table: "Users",
                column: "HousingDevelopmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaHistory_UserId",
                table: "MediaHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_HousingDevelopments_AddressId",
                table: "HousingDevelopments",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_HousingDevelopments_Address_AddressId",
                table: "HousingDevelopments",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaHistory_Users_UserId",
                table: "MediaHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_HousingDevelopments_HousingDevelopmentId",
                table: "Users",
                column: "HousingDevelopmentId",
                principalTable: "HousingDevelopments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
