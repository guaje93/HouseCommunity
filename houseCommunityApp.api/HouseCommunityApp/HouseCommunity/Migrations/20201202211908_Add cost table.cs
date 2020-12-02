using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class Addcosttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CostId",
                table: "Buildings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cost",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColdWaterUnitCost = table.Column<decimal>(nullable: false),
                    HotWaterUnitCost = table.Column<decimal>(nullable: false),
                    HeatingUnitCost = table.Column<decimal>(nullable: false),
                    GarbageUnitCost = table.Column<decimal>(nullable: false),
                    ExUnitCost = table.Column<decimal>(nullable: false),
                    OperatingUnitCost = table.Column<decimal>(nullable: false),
                    AdministrationUnitCost = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cost", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CostId",
                table: "Buildings",
                column: "CostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Cost_CostId",
                table: "Buildings",
                column: "CostId",
                principalTable: "Cost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Cost_CostId",
                table: "Buildings");

            migrationBuilder.DropTable(
                name: "Cost");

            migrationBuilder.DropIndex(
                name: "IX_Buildings_CostId",
                table: "Buildings");

            migrationBuilder.DropColumn(
                name: "CostId",
                table: "Buildings");
        }
    }
}
