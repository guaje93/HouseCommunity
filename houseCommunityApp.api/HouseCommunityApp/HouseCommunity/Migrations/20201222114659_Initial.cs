using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HouseCommunity.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });

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
                name: "Cost",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColdWaterUnitCost = table.Column<double>(nullable: false),
                    HotWaterUnitCost = table.Column<double>(nullable: false),
                    HeatingUnitCost = table.Column<double>(nullable: false),
                    GarbageUnitCost = table.Column<double>(nullable: false),
                    OperatingUnitCost = table.Column<double>(nullable: false),
                    AdministrationUnitCost = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HousingDevelopments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousingDevelopments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotWaterValue = table.Column<double>(nullable: false),
                    ColdWaterValue = table.Column<double>(nullable: false),
                    HeatingValue = table.Column<double>(nullable: false),
                    AdministrationValue = table.Column<double>(nullable: false),
                    OperatingCostValue = table.Column<double>(nullable: false),
                    HeatingRefundValue = table.Column<double>(nullable: false),
                    GarbageValue = table.Column<double>(nullable: false),
                    WaterRefundValue = table.Column<double>(nullable: false),
                    HeatingDescription = table.Column<string>(nullable: true),
                    ColdWaterDescription = table.Column<string>(nullable: true),
                    GarbageDescription = table.Column<string>(nullable: true),
                    HotWaterDescription = table.Column<string>(nullable: true),
                    AdministrationDescription = table.Column<string>(nullable: true),
                    OperatingCostDescription = table.Column<string>(nullable: true),
                    HeatingRefundDescription = table.Column<string>(nullable: true),
                    WaterRefundDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HousingDevelopmentId = table.Column<int>(nullable: false),
                    AddressId = table.Column<int>(nullable: false),
                    CostId = table.Column<int>(nullable: true),
                    HouseManagerId = table.Column<int>(nullable: true),
                    HeatingEstimatedUsageForOneHuman = table.Column<double>(nullable: false),
                    ColdWaterEstimatedUsageForOneHuman = table.Column<double>(nullable: false),
                    HotWaterEstimatedUsageForOneHuman = table.Column<double>(nullable: false)
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
                        name: "FK_Buildings_Cost_CostId",
                        column: x => x.CostId,
                        principalTable: "Cost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Area = table.Column<double>(nullable: false),
                    BuildingId = table.Column<int>(nullable: false),
                    ResidentsAmount = table.Column<int>(nullable: false),
                    HeatingEstimatedUsage = table.Column<double>(nullable: false),
                    ColdWaterEstimatedUsage = table.Column<double>(nullable: false),
                    HotWaterEstimatedUsage = table.Column<double>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "MediaHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    UserDescription = table.Column<string>(nullable: true),
                    LastValue = table.Column<double>(nullable: false),
                    CurrentValue = table.Column<double>(nullable: false),
                    MediaType = table.Column<int>(nullable: false),
                    StartPeriodDate = table.Column<DateTime>(nullable: false),
                    EndPeriodDate = table.Column<DateTime>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    AcceptanceDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    FlatId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaHistory_Flats_FlatId",
                        column: x => x.FlatId,
                        principalTable: "Flats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Value = table.Column<double>(nullable: false),
                    DetailsId = table.Column<int>(nullable: true),
                    PaymentDeadline = table.Column<DateTime>(nullable: false),
                    PaymentBookDate = table.Column<DateTime>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    FlatId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    OrderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentDetail_DetailsId",
                        column: x => x.DetailsId,
                        principalTable: "PaymentDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Flats_FlatId",
                        column: x => x.FlatId,
                        principalTable: "Flats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Birthdate = table.Column<DateTime>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    UserRole = table.Column<int>(nullable: false),
                    FlatId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Flats_FlatId",
                        column: x => x.FlatId,
                        principalTable: "Flats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(nullable: true),
                    ConversationId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "UserAnnouncements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    AnnouncementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnnouncements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnnouncements_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnnouncements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserConversations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: true),
                    ConversationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserConversations_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserConversations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_AddressId",
                table: "Buildings",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CostId",
                table: "Buildings",
                column: "CostId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_HouseManagerId",
                table: "Buildings",
                column: "HouseManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_HousingDevelopmentId",
                table: "Buildings",
                column: "HousingDevelopmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Damages_BuildingId",
                table: "Damages",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Damages_RequestCreatorId",
                table: "Damages",
                column: "RequestCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Flats_BuildingId",
                table: "Flats",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaHistory_FlatId",
                table: "MediaHistory",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DetailsId",
                table: "Payments",
                column: "DetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FlatId",
                table: "Payments",
                column: "FlatId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnnouncements_AnnouncementId",
                table: "UserAnnouncements",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnnouncements_UserId",
                table: "UserAnnouncements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConversations_ConversationId",
                table: "UserConversations",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConversations_UserId",
                table: "UserConversations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FlatId",
                table: "Users",
                column: "FlatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buildings_Users_HouseManagerId",
                table: "Buildings",
                column: "HouseManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Addresses_AddressId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Cost_CostId",
                table: "Buildings");

            migrationBuilder.DropForeignKey(
                name: "FK_Buildings_Users_HouseManagerId",
                table: "Buildings");

            migrationBuilder.DropTable(
                name: "BlobFiles");

            migrationBuilder.DropTable(
                name: "MediaHistory");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "UserAnnouncements");

            migrationBuilder.DropTable(
                name: "UserConversations");

            migrationBuilder.DropTable(
                name: "Damages");

            migrationBuilder.DropTable(
                name: "PaymentDetail");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Cost");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Flats");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "HousingDevelopments");
        }
    }
}
